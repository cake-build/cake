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
        private readonly LinkedList<string> _aliases;

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
        /// Initializes a new instance of the <see cref="ScriptProcessorContext"/> class.
        /// </summary>
        public ScriptProcessorContext()
        {
            _processedScripts = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _references = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _namespaces = new HashSet<string>(StringComparer.Ordinal);
            _lines = new LinkedList<string>();
            _aliases = new LinkedList<string>();
        }

        /// <summary>
        /// Determines whether the specified script has been processed.
        /// </summary>
        /// <param name="scriptTokenId">The script's token id.</param>
        /// <returns><c>true</c> if the script has been processed; else <c>false</c>.</returns>
        public bool HasScriptBeenProcessed(string scriptTokenId)
        {
            return _processedScripts.Contains(scriptTokenId);
        }

        /// <summary>
        /// Marks the script as processed.
        /// </summary>
        /// <param name="scriptTokenId">The script's token id to mark as processed.</param>
        public void MarkScriptAsProcessed(string scriptTokenId)
        {
            _processedScripts.Add(scriptTokenId);
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
        /// Adds the code for a script alias.
        /// </summary>
        /// <param name="code">The code to add.</param>
        public void AddScriptAliasCode(string code)
        {
            _aliases.AddLast(code);
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
        /// Gets the script code.
        /// </summary>
        /// <returns>The script code.</returns>
        public string GetScriptCode()
        {
            var aliases = string.Join("\r\n", _aliases);
            var code = string.Join("\r\n", _lines);
            if (!string.IsNullOrWhiteSpace(aliases))
            {
                return string.Join("\r\n", aliases, code);
            }
            return code;
        }
    }
}