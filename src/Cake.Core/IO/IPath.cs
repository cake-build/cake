// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a path.
    /// </summary>
    /// <typeparam name="T">The path type.</typeparam>
    public interface IPath<T>
    {
        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        string FullPath { get; }

        /// <summary>
        /// Gets a value indicating whether or not this path is relative.
        /// </summary>
        /// <value>
        /// <c>true</c> if this path is relative; otherwise, <c>false</c>.
        /// </value>
        bool IsRelative { get; }

        /// <summary>
        /// Gets a value indicating whether or not this path is an UNC path.
        /// </summary>
        /// <value>
        /// <c>true</c> if this path is an UNC path; otherwise, <c>false</c>.
        /// </value>
        bool IsUNC { get; }

        /// <summary>
        /// Gets the separator this path was normalized with.
        /// </summary>
        char Separator { get; }

        /// <summary>
        /// Gets the segments making up the path.
        /// </summary>
        /// <value>The segments making up the path.</value>
        string[] Segments { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this path.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        string ToString();

        /// <summary>
        /// Makes the {T} path absolute (if relative) using the current working directory.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>An absolute path.</returns>
        T MakeAbsolute(ICakeEnvironment environment);

        /// <summary>
        /// Makes the path absolute (if relative) using the specified directory path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>An absolute path.</returns>
        T MakeAbsolute(DirectoryPath path);

        /// <summary>
        /// Collapses a path containing ellipses.
        /// </summary>
        /// <returns>A collapsed path.</returns>
        T Collapse();

        /// <summary>
        /// Get the relative path to another path.
        /// </summary>
        /// <param name="to">The target path.</param>
        /// <returns>A path.</returns>
        public T GetRelativePath(T to);
    }
}