using System.Collections.Generic;

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

        /// <summary>
        /// Gets the script's namespaces imported
        /// via the <c>using</c> statement.
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
        /// Initializes a new instance of the <see cref="Script" /> class.
        /// </summary>
        /// <param name="namespaces">The namespaces.</param>
        /// <param name="lines">The scrip lines.</param>
        /// <param name="aliases">The script aliases.</param>
        public Script(IEnumerable<string> namespaces, IEnumerable<string> lines, IEnumerable<ScriptAlias> aliases)
        {
            _namespaces = new List<string>(namespaces);
            _lines = new List<string>(lines);
            _aliases = new List<ScriptAlias>(aliases);
        }
    }
}
