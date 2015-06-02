using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake
{
    internal sealed class CakeModuleFinder
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        public CakeModuleFinder(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        public IReadOnlyCollection<ICakeModule> FindModules(CakeOptions options)
        {
            var result = new List<ICakeModule>();
            if (options != null)
            {
                if (options.Module != null)
                {
                    var modulePath = options.Module.MakeAbsolute(_environment);
                    foreach (var module in FindModules(modulePath))
                    {
                        result.Add(module);

                        _log.Information("Loaded module {0} in assembly {1}.", 
                            module.GetType().Name, 
                            modulePath.GetFilename().FullPath);
                    }
                }
            }
            return result;
        }

        private IEnumerable<ICakeModule> FindModules(FilePath path)
        {
            var result = new List<ICakeModule>();
            foreach (var type in GetLoadableTypes(path))
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof(ICakeModule).IsAssignableFrom(type))
                    {
                        // Got an empty constructor?
                        var emptyConstructor = type.GetConstructor(Type.EmptyTypes);
                        if (emptyConstructor != null)
                        {
                            result.Add((ICakeModule)Activator.CreateInstance(type));
                        }
                    }
                }
            }
            return result;
        }

        private IEnumerable<Type> GetLoadableTypes(FilePath path)
        {
            path = path.MakeAbsolute(_environment);
            if (_fileSystem.Exist(path))
            {
                try
                {
                    var name = AssemblyName.GetAssemblyName(path.FullPath);
                    var assembly = AppDomain.CurrentDomain.Load(name);
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        return e.Types.Where(t => t != null);
                    }
                }
                catch (BadImageFormatException)
                {
                }
            }
            return Enumerable.Empty<Type>();
        }
    }
}
