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
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Composition
{
    internal sealed class ModuleSearcher
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        public ModuleSearcher(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        public IReadOnlyList<Type> Search(DirectoryPath path)
        {
            path = path.MakeAbsolute(_environment);
            var root = _fileSystem.GetDirectory(path);
            if (!root.Exists)
            {
                _log.Debug("Module directory does not exist.");
                return new Type[] { };
            }

            var result = new List<Type>();
            var files = root.GetFiles("Cake.*.Module.dll", SearchScope.Recursive);
            foreach (var file in files)
            {
                if (ShouldLoadModule(file.Path))
                {
                    var module = LoadModule(file.Path);
                    if (module != null)
                    {
                        result.Add(module);
                    }
                }
            }

            return result;
        }

        private static bool ShouldLoadModule(FilePath path)
        {
            try
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;
                var assembly = Assembly.ReflectionOnlyLoadFrom(path.FullPath);
                var attributes = CustomAttributeData.GetCustomAttributes(assembly);
                foreach (var attribute in attributes)
                {
                    if (attribute.AttributeType.FullName == typeof(CakeModuleAttribute).FullName)
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                // Unregister reflection-only assembly resolve.
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= CurrentDomain_ReflectionOnlyAssemblyResolve;
            }
        }

        private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }

        private Type LoadModule(FilePath path)
        {
            var assembly = Assembly.LoadFrom(path.FullPath);

            var attribute = assembly.GetCustomAttributes<CakeModuleAttribute>().FirstOrDefault();
            if (attribute == null)
            {
                _log.Warning("The assembly '{0}' does not have module metadata.", path.FullPath);
                return null;
            }

            if (!typeof(ICakeModule).IsAssignableFrom(attribute.ModuleType))
            {
                _log.Warning("The module type '{0}' is not an actual module.", attribute.ModuleType.FullName);
                return attribute.ModuleType;
            }

            return attribute.ModuleType;
        }
    }
}
