// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.Versioning;

namespace Cake.Core.IO
{
    internal sealed class File : IFile
    {
        private readonly FileInfo _file;

        public FilePath Path { get; }

        Path IFileSystemInfo.Path => Path;

        public bool Exists => _file.Exists;

        public bool Hidden => (_file.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        public DateTime LastWriteTime => _file.LastWriteTime;

        public DateTime? LastWriteTimeUtc => _file.LastWriteTimeUtc;
        public DateTime? CreationTimeUtc => _file.CreationTimeUtc;
        public DateTime? LastAccessTimeUtc => _file.LastAccessTimeUtc;

        public long Length => _file.Length;

        public FileAttributes Attributes
        {
            get { return _file.Attributes; }
            set { _file.Attributes = value; }
        }

        public UnixFileMode? UnixFileMode => _file.UnixFileMode;

        public File(FilePath path)
        {
            Path = path;
            _file = new FileInfo(path.FullPath);
        }

        public void Copy(FilePath destination, bool overwrite)
        {
            ArgumentNullException.ThrowIfNull(destination);
            _file.CopyTo(destination.FullPath, overwrite);
        }

        public void Move(FilePath destination)
        {
            ArgumentNullException.ThrowIfNull(destination);
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

        /// <inheritdoc/>
        public IFile SetCreationTime(DateTime creationTime)
        {
            System.IO.File.SetCreationTime(Path.FullPath, creationTime);
            _file.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetCreationTimeUtc(DateTime creationTimeUtc)
        {
            System.IO.File.SetCreationTimeUtc(Path.FullPath, creationTimeUtc);
            _file.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastAccessTime(DateTime lastAccessTime)
        {
            System.IO.File.SetLastAccessTime(Path.FullPath, lastAccessTime);
            _file.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
        {
            System.IO.File.SetLastAccessTimeUtc(Path.FullPath, lastAccessTimeUtc);
            _file.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastWriteTime(DateTime lastWriteTime)
        {
            System.IO.File.SetLastWriteTime(Path.FullPath, lastWriteTime);
            _file.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IFile SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
        {
            System.IO.File.SetLastWriteTimeUtc(Path.FullPath, lastWriteTimeUtc);
            _file.Refresh();
            return this;
        }

        /// <inheritdoc/>
        [UnsupportedOSPlatform("windows")]
        public IFile SetUnixFileMode(UnixFileMode unixFileMode)
        {
            _file.UnixFileMode = unixFileMode;
            _file.Refresh();
            return this;
        }
    }
}