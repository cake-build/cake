// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// The script conventions used by Cake.
    /// </summary>
    public sealed class ScriptConventions : IScriptConventions
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptConventions"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public ScriptConventions(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Gets the default namespaces.
        /// </summary>
        /// <returns>A list containing all default namespaces.</returns>
        public IReadOnlyList<string> GetDefaultNamespaces()
        {
            return new List<string>
            {
                "System", "System.Collections.Generic", "System.Linq",
                "System.Text", "System.Threading.Tasks", "System.IO",
                "Cake.Core", "Cake.Core.IO", "Cake.Core.Scripting", "Cake.Core.Diagnostics"
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
            var defaultAssemblies = new List<Assembly>
            {
                typeof(Action).GetTypeInfo().Assembly, // mscorlib
                typeof(Uri).GetTypeInfo().Assembly, // System
                typeof(IQueryable).GetTypeInfo().Assembly, // System.Core
                typeof(System.Data.DataTable).GetTypeInfo().Assembly, // System.Data
                typeof(System.Xml.XmlReader).GetTypeInfo().Assembly, // System.Xml
                typeof(System.Xml.Linq.XDocument).GetTypeInfo().Assembly, // System.Xml.Linq
            };

            // Load other Cake-related assemblies that we need.
            var assemblyDirectory = _fileSystem.GetDirectory(root);
            var patterns = new[] { "Cake.Core.dll", "Cake.Common.dll", "Cake.exe" };
            foreach (var pattern in patterns)
            {
                var cakeAssemblies = assemblyDirectory.GetFiles(pattern, SearchScope.Current);
                foreach (var cakeAssembly in cakeAssemblies)
                {
                    var assembly = Assembly.LoadFrom(cakeAssembly.Path.FullPath);
                    defaultAssemblies.Add(assembly);
                }
            }

            // Return the assemblies.
            return defaultAssemblies;
        }
    }
}
