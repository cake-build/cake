using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cake.Core.IO
{
    public sealed class Directory : IDirectory
    {
        private readonly DirectoryInfo _directory;
        private readonly DirectoryPath _path;

        public DirectoryPath Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _directory.Exists; }
        }

        public Directory(DirectoryPath path)
        {
            _path = path;
            _directory = new DirectoryInfo(_path.FullPath);
        }

        public bool Create()
        {
            try
            {
                _directory.Create();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetDirectories(filter, option).Select(directory => new Directory(directory.FullName));
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetFiles(filter, option).Select(file => new File(file.FullName));
        }
    }
}