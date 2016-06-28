// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Packaging;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents a script analysis result.
    /// </summary>
    public sealed class ScriptAnalyzerResult
    {
        private readonly HashSet<string> _references;
        private readonly HashSet<string> _namespaces;
        private readonly HashSet<string> _usingAliases;

        /// <summary>
        /// Gets the analyzed script.
        /// </summary>
        /// <value>The script.</value>
        public IScriptInformation Script { get; }

        /// <summary>
        /// Gets the merged script lines.
        /// </summary>
        /// <value>The merged script lines.</value>
        public IReadOnlyList<string> Lines { get; }

        /// <summary>
        /// Gets all references.
        /// </summary>
        /// <value>The references.</value>
        public ISet<string> References => _references;

        /// <summary>
        /// Gets all the namespaces.
        /// </summary>
        /// <value>The namespaces.</value>
        public ISet<string> Namespaces => _namespaces;

        /// <summary>
        /// Gets the using aliases.
        /// </summary>
        /// <value>The using aliases.</value>
        public ISet<string> UsingAliases => _usingAliases;

        /// <summary>
        /// Gets the addins.
        /// </summary>
        /// <value>The addins.</value>
        public HashSet<PackageReference> Addins { get; }

        /// <summary>
        /// Gets the tools.
        /// </summary>
        /// <value>The tools.</value>
        public HashSet<PackageReference> Tools { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAnalyzerResult"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="lines">The merged script lines.</param>
        public ScriptAnalyzerResult(IScriptInformation script, IReadOnlyList<string> lines)
        {
            Script = script;
            Lines = lines;
            _references = new HashSet<string>(Collect(script, i => i.References));
            _namespaces = new HashSet<string>(Collect(script, i => i.Namespaces));
            _usingAliases = new HashSet<string>(Collect(script, i => i.UsingAliases));
            Tools = new HashSet<PackageReference>(Collect(script, i => i.Tools));
            Addins = new HashSet<PackageReference>(Collect(script, i => i.Addins));
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