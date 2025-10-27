// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace Cake.Core.IO
{
    internal sealed class Directory : IDirectory
    {
        private readonly DirectoryInfo _directory;

        public DirectoryPath Path { get; }

        Path IFileSystemInfo.Path => Path;

        public bool Exists => _directory.Exists;

        public bool Hidden => (_directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;

        public DateTime LastWriteTime => _directory.LastWriteTime;

        public DateTime? LastWriteTimeUtc => _directory.LastWriteTimeUtc;
        public DateTime? CreationTimeUtc => _directory.CreationTimeUtc;
        public DateTime? LastAccessTimeUtc => _directory.LastAccessTimeUtc;
        public UnixFileMode? UnixFileMode => _directory.UnixFileMode;

        public Directory(DirectoryPath path)
        {
            Path = path;
            _directory = new DirectoryInfo(Path.FullPath);
        }

        public void Create()
        {
            _directory.Create();
        }

        public void Move(DirectoryPath destination)
        {
            ArgumentNullException.ThrowIfNull(destination);
            _directory.MoveTo(destination.FullPath);
        }

        public void Delete(bool recursive)
        {
            _directory.Delete(recursive);
        }

        public IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.EnumerateDirectories(filter, option)
                .Select(directory => new Directory(directory.FullName));
        }

        public IEnumerable<IFile> GetFiles(string filter, SearchScope scope)
        {
            var option = scope == SearchScope.Current ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories;
            return _directory.EnumerateFiles(filter, option)
                .Select(file => new File(file.FullName));
        }

        /// <inheritdoc/>
        public IDirectory SetCreationTime(DateTime creationTime)
        {
            System.IO.Directory.SetCreationTime(Path.FullPath, creationTime);
            _directory.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetCreationTimeUtc(DateTime creationTimeUtc)
        {
            System.IO.Directory.SetCreationTimeUtc(Path.FullPath, creationTimeUtc);
            _directory.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastAccessTime(DateTime lastAccessTime)
        {
            System.IO.Directory.SetLastAccessTime(Path.FullPath, lastAccessTime);
            _directory.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastAccessTimeUtc(DateTime lastAccessTimeUtc)
        {
            System.IO.Directory.SetLastAccessTimeUtc(Path.FullPath, lastAccessTimeUtc);
            _directory.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastWriteTime(DateTime lastWriteTime)
        {
            System.IO.Directory.SetLastWriteTime(Path.FullPath, lastWriteTime);
            _directory.Refresh();
            return this;
        }

        /// <inheritdoc/>
        public IDirectory SetLastWriteTimeUtc(DateTime lastWriteTimeUtc)
        {
            System.IO.Directory.SetLastWriteTimeUtc(Path.FullPath, lastWriteTimeUtc);
            _directory.Refresh();
            return this;
        }

        /// <inheritdoc/>
        [UnsupportedOSPlatform("windows")]
        public IDirectory SetUnixFileMode(UnixFileMode unixFileMode)
        {
            _directory.UnixFileMode = unixFileMode;
            _directory.Refresh();
            return this;
        }
    }
}