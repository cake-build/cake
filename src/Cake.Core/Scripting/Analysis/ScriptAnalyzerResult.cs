// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Processors;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents a script analysis result.
    /// </summary>
    public sealed class ScriptAnalyzerResult
    {
        private readonly IScriptInformation _script;
        private readonly IReadOnlyList<string> _lines;
        private readonly HashSet<string> _references;
        private readonly HashSet<string> _namespaces;
        private readonly HashSet<string> _usingAliases;
        private readonly HashSet<PackageReference> _addins;
        private readonly HashSet<PackageReference> _tools;
        private readonly HashSet<PackageReference> _nugetScripts;
        private readonly ProcessorValues _processorValues;

        /// <summary>
        /// Gets the analyzed script.
        /// </summary>
        /// <value>The script.</value>
        public IScriptInformation Script
        {
            get { return _script; }
        }

        /// <summary>
        /// Gets the merged script lines.
        /// </summary>
        /// <value>The merged script lines.</value>
        public IReadOnlyList<string> Lines
        {
            get { return _lines; }
        }

        /// <summary>
        /// Gets all references.
        /// </summary>
        /// <value>The references.</value>
        public ISet<string> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Gets all the namespaces.
        /// </summary>
        /// <value>The namespaces.</value>
        public ISet<string> Namespaces
        {
            get { return _namespaces; }
        }

        /// <summary>
        /// Gets the using aliases.
        /// </summary>
        /// <value>The using aliases.</value>
        public ISet<string> UsingAliases
        {
            get { return _usingAliases; }
        }

        /// <summary>
        /// Gets the addins.
        /// </summary>
        /// <value>The addins.</value>
        public HashSet<PackageReference> Addins
        {
            get { return _addins; }
        }

        /// <summary>
        /// Gets the tools.
        /// </summary>
        /// <value>The tools.</value>
        public HashSet<PackageReference> Tools
        {
            get { return _tools; }
        }

        /// <summary>
        /// Gets the scripts.
        /// </summary>
        /// <value>The scripts.</value>
        public HashSet<PackageReference> NugetScripts
        {
            get { return _nugetScripts; }
        }

        /// <summary>
        /// Gets <see cref="ProcessorValues"/>s for a processor.
        /// </summary>
        public ProcessorValues ProcessorValues
        {
            get
            {
                return _processorValues;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAnalyzerResult"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="lines">The merged script lines.</param>
        public ScriptAnalyzerResult(IScriptInformation script, IReadOnlyList<string> lines)
        {
            _script = script;
            _lines = lines;
            _references = new HashSet<string>(Collect(script, i => i.References));
            _namespaces = new HashSet<string>(Collect(script, i => i.Namespaces));
            _usingAliases = new HashSet<string>(Collect(script, i => i.UsingAliases));
            _tools = new HashSet<PackageReference>(Collect(script, i => i.Tools));
            _addins = new HashSet<PackageReference>(Collect(script, i => i.Addins));
            _nugetScripts = new HashSet<PackageReference>(Collect(script, i => i.NugetScripts));
            _processorValues = new ProcessorValues(Collect(script, i => i.ProcessorValues));
        }

        private IEnumerable<T> Collect<T>(IScriptInformation script, Func<IScriptInformation, IEnumerable<T>> collector)
        {
            var stack = new Stack<IScriptInformation>();
            stack.Push(script);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                foreach (var include in current.Includes)
                {
                    stack.Push(include);
                }

                foreach (var item in collector(current))
                {
                    yield return item;
                }
            }
        }
    }
}
