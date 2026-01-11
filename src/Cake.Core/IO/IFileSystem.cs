// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Gets a <see cref="IFile"/> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./build.cake");
        /// Information("Exists: {0}", file.Exists);
        /// </code>
        /// </example>
        IFile GetFile(FilePath path);

        /// <summary>
        /// Gets a <see cref="IDirectory"/> instance representing the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>A <see cref="IDirectory"/> instance representing the specified path.</returns>
        /// <example>
        /// <code>
        /// var dir = context.FileSystem.GetDirectory("./artifacts");
        /// Information("Exists: {0}", dir.Exists);
        /// </code>
        /// </example>
        IDirectory GetDirectory(DirectoryPath path);

        /// <summary>
        /// Gets a <see cref="IFileSystemInfo"/> for the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="IDirectory"/> when the path exists as a directory; otherwise an <see cref="IFile"/>,
        /// including when the path does not exist.
        /// </returns>
        /// <example>
        /// <code>
        /// var info = context.FileSystem.GetFileSystemInfo(context.Environment.WorkingDirectory);
        /// Information("{0} is a directory: {1}", info.Path, info is IDirectory);
        /// </code>
        /// </example>
        IFileSystemInfo GetFileSystemInfo(Path path)
        {
            ArgumentNullException.ThrowIfNull(path);

            return GetFileSystemInfo(path.FullPath);
        }

        /// <summary>
        /// Gets a <see cref="IFileSystemInfo"/> for the specified path, when it is not known
        /// whether the path refers to a file or a directory.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// A <see cref="IDirectory"/> when the path exists as a directory; otherwise an <see cref="IFile"/>,
        /// including when the path does not exist.
        /// </returns>
        /// <example>
        /// <code>
        /// var info = context.FileSystem.GetFileSystemInfo("./artifacts");
        /// Information("{0} exists: {1}, is a directory: {2}", info.Path, info.Exists, info is IDirectory);
        /// </code>
        /// </example>
        IFileSystemInfo GetFileSystemInfo(string path)
        {
            ArgumentNullException.ThrowIfNull(path);

            var directory = GetDirectory(new DirectoryPath(path));
            return directory.Exists
                ? directory
                : GetFile(new FilePath(path));
        }
    }
}
