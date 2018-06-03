﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Diagnostics;

namespace Cake.Scripting.Roslyn
{
    internal sealed class ScriptAssemblyResolver : IDisposable
    {
        private readonly ICakeLog _log;
        private readonly HashSet<string> _resolvedNames = new HashSet<string>();

        public ScriptAssemblyResolver(ICakeLog log)
        {
            _log = log;
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var name = new AssemblyName(args.Name);

            // Prevent recursion from the Assembly.Load() call inside
            if (_resolvedNames.Add(name.Name))
            {
                _log.Verbose($"Resolving assembly {args.Name}");
                try
                {
                    var assembly = AppDomain.CurrentDomain.GetAssemblies()
                        .FirstOrDefault(x => !x.IsDynamic && x.GetName().Name == name.Name)
                        ?? Assembly.Load(name.Name);
                    if (assembly != null)
                    {
                        _log.Verbose($"Resolved {name.Name} by assembly {assembly.FullName}");
                    }
                    else
                    {
                        _log.Verbose($"Assembly {name.Name} not resolved");
                    }
                    return assembly;
                }
                catch (Exception ex)
                {
                    _log.Verbose($"Exception while resolving assembly {name.Name}: {ex.Message}");
                }
            }
            return null;
        }
    }
}