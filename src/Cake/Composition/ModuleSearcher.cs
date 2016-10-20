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
using Cake.Core.Reflection;

namespace Cake.Composition
{
    internal sealed class ModuleSearcher
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IAssemblyLoader _assemblyLoader;
        private readonly ICakeLog _log;

        public ModuleSearcher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IAssemblyLoader assemblyLoader,
            ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _assemblyLoader = assemblyLoader;
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
                var module = LoadModule(file.Path);
                if (module != null)
                {
                    result.Add(module);
                }
            }

            return result;
        }

        private Type LoadModule(FilePath path)
        {
            try
            {
                var assembly = LoadAssembly(path);

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
            catch
            {
                _log.Warning("Could not load module '{0}'.", path.FullPath);
                return null;
            }
        }

        private Assembly LoadAssembly(FilePath path)
        {
            VerifyCompatibility(path);
            return _assemblyLoader.Load(path);
        }

        private static void VerifyCompatibility(FilePath path)
        {
#if !NETCORE
            // Make sure that the module is compatible.
            // Kind of hackish, but this will have to do until we figure out a better way...
            var assembly = Assembly.ReflectionOnlyLoadFrom(path.FullPath);
            var references = assembly.GetReferencedAssemblies();
            foreach (var reference in references)
            {
                if (reference.Name != null && reference.Name.Equals("Cake.Core", StringComparison.OrdinalIgnoreCase))
                {
                    var minVersion = new Version(0, 16, 0);
                    if (reference.Version < minVersion)
                    {
                        const string format = "The module '{0}' is targeting an incompatible version of Cake.Core.dll. It needs to target at least version {1}.";
                        throw new CakeException(string.Format(format, path.GetFilename().FullPath, minVersion.ToString(3)));
                    }
                }
            }
#endif
        }
    }
}