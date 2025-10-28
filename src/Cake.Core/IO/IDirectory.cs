// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a directory.
    /// </summary>
    public interface IDirectory : IFileSystemInfo
    {
        /// <summary>
        /// Gets the path to the directory.
        /// </summary>
        /// <value>The path.</value>
        new DirectoryPath Path { get; }

        /// <summary>
        /// Creates the directory.
        /// </summary>
        void Create();

        /// <summary>
        /// Moves the directory to the specified destination path.
        /// </summary>
        /// <param name="destination">The destination path.</param>
        void Move(DirectoryPath destination);

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        void Delete(bool recursive);

        /// <summary>
        /// Gets directories matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Directories matching the filter and scope.</returns>
        IEnumerable<IDirectory> GetDirectories(string filter, SearchScope scope);

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <returns>Files matching the specified filter and scope.</returns>
        IEnumerable<IFile> GetFiles(string filter, SearchScope scope);

        /// <summary>
        /// Sets the date and time that the file was created.
        /// </summary>
        /// <param name="creationTime">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IDirectory SetCreationTime(DateTime creationTime) => this;

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC), that the file was created.
        /// </summary>
        /// <param name="creationTimeUtc">A <see cref="DateTime"/> containing the value to set for the creation date and time of path. This value is expressed in UTC time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IDirectory SetCreationTimeUtc(DateTime creationTimeUtc) => this;

        /// <summary>
        /// Sets the date and time that the specified file or directory was last accessed.
        /// </summary>
        /// <param name="lastAccessTime">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IDirectory SetLastAccessTime(DateTime lastAccessTime) => this;

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC), that the specified file or directory was last accessed.
        /// </summary>
        /// <param name="lastAccessTimeUtc">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IDirectory SetLastAccessTimeUtc(DateTime lastAccessTimeUtc) => this;

        /// <summary>
        /// Sets the date and time that the specified file or directory was last written to.
        /// </summary>
        /// <param name="lastWriteTime">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IDirectory SetLastWriteTime(DateTime lastWriteTime) => this;

        /// <summary>
        /// Sets the date and time, in Coordinated Universal Time (UTC), that the specified file or directory was last written to.
        /// </summary>
        /// <param name="lastWriteTimeUtc">A <see cref="DateTime"/> containing the value to set for the last access date and time of path. This value is expressed in local time.</param>
        /// <returns>A <see cref="IFile"/> instance representing the specified path.</returns>
        IDirectory SetLastWriteTimeUtc(DateTime lastWriteTimeUtc) => this;

        /// <summary>
        /// Sets the Unix file mode for the directory.
        /// </summary>
        /// <param name="unixFileMode">The <see cref="UnixFileMode"/> to set.</param>
        /// <returns>The <see cref="IDirectory"/> instance representing the specified path.</returns>
        IDirectory SetUnixFileMode(UnixFileMode unixFileMode) => this;
    }
}