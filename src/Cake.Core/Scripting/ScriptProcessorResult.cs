using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    public sealed class ScriptProcessorResult
    {
        private readonly string _code;
        private readonly DirectoryPath _root;
        private readonly List<FilePath> _references;

        public string Code
        {
            get { return _code; }
        }

        public DirectoryPath Root
        {
            get { return _root; }
        }

        public IReadOnlyList<FilePath> References
        {
            get { return _references; }
        }

        public ScriptProcessorResult(string code, DirectoryPath root, IEnumerable<FilePath> references)
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
            _code = code;
            _root = root;
            _references = references as List<FilePath> ?? new List<FilePath>(references);
        }
    }
}