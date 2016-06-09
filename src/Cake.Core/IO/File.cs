// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.IO;

namespace Cake.Core.IO
{
    internal sealed class File : IFile
    {
        private readonly FileInfo _file;
        private readonly FilePath _path;

        public FilePath Path
        {
            get { return _path; }
        }

        Path IFileSystemInfo.Path
        {
            get { return _path; }
        }

        public bool Exists
        {
            get { return _file.Exists; }
        }

        public bool Hidden
        {
            get { return (_file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden; }
        }

        public long Length
        {
            get { return _file.Length; }
        }

        public File(FilePath path)
        {
            _path = path;
            _file = new FileInfo(path.FullPath);
        }

        public void Copy(FilePath destination, bool overwrite)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            _file.CopyTo(destination.FullPath, overwrite);
        }

        public void Move(FilePath destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }
            _file.MoveTo(destination.FullPath);
        }

        public void Delete()
        {
            _file.Delete();
        }

        public Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
        {
            return _file.Open(fileMode, fileAccess, fileShare);
        }
    }
}
