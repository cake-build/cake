using System;
using System.Linq;

namespace Cake.Core.IO
{
    public abstract class Path
    {
        private readonly string _path;
        private readonly bool _isRelative;
        private readonly string[] _segments;
        private static readonly char[] _invalidPathCharacters;

        public string FullPath
        {
            get { return _path; }
        }

        public bool IsRelative
        {
            get { return _isRelative; }
        }

        public string[] Segments
        {
            get { return _segments; }
        }

        protected Path(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be empty.", "path");
            }         

            _path = path.Replace('\\', '/').Trim();
            _path = _path == "./" ? string.Empty : _path;

            // Remove relative part of a path.
            if (_path.StartsWith("./", StringComparison.Ordinal))
            {
                _path = _path.Substring(2);
            }

            _isRelative = !System.IO.Path.IsPathRooted(_path);

            // Extract path segments.
            _segments = _path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // Validate the path.
            foreach (var character in path)
            {
                if (_invalidPathCharacters.Contains(character))
                {
                    const string format = "Illegal characters in directory path ({0}).";
                    throw new ArgumentException(string.Format(format, character), "path");
                }
            }
        }

        static Path()
        {
            _invalidPathCharacters = System.IO.Path.GetInvalidPathChars().Concat(new[] { '*', '?' }).ToArray();
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}