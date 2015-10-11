using System;
using System.Collections.Generic;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Acts as a context, keeping track of loaded scripts,
    /// references and namespaces to add and script content.
    /// </summary>
    public sealed class ScriptProcessorContext
    {
        private readonly HashSet<string> _processedScripts;
        private readonly HashSet<string> _references;
        private readonly HashSet<string> _namespaces;
        private readonly LinkedList<string> _lines;
        private readonly HashSet<ScriptAlias> _aliases;
        private readonly LinkedList<string> _usingAliasDirectives;

        /// <summary>
        /// Gets the script's assembly references
        /// referenced via the <c>#r</c> directive.
        /// </summary>
        /// <value>The references.</value>
        public HashSet<string> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Gets the script's namespaces imported
        /// via the <c>using</c> statement.
        /// </summary>
        /// <value>The namespaces.</value>
        public HashSet<string> Namespaces
        {
            get { return _namespaces; }
        }

        /// <summary>
        /// Gets the scripts that has been processed so far.
        /// </summary>
        /// <value>The processed scripts.</value>
        public HashSet<string> ProcessedScripts
        {
            get { return _processedScripts; }
        }

        /// <summary>
        /// Gets the script lines.
        /// </summary>
        /// <value>
        /// The lines.
        /// </value>
        public LinkedList<string> Lines
        {
            get { return _lines; }
        }

        /// <summary>
        /// Gets the aliases.
        /// </summary>
        /// <value>The aliases.</value>
        public HashSet<ScriptAlias> Aliases
        {
            get { return _aliases; }
        }

        /// <summary>
        /// Gets the using alias directives.
        /// </summary>
        /// <value>The using alias directives.</value>
        public LinkedList<string> UsingAliasDirectives
        {
            get { return _usingAliasDirectives; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptProcessorContext"/> class.
        /// </summary>
        public ScriptProcessorContext()
        {
            _processedScripts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _references = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _namespaces = new HashSet<string>(StringComparer.Ordinal);
            _lines = new LinkedList<string>();
            _aliases = new HashSet<ScriptAlias>();
            _usingAliasDirectives = new LinkedList<string>();
        }

        /// <summary>
        /// Determines whether the specified script has been processed.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <returns><c>true</c> if the script has been processed; else <c>false</c>.</returns>
        public bool HasScriptBeenProcessed(string script)
        {
            return _processedScripts.Contains(script);
        }

        /// <summary>
        /// Marks the script as processed.
        /// </summary>
        /// <param name="script">The script to mark as processed.</param>
        internal void MarkScriptAsProcessed(string script)
        {
            _processedScripts.Add(script);
        }

        /// <summary>
        /// Adds an assembly reference to be loaded later.
        /// </summary>
        /// <param name="reference">The assembly reference.</param>
        public void AddReference(string reference)
        {
            _references.Add(reference);
        }

        /// <summary>
        /// Adds a namespace to be loaded later.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
        public void AddNamespace(string @namespace)
        {
            _namespaces.Add(@namespace);
        }

        /// <summary>
        /// Adds a script alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        public void AddScriptAlias(ScriptAlias alias)
        {
            _aliases.Add(alias);
        }

        /// <summary>
        /// Appends a line to the script.
        /// </summary>
        /// <param name="line">The line to add.</param>
        public void AppendScriptLine(string line)
        {
            _lines.AddLast(line);
        }

        /// <summary>
        /// Adds a using alias directive.
        /// </summary>
        /// <param name="line">The using alias directive.</param>
        public void AddUsingAliasDirective(string line)
        {
            _usingAliasDirectives.AddLast(line);
        }
    }
}