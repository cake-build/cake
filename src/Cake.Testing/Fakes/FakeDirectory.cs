using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Testing.Fakes
{
    public sealed class FakeDirectory : IDirectory
    {
        private readonly FakeFileSystem _fileSystem;
        private readonly DirectoryPath _path;
        private readonly bool _creatable;
        private bool _exist;
        private readonly bool _hidden;

        public DirectoryPath Path
        {
            get { return _path; }
        }

        Core.IO.Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _exist; }
        }

        public bool Hidden
        {
            get { return _exist && _hidden; }
        }

        public FakeDirectory(FakeFileSystem fileSystem, DirectoryPath path, bool creatable, bool hidden)
        {
            _fileSystem = fileSystem;
            _path = path;
            _exist = false;
            _creatable = creatable;
            _hidden = hidden;
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

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate)
        {
            return GetDirectories(filter, scope, wherePredicate, null);
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
        {
            return GetDirectories(filter, scope)
                .Where(entry => WherePredicate(entry, wherePredicate, predicateFiltered));
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

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate)
        {
            return GetFiles(filter, scope, wherePredicate, null);
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
        {
            return GetFiles(filter, scope)
                .Where(entry=>WherePredicate(entry, wherePredicate, predicateFiltered));
        }

        private static bool WherePredicate(IFileSystemInfo entry, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
        {
            var include = wherePredicate==null || wherePredicate(entry);
            if (!include && predicateFiltered != null)
            {
                predicateFiltered(entry);
            }
            return include;
        }
    }
}