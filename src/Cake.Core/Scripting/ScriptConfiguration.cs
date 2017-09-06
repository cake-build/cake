// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Provides the ability to configure the script via IoC.
    /// </summary>
    public sealed class ScriptConfiguration : IScriptConfiguration
    {
        private readonly List<string> scriptNamespaces = new List<string>();
        private readonly List<string> scriptAssemblyNames = new List<string>();

        /// <summary>
        /// Adds a namespace to import in the script.
        /// </summary>
        /// <param name="namespace">The namespace to import in the script.</param>
        public void AddScriptNamespace(string @namespace) => scriptNamespaces.Add(@namespace);

        /// <summary>
        /// Adds an assembly name to reference in the script.
        /// </summary>
        /// <param name="assemblyName">The assembly name to reference in the script.</param>
        public void AddScriptAssembly(string assemblyName) => scriptAssemblyNames.Add(assemblyName);

        IReadOnlyList<string> IScriptConfiguration.Namespaces => scriptNamespaces;

        IReadOnlyList<string> IScriptConfiguration.AssemblyNames => scriptAssemblyNames;
    }
}
