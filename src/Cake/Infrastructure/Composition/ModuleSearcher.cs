// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Reflection;

namespace Cake.Infrastructure.Composition
{
    public interface IModuleSearcher
    {
        ICollection<Type> FindModuleTypes(DirectoryPath root, ICakeConfiguration configuration);
    }

    public sealed class ModuleSearcher : IModuleSearcher
    {
        private static readonly Dictionary<string, string> _excludedModules = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Cake.DotNetTool.Module", "Cake.DotNetTool.Module is now included with Cake, so should no longer be installed separately to module directory or using #module directive" }
            };

        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        public ModuleSearcher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        public ICollection<Type> FindModuleTypes(DirectoryPath root, ICakeConfiguration configuration)
        {
            var modulePath = _fileSystem.GetDirectory(configuration.GetModulePath(root, _environment));
            if (!modulePath.Exists)
            {
                _log.Debug("Module directory does not exist.");
                return Array.Empty<Type>();
            }

            var result = new List<Type>();
            var files = modulePath.GetFiles("Cake.*.Module.dll", SearchScope.Recursive);
            foreach (var file in files)
            {
                var module = LoadModule(file.Path, configuration);
                if (module != null)
                {
                    result.Add(module);
                }
            }

            return result;
        }

        private Type LoadModule(FilePath path, ICakeConfiguration configuration)
        {
            try
            {
                if (_excludedModules.TryGetValue(path.GetFilenameWithoutExtension().FullPath, out var message))
                {
                    _log.Warning("{0}, Assembly {1} is excluded from loading.", message, path);
                    return null;
                }

                var loader = new AssemblyLoader(_environment, _fileSystem, new AssemblyVerifier(configuration, _log));
                var assembly = loader.Load(path, true);

                var attribute = assembly.GetCustomAttributes<CakeModuleAttribute>().FirstOrDefault();
                if (attribute == null)
                {
                    _log.Warning("The assembly '{0}' does not have module metadata.", path.FullPath);
                    return null;
                }

                if (!typeof(ICakeModule).IsAssignableFrom(attribute.ModuleType))
                {
                    _log.Warning("The module type '{0}' is not an actual module.", attribute.ModuleType.FullName);
                    return null;
                }

                return attribute.ModuleType;
            }
            catch (CakeException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _log.Warning("Could not load module '{0}'. {1}", path.FullPath, ex);
                return null;
            }
        }
    }
}
