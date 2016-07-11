// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script.
    /// </summary>
    public sealed class Script
    {
        private readonly List<string> _namespaces;
        private readonly List<string> _lines;
        private readonly List<ScriptAlias> _aliases;
        private readonly List<string> _usingAliasDirectives;

        /// <summary>
        /// Gets the namespaces imported via the <c>using</c> statement.
        /// </summary>
        /// <value>The namespaces.</value>
        public IReadOnlyList<string> Namespaces
        {
            get { return _namespaces; }
        }

        /// <summary>
        /// Gets the script lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public IReadOnlyList<string> Lines
        {
            get { return _lines; }
        }

        /// <summary>
        /// Gets the aliases.
        /// </summary>
        /// <value>The aliases.</value>
        public IReadOnlyList<ScriptAlias> Aliases
        {
            get { return _aliases; }
        }

        /// <summary>
        /// Gets the using alias directives.
        /// </summary>
        /// <value>The using alias directives.</value>
        public IReadOnlyList<string> UsingAliasDirectives
        {
            get { return _usingAliasDirectives; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Script" /> class.
        /// </summary>
        /// <param name="namespaces">The namespaces.</param>
        /// <param name="lines">The scrip lines.</param>
        /// <param name="aliases">The script aliases.</param>
        /// <param name="usingAliasDirectives">The using alias directives.</param>
        public Script(
            IEnumerable<string> namespaces,
            IEnumerable<string> lines,
            IEnumerable<ScriptAlias> aliases,
            IEnumerable<string> usingAliasDirectives)
        {
            _namespaces = new List<string>(namespaces ?? Enumerable.Empty<string>());
            _lines = new List<string>(lines ?? Enumerable.Empty<string>());
            _aliases = new List<ScriptAlias>(aliases ?? Enumerable.Empty<ScriptAlias>());
            _usingAliasDirectives = new List<string>(usingAliasDirectives ?? Enumerable.Empty<string>());
        }
    }
}
