using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DirectoryInfo = Pri.LongPath.DirectoryInfo;

namespace Cake.Core.IO.LongPath
{
    internal sealed class Directory : IDirectory
    {
        private readonly DirectoryInfo _directory;
        private readonly DirectoryPath _path;

        Path IFileSystemInfo.Path
        {
            get { return Path; }
        }
        public DirectoryPath Path { get { return _path; } }
        public bool Exists { get { return _directory.Exists; } }
        
        public bool Hidden
        {
            get { return (_directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
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
            var option = GetSearchOptionFromScope(scope);
            
            return _directory
                .EnumerateDirectories(filter,option)
                .Select(directoryInfo=>new Directory(directoryInfo.FullName));
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var option = GetSearchOptionFromScope(scope);
            
            return _directory
                .EnumerateFiles(filter,option)
                .Select(directoryInfo=>new File(directoryInfo.FullName));
        }

        internal Directory(DirectoryPath path)
        {
            _path = path;
            _directory = new DirectoryInfo(path.FullPath);
        }

        private static SearchOption GetSearchOptionFromScope(SearchScope scope)
        {
            SearchOption option;
            switch (scope)
            {
                case SearchScope.Current:
                    option = SearchOption.TopDirectoryOnly;
                    break;
                case SearchScope.Recursive:
                    option = SearchOption.AllDirectories;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scope", scope, "Unable to map supplied scope");
            }
            return option;
        }
    }
}