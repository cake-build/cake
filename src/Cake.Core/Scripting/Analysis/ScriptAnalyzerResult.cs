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
        private readonly HashSet<string> _usingStaticDirectives;
        private readonly HashSet<string> _defines;

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
        /// Gets all types referenced with the using static directive.
        /// </summary>
        public ISet<string> UsingStaticDirectives => _usingStaticDirectives;

        /// <summary>
        /// Gets the defines.
        /// </summary>
        /// <value>The defines.</value>
        public ISet<string> Defines => _defines;

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
        /// Gets the modules.
        /// </summary>
        /// <value>The modules.</value>
        public HashSet<PackageReference> Modules { get; }

        /// <summary>
        /// Gets a value indicating whether to analysis succeeded without errors.
        /// </summary>
        public bool Succeeded { get; }

        /// <summary>
        /// Gets the list of analyzer errors.
        /// </summary>
        public IReadOnlyList<ScriptAnalyzerError> Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAnalyzerResult"/> class.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="lines">The merged script lines.</param>
        /// <param name="errors">The analyzer errors.</param>
        public ScriptAnalyzerResult(IScriptInformation script, IReadOnlyList<string> lines, IReadOnlyList<ScriptAnalyzerError> errors = null)
        {
            Script = script;
            Lines = lines;
            _references = new HashSet<string>(Collect(script, i => i.References));
            _namespaces = new HashSet<string>(Collect(script, i => i.Namespaces));
            _usingAliases = new HashSet<string>(Collect(script, i => i.UsingAliases));
            _usingStaticDirectives = new HashSet<string>(Collect(script, i => i.UsingStaticDirectives));
            _defines = new HashSet<string>(Collect(script, i => i.Defines));
            Tools = new HashSet<PackageReference>(Collect(script, i => i.Tools));
            Addins = new HashSet<PackageReference>(Collect(script, i => i.Addins));
            Modules = new HashSet<PackageReference>(Collect(script, i => i.Modules));
            Errors = errors ?? new List<ScriptAnalyzerError>(0);
            Succeeded = Errors.Count == 0;
        }

        private static IEnumerable<T> Collect<T>(IScriptInformation script, Func<IScriptInformation, IEnumerable<T>> collector)
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