﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptConventions"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="loader">The assembly loader.</param>
        public ScriptConventions(IFileSystem fileSystem, IAssemblyLoader loader)
        {
            _fileSystem = fileSystem;
            _loader = loader;
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
#endif

            // Return the assemblies.
            return result.ToArray();
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