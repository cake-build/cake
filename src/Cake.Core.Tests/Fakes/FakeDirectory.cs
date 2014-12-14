using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Core.Tests.Fakes
{
    public sealed class FakeDirectory : IDirectory
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly DirectoryPath _path;
        private readonly bool _creatable;
        private bool _exist;

        public DirectoryPath Path
        {
            get { return _path; }
        }

        public string Name
        {
            get { return _path.Segments.Last(); }
        }

        public bool Exists
        {
            get { return _exist; }
            set { _exist = value; }
        }

        public FakeDirectory(FakeFileSystem fileSystem, DirectoryPath path, bool creatable)
        {
            _fileSystem = fileSystem;
            _path = path;
            _exist = false;
            _creatable = creatable;
        }

        public void Create()
        {
            if (_creatable)
            {
                _exist = true;
            }
        }

        public void Delete(bool recursive)
        {
            if (recursive)
            {
                foreach (var directory in GetDirectories("*", SearchScope.Current))
                {
                    directory.Delete(true);
                }
                foreach (var file in GetFiles("*", SearchScope.Current))
                {
                    file.Delete();
                }
            }
            _exist = false;
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var result = new List<IDirectory>();
            var children = _fileSystem.Directories.Where(x => x.Key.FullPath.StartsWith(_path.FullPath + "/", StringComparison.OrdinalIgnoreCase));
            foreach (var child in children.Where(c => c.Value.Exists))
            {
                var relative = child.Key.FullPath.Substring(_path.FullPath.Length + 1);
                if (!relative.Contains("/"))
                {
                    result.Add(child.Value);
                }
            }
            return result;
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var result = new List<IFile>();
            var children = _fileSystem.Files.Where(x => x.Key.FullPath.StartsWith(_path.FullPath + "/", StringComparison.OrdinalIgnoreCase));
            foreach (var child in children.Where(c => c.Value.Exists))
            {
                var relative = child.Key.FullPath.Substring(_path.FullPath.Length + 1);
                if (!relative.Contains("/"))
                {
                    result.Add(child.Value);
                }
            }
            return result;
        }
    }
}