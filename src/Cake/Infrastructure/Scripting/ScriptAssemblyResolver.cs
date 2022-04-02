// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Infrastructure.Scripting
{
    public sealed class ScriptAssemblyResolver : IDisposable
    {
        private const string AssemblyResourcesExtension = ".resources";
        private static readonly Version VersionZero = new Version(0, 0, 0, 0);

        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        private readonly Lazy<bool> _shouldTryResolveNeutral;
        private readonly HashSet<string> _resolvedNames = new HashSet<string>();

        public ScriptAssemblyResolver(ICakeEnvironment environment, ICakeLog log)
        {
            _environment = environment;
            _log = log;

            _shouldTryResolveNeutral = new Lazy<bool>(GetShouldTryResolveNeutral);

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var fullName = args?.Name;
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            var assemblyName = new AssemblyName(fullName);
            var shortName = assemblyName.Name;
            var version = assemblyName.Version;

            if (version == VersionZero)
            {
                return null;
            }

            // Preventing indirect recursive calls via Assembly.Load()
            if (!_resolvedNames.Add(shortName + version))
            {
                return null;
            }

            _log.Debug($"Resolving assembly '{fullName}' using runtime installed at '{RuntimeEnvironment.GetRuntimeDirectory()}'...");
            return AssemblyResolve(assemblyName);
        }

        private Assembly AssemblyResolve(AssemblyName assemblyName)
        {
            var fullName = assemblyName.FullName;
            var shortName = assemblyName.Name;
            var version = assemblyName.Version;

            if (version == VersionZero)
            {
                return null;
            }

            Assembly assembly = null;
            try
            {
                assembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => !x.IsDynamic
                        && x.GetName().Name == shortName
                        && x.GetName().Version == version)
                    ?? Assembly.Load(fullName);
            }
            catch (Exception ex)
            {
                _log.Debug($"Exception occurred while resolving assembly {shortName}: {ex.Message}");
            }

            if (assembly != null)
            {
                _log.Debug($"Assembly {shortName} resolved as '{assembly.FullName}' (file location: '{assembly.Location}')");
                return assembly;
            }

            if (_shouldTryResolveNeutral.Value)
            {
                // This occurs when current culture differs from assembly neutral culture
                if (shortName.EndsWith(AssemblyResourcesExtension))
                {
                    assemblyName.Name = shortName.Remove(shortName.Length - AssemblyResourcesExtension.Length);

                    _log.Debug($"Trying to resolve assembly {shortName} as '{assemblyName.FullName}'...");
                    return AssemblyResolve(assemblyName);
                }
            }

            _log.Debug($"Assembly '{fullName}' not resolved");
            return null;
        }

        private bool GetShouldTryResolveNeutral()
        {
            // Since .NET Core 3.0
            var runtimeVersionMajor = _environment.Runtime.BuiltFramework.Version.Major;
            return runtimeVersionMajor >= 5 || (runtimeVersionMajor == 3 && _environment.Runtime.IsCoreClr);
        }
    }
}