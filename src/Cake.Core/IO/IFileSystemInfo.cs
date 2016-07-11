// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.IO
{
    /// <summary>
    /// Represents an entry in the file system
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
    }
}
