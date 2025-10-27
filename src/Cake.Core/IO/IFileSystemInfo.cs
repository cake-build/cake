// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents an entry in the file system.
    /// </summary>
    public interface IFileSystemInfo
    {
        /// <summary>
        /// Gets the path to the entry.
        /// </summary>
        /// <value>The path.</value>
        Path Path { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFileSystemInfo"/> exists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry exists; otherwise, <c>false</c>.
        /// </value>
        bool Exists { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IFileSystemInfo"/> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry is hidden; otherwise, <c>false</c>.
        /// </value>
        bool Hidden { get; }

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the file was last written to.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the last write time in UTC, or <c>null</c> if not available.
        /// </value>
        DateTime? LastWriteTimeUtc => null;

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the file was created.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the creation time in UTC, or <c>null</c> if not available.
        /// </value>
        DateTime? CreationTimeUtc => null;

        /// <summary>
        /// Gets the date and time, in Coordinated Universal Time (UTC), that the file was last accessed.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> value that represents the last access time in UTC, or <c>null</c> if not available.
        /// </value>
        DateTime? LastAccessTimeUtc => null;

        /// <summary>
        /// Gets the Unix file mode of the entry.
        /// </summary>
        /// <value>
        /// A <see cref="System.IO.UnixFileMode"/> value that represents the Unix file mode of the entry.
        /// </value>
        System.IO.UnixFileMode? UnixFileMode => null;
    }
}