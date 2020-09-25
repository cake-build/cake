// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.Configuration;
using Cake.Core.IO;
using Cake.Core.Reflection;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script conventions used by Cake.
    /// </summary>
    public sealed class ScriptConventions : IScriptConventions
    {
        private readonly IFileSystem _fileSystem;
        private readonly IAssemblyLoader _loader;
        private readonly ICakeRuntime _runtime;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptConventions"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="loader">The assembly loader.</param>
        /// <param name="runtime">The Cake runtime.</param>
        public ScriptConventions(IFileSystem fileSystem, IAssemblyLoader loader, ICakeRuntime runtime)
        {
            _fileSystem = fileSystem;
            _loader = loader;
            _runtime = runtime;
        }

        /// <summary>
        /// Gets the default namespaces.
        /// </summary>
        /// <returns>A list containing all default namespaces.</returns>
        public IReadOnlyList<string> GetDefaultNamespaces()
        {
            return new List<string>
            {
                "System",
                "System.Collections.Generic",
                "System.Linq",
                "System.Text",
                "System.Threading.Tasks",
                "System.IO",
                "Cake.Core",
                "Cake.Core.IO",
                "Cake.Core.Scripting",
                "Cake.Core.Diagnostics"
            };
        }

        /// <summary>
        /// Gets the default assemblies.
        /// </summary>
        /// <param name="root">The root to where to find Cake related assemblies.</param>
        /// <returns>A list containing all default assemblies.</returns>
        public IReadOnlyList<Assembly> GetDefaultAssemblies(DirectoryPath root)
        {
            // Prepare the default assemblies.
            var result = new HashSet<Assembly>(new SimpleAssemblyComparer());
            result.Add(typeof(Action).GetTypeInfo().Assembly); // mscorlib or System.Private.Core
            result.Add(typeof(IQueryable).GetTypeInfo().Assembly); // System.Core or System.Linq.Expressions
            result.Add(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly); // Dynamic support

            // Load other Cake-related assemblies that we need.
            var cakeAssemblies = LoadCakeAssemblies(root);
            result.AddRange(cakeAssemblies);

#if NETCORE
            // Load all referenced assemblies.
            foreach (var cakeAssembly in cakeAssemblies)
            {
                foreach (var reference in cakeAssembly.GetReferencedAssemblies())
                {
                    result.Add(_loader.Load(reference));
                }
            }
#else
            result.Add(typeof(Uri).GetTypeInfo().Assembly); // System
            result.Add(typeof(System.Xml.XmlReader).GetTypeInfo().Assembly); // System.Xml
            result.Add(typeof(System.Xml.Linq.XDocument).GetTypeInfo().Assembly); // System.Xml.Linq
            result.Add(typeof(System.Data.DataTable).GetTypeInfo().Assembly); // System.Data

            // This is just to please Roslyn when running under Mono. See issue https://github.com/dotnet/roslyn/issues/19364
            result.Add(_loader.Load(new AssemblyName("System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"))); // System.Runtime
            result.Add(_loader.Load(new AssemblyName("System.Collections, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"))); // System.Collections
            result.Add(_loader.Load(new AssemblyName("System.Net.Http, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"))); // System.Net.Http

            try
            {
                result.Add(_loader.Load(new AssemblyName("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51"))); // NETStandard.Library
            }
            catch
            {
                /*
                 * Silently continue instead and let it blow up during runtime if netstandard assembly was not found.
                 * TODO: Log that netstandard assembly was not found.
                 * Unfortunately, logger is not available in this class, and that would be too big of a change for the 0.26.1 hotfix release.
                 */
            }
#endif

            // Return the assemblies.
            return result.ToArray();
        }

        /// <summary>
        /// Gets the default defines.
        /// </summary>
        /// <returns>A list containing all default defines.</returns>
        public IReadOnlyList<string> GetDefaultDefines()
        {
            return new[]
            {
                "#define CAKE",
                _runtime.IsCoreClr ? "#define NETCOREAPP" : "#define NETFRAMEWORK",
                $"#define {GetFrameworkDefine()}"
            };
        }

        private string GetFrameworkDefine()
        {
            switch (_runtime.BuiltFramework.FullName)
            {
                case ".NETFramework,Version=v4.6.1":
                    return "NET461";

                case ".NETCoreApp,Version=v2.0":
                    return "NETCOREAPP2_0";

                case ".NETCoreApp,Version=v2.1":
                    return "NETCOREAPP2_1";

                case ".NETCoreApp,Version=v2.2":
                    return "NETCOREAPP2_2";

                case ".NETCoreApp,Version=v3.0":
                    return "NETCOREAPP3_0";

                case ".NETCoreApp,Version=v3.1":
                    return "NETCOREAPP3_1";

                case ".NETCoreApp,Version=v5.0":
                    return "NET5_0";

                default:
                    Console.Error.WriteLine(_runtime.BuiltFramework.FullName);
                    return "NETSTANDARD2_0";
            }
        }

        private List<Assembly> LoadCakeAssemblies(DirectoryPath root)
        {
            var result = new List<Assembly>();
            var assemblyDirectory = _fileSystem.GetDirectory(root);
            foreach (var pattern in GetCakeAssemblyNames())
            {
                var cakeAssemblies = assemblyDirectory.GetFiles(pattern, SearchScope.Current);
                foreach (var cakeAssembly in cakeAssemblies)
                {
                    result.Add(_loader.Load(cakeAssembly.Path, false));
                }
            }
            return result;
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Local
        private static string[] GetCakeAssemblyNames()
        {
#if NETCORE
            return new[] { "Cake.Core.dll", "Cake.Common.dll" };
#else
            return new[] { "Cake.Core.dll", "Cake.Common.dll", "Cake.exe" };
#endif
        }
    }
}