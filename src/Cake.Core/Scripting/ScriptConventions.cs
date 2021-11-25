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
        private readonly IReferenceAssemblyResolver _referenceAssemblyResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptConventions"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="loader">The assembly loader.</param>
        /// <param name="runtime">The Cake runtime.</param>
        /// <param name="referenceAssemblyResolver">The reference assembly resolver.</param>
        public ScriptConventions(IFileSystem fileSystem, IAssemblyLoader loader, ICakeRuntime runtime, IReferenceAssemblyResolver referenceAssemblyResolver)
        {
            _fileSystem = fileSystem;
            _loader = loader;
            _runtime = runtime;
            _referenceAssemblyResolver = referenceAssemblyResolver;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IReadOnlyList<Assembly> GetDefaultAssemblies(DirectoryPath root)
        {
            // Prepare the default assemblies.
            var result = new HashSet<Assembly>(new SimpleAssemblyComparer());
            result.Add(typeof(Action).GetTypeInfo().Assembly); // mscorlib or System.Private.Core
            result.Add(typeof(IQueryable).GetTypeInfo().Assembly); // System.Core or System.Linq.Expressions
            result.Add(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly); // Dynamic support

            result.AddRange(_referenceAssemblyResolver.GetReferenceAssemblies());

            // Load other Cake-related assemblies that we need.
            var cakeAssemblies = LoadCakeAssemblies(root);
            result.AddRange(cakeAssemblies);

            // Load all referenced assemblies.
            foreach (var cakeAssembly in cakeAssemblies)
            {
                foreach (var reference in cakeAssembly.GetReferencedAssemblies())
                {
                    result.Add(_loader.Load(reference));
                }
            }

            // Return the assemblies.
            return result.ToArray();
        }

        /// <inheritdoc/>
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

                case ".NETCoreApp,Version=v6.0":
                    return "NET6_0";

                default:
                    Console.Error.WriteLine(_runtime.BuiltFramework.FullName);
                    Console.Error.Flush();

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
            => new[] { "Cake.Core.dll", "Cake.Common.dll", "Spectre.Console.dll" };
    }
}