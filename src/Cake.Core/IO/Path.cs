using System;

namespace Cake.Core.IO
{
    public abstract class Path
    {
        private readonly string _path;
        private readonly bool _isRelative;
        private readonly string[] _segments;

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
        }

        public override string ToString()
        {
            return FullPath;
        }
    }
}