using System.Collections.Generic;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobVisitorContext
    {
        private readonly LinkedList<string> _pathParts;
        private readonly List<IFileSystemInfo> _results;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        internal string FullPath
        {
            get { return string.Join("/", _pathParts); }
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ICakeEnvironment Environment
        {
            get { return _environment; }
        }

        public List<IFileSystemInfo> Results
        {
            get { return _results; }
        }

        public GlobVisitorContext(IFileSystem fileSystem, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _results = new List<IFileSystemInfo>();
            _pathParts = new LinkedList<string>();
        }

        public void AddResult(IFileSystemInfo path)
        {
            _results.Add(path);
        }

        public void Push(string path)
        {
            _pathParts.AddLast(path);
        }

        public void Pop()
        {
            _pathParts.RemoveLast();
        }
    }
}