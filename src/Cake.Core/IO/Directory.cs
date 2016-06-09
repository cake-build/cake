// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetDirectories(filter, option)
                .Select(directory => new Directory(directory.FullName));
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.GetFiles(filter, option)
                .Select(file => new File(file.FullName));
        }
    }
}
