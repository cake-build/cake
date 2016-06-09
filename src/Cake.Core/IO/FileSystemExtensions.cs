// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Contains extensions for <see cref="IFileSystem"/>.
    /// </summary>
    public static class FileSystemExtensions
    {
        /// <summary>
        /// Determines if a specified <see cref="FilePath"/> exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>Whether or not the specified file exist.</returns>
        public static bool Exist(this IFileSystem fileSystem, FilePath path)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            var file = fileSystem.GetFile(path);
            return file != null && file.Exists;
        }

        /// <summary>
        /// Determines if a specified <see cref="DirectoryPath"/> exist.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="path">The path.</param>
        /// <returns>Whether or not the specified directory exist.</returns>
        public static bool Exist(this IFileSystem fileSystem, DirectoryPath path)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            var directory = fileSystem.GetDirectory(path);
            return directory != null && directory.Exists;
        }
    }
}
