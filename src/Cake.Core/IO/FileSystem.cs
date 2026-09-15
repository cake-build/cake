// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// A physical file system implementation.
    /// </summary>
    public sealed class FileSystem : IFileSystem
    {
        /// <inheritdoc/>
        public IFile GetFile(FilePath path)
        {
            return new File(path);
        }

        /// <inheritdoc/>
        public IDirectory GetDirectory(DirectoryPath path)
        {
            return new Directory(path);
        }

        /// <inheritdoc/>
        public IFileSystemInfo GetFileSystemInfo(DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            var directory = GetDirectory(path);
            if (directory.Exists)
            {
                return directory;
            }
            return GetFile(path.FullPath);
        }
    }
}