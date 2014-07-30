using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a result of a processed script.
    /// </summary>
    public sealed class ScriptProcessorResult
    {
        private readonly string _code;
        private readonly DirectoryPath _root;
        private readonly List<FilePath> _references;
        private readonly List<FilePath> _scripts;

        /// <summary>
        /// Gets the script code.
        /// </summary>
        /// <value>The code.</value>
        public string Code
        {
            get { return _code; }
        }

        /// <summary>
        /// Gets the root directory path.
        /// </summary>
        /// <value>The root.</value>
        public DirectoryPath Root
        {
            get { return _root; }
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <value>The references.</value>
        public IReadOnlyList<FilePath> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <value>The references.</value>
        public IReadOnlyList<FilePath> Scripts
        {
            get { return _scripts; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptProcessorResult"/> class.
        /// </summary>
        /// <param name="code">The script code.</param>
        /// <param name="root">The root directory path.</param>
        /// <param name="references">The references.</param>
        /// <param name="scripts">The scripts.</param>
        public ScriptProcessorResult(string code, DirectoryPath root, 
            IEnumerable<FilePath> references, IEnumerable<FilePath> scripts)
        {
            if (code == null)
            {
                throw new ArgumentNullException("code");
            }
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }
            if (root.IsRelative)
            {
                throw new CakeException("Script root cannot be relative.");
            }
            if (references == null)
            {
                throw new ArgumentNullException("references");
            }
            if (scripts == null)
            {
                throw new ArgumentNullException("scripts");
            }
            _code = code;
            _root = root;            
            _references = references as List<FilePath> ?? new List<FilePath>(references);
            _scripts = scripts as List<FilePath> ?? new List<FilePath>(scripts);
        }
    }
}