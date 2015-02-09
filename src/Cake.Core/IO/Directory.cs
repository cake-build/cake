using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cake.Core.IO
{
    internal sealed class Directory : IDirectory
    {
        private readonly DirectoryInfo _directory;
        private readonly DirectoryPath _path;

        public DirectoryPath Path
        {
            get { return _path; }
        }

        Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _directory.Exists; }
        }

        public bool Hidden
        {
            get { return (_directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
        }

        public Directory(DirectoryPath path)
        {
            _path = path;
            _directory = new DirectoryInfo(_path.FullPath);
        }

        public void Create()
        {
            _directory.Create();
        }

        public void Delete(bool recursive)
        {
            _directory.Delete(recursive);
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            return GetDirectories(filter, scope, null);
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate)
        {
            return GetDirectories(filter, scope, wherePredicate, null);
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory
                .GetDirectories(filter, option)
                .Select(directory => new Directory(directory.FullName))
                .Where(entry => WherePredicate(entry, wherePredicate, predicateFiltered));
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            return GetFiles(filter, scope, null);
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate)
        {
            return GetFiles(filter, scope, wherePredicate, null);
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory
                .GetFiles(filter, option)
                .Select(file => new File(file.FullName))
                .Where(entry => WherePredicate(entry, wherePredicate, predicateFiltered));
        }

        private static bool WherePredicate(IFileSystemInfo entry, Func<IFileSystemInfo, bool> wherePredicate, Action<IFileSystemInfo> predicateFiltered)
        {
            var include = wherePredicate == null || wherePredicate(entry);
            if (!include && predicateFiltered != null)
            {
                predicateFiltered(entry);
            }
            return include;
        }
    }
}