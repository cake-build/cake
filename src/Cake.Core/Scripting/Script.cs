// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script.
    /// </summary>
    public sealed class Script
    {
        /// <summary>
        /// Gets the namespaces imported via the <c>using</c> statement.
        /// </summary>
        /// <value>The namespaces.</value>
        public IReadOnlyList<string> Namespaces { get; }

        /// <summary>
        /// Gets the namespaces flagged to be excluded by code generation and affected aliases.
        /// </summary>
        public IDictionary<string, IList<string>> ExcludedNamespaces { get;  }

        /// <summary>
        /// Gets the script lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public IReadOnlyList<string> Lines { get; }

        /// <summary>
        /// Gets the aliases.
        /// </summary>
        /// <value>The aliases.</value>
        public IReadOnlyList<ScriptAlias> Aliases { get; }

        /// <summary>
        /// Gets the using alias directives.
        /// </summary>
        /// <value>The using alias directives.</value>
        public IReadOnlyList<string> UsingAliasDirectives { get; }

        /// <summary>
        /// Gets the using static directives.
        /// </summary>
        /// <value>The using static directives.</value>
        public IReadOnlyList<string> UsingStaticDirectives { get; }

        /// <summary>
        /// Gets the defines.
        /// </summary>
        /// <value>The defines.</value>
        public IReadOnlyList<string> Defines { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Script" /> class.
        /// </summary>
        /// <param name="namespaces">The namespaces.</param>
        /// <param name="lines">The scrip lines.</param>
        /// <param name="aliases">The script aliases.</param>
        /// <param name="usingAliasDirectives">The using alias directives.</param>
        /// <param name="usingStaticDirectives">The using static directives.</param>
        /// <param name="defines">The defines.</param>
        public Script(
            IEnumerable<string> namespaces,
            IEnumerable<string> lines,
            IEnumerable<ScriptAlias> aliases,
            IEnumerable<string> usingAliasDirectives,
            IEnumerable<string> usingStaticDirectives,
            IEnumerable<string> defines)
        {
            Namespaces = new List<string>(namespaces ?? Enumerable.Empty<string>());
            Lines = new List<string>(lines ?? Enumerable.Empty<string>());
            Aliases = new List<ScriptAlias>(aliases ?? Enumerable.Empty<ScriptAlias>());
            UsingAliasDirectives = new List<string>(usingAliasDirectives ?? Enumerable.Empty<string>());
            UsingStaticDirectives = new List<string>(usingStaticDirectives ?? Enumerable.Empty<string>());
            Defines = new List<string>(defines ?? Enumerable.Empty<string>());
            ExcludedNamespaces = new Dictionary<string, IList<string>>(StringComparer.Ordinal);
        }
    }
}