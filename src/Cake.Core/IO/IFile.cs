// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a file.
    /// </summary>
    public interface IFile : IFileSystemInfo
    {
        /// <summary>
        /// Gets the path to the file.
        /// </summary>
        /// <value>The path.</value>
        new FilePath Path { get; }

        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file.</value>
        long Length { get; }

        /// <summary>
        /// Gets or sets the file attributes.
        /// </summary>
        /// <value>The file attributes.</value>
        FileAttributes Attributes { get; set; }

        /// <summary>
        /// Copies the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        /// <param name="overwrite">Will overwrite existing destination file if set to <c>true</c>.</param>
        void Copy(FilePath destination, bool overwrite);

        /// <summary>
        /// Moves the file to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        void Move(FilePath destination);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        void Delete();

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="fileMode">The file mode.</param>
        /// <param name="fileAccess">The file access.</param>
        /// <param name="fileShare">The file share.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        Stream Open(FileMode fileMode, FileAccess fileAccess, FileShare fileShare);

        /// <summary>
        /// Sets the date and time that the file was created.
        /// </summary>
        /// <param name="creationTime">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetCreationTime(DateTime creationTime) => this;

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC), that the file was created.
        /// </summary>
        /// <param name="creationTimeUtc">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetCreationTimeUtc(DateTime creationTimeUtc) => this;

        /// <summary>
        /// Sets the date and time that the specified file or directory was last accessed.
        /// </summary>
        /// <param name="lastAccessTime">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetLastAccessTime(DateTime lastAccessTime) => this;

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC), that the specified file or directory was last accessed.
        /// </summary>
        /// <param name="lastAccessTimeUtc">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetLastAccessTimeUtc(DateTime lastAccessTimeUtc) => this;

        /// <summary>
        /// Sets the date and time that the specified file or directory was last written to.
        /// </summary>
        /// <param name="lastWriteTime">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetLastWriteTime(DateTime lastWriteTime) => this;

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC), that the specified file or directory was last written to.
        /// </summary>
        /// <param name="lastWriteTimeUtc">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetLastWriteTimeUtc(DateTime lastWriteTimeUtc) => this;

        /// <summary>
        /// Sets the Unix file mode for the file.
        /// </summary>
        /// <param name="unixFileMode">The <see cref="UnixFileMode"/> to set.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IFile SetUnixFileMode(UnixFileMode unixFileMode) => this;
    }
}
