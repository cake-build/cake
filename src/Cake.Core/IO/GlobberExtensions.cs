// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO;

/// <summary>
/// Contains extensions for <see cref="IGlobber"/>.
/// </summary>
public static class GlobberExtensions
{
    /// <summary>
    /// Gets all files matching the specified pattern.
    /// </summary>
    /// <param name="globber">The globber.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The files matching the specified pattern.</returns>
    public static IEnumerable<FilePath> GetFiles(this IGlobber globber, GlobPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(globber);
        return globber.Match(pattern).OfType<FilePath>();
    }

    /// <summary>
    /// Gets all directories matching the specified pattern.
    /// </summary>
    /// <param name="globber">The globber.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The directories matching the specified pattern.</returns>
    public static IEnumerable<DirectoryPath> GetDirectories(this IGlobber globber, GlobPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(globber);
        return globber.Match(pattern).OfType<DirectoryPath>();
    }

    /// <summary>
    /// Gets all files and directories matching the specified pattern.
    /// Scripts and Frosting should use the <c>GetFileSystemInfos(pattern)</c> alias in Cake.Common instead,
    /// which supplies the file system from the context.
    /// </summary>
    /// <example>
    /// <code>
    /// var entries = context.Globber.GetFileSystemInfos(context.FileSystem, "./artifacts/*");
    /// foreach (var entry in entries)
    /// {
    ///     Information("{0}: {1}", entry is IDirectory ? "Directory" : "File", entry.Path);
    /// }
    /// </code>
    /// </example>
    /// <param name="globber">The globber.</param>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The file system entries matching the specified pattern.</returns>
    public static IEnumerable<IFileSystemInfo> GetFileSystemInfos(this IGlobber globber, IFileSystem fileSystem, GlobPattern pattern)
    {
        return GetFileSystemInfos(globber, fileSystem, pattern, new GlobberSettings());
    }

    /// <summary>
    /// Gets all files and directories matching the specified pattern.
    /// Scripts and Frosting should use the <c>GetFileSystemInfos(pattern)</c> alias in Cake.Common instead,
    /// which supplies the file system from the context.
    /// </summary>
    /// <example>
    /// <code>
    /// Func&lt;IFileSystemInfo, bool&gt; exclude_node_modules =
    ///     fileSystemInfo => !fileSystemInfo.Path.FullPath.EndsWith(
    ///         "node_modules", StringComparison.OrdinalIgnoreCase);
    ///
    /// var entries = context.Globber.GetFileSystemInfos(
    ///     context.FileSystem,
    ///     "./src/**/*",
    ///     new GlobberSettings { Predicate = exclude_node_modules });
    /// foreach (var entry in entries)
    /// {
    ///     Information("{0}: {1}", entry is IDirectory ? "Directory" : "File", entry.Path);
    /// }
    /// </code>
    /// </example>
    /// <param name="globber">The globber.</param>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="settings">The globber settings.</param>
    /// <returns>The file system entries matching the specified pattern.</returns>
    public static IEnumerable<IFileSystemInfo> GetFileSystemInfos(this IGlobber globber, IFileSystem fileSystem, GlobPattern pattern, GlobberSettings settings)
    {
        ArgumentNullException.ThrowIfNull(globber);
        ArgumentNullException.ThrowIfNull(fileSystem);
        return globber.Match(pattern, settings).Select(path =>
        {
            if (path is DirectoryPath directory)
            {
                return (IFileSystemInfo)fileSystem.GetDirectory(directory);
            }

            return fileSystem.GetFile((FilePath)path);
        });
    }

    /// <summary>
    /// Returns <see cref="Path" /> instances matching the specified pattern.
    /// </summary>
    /// <param name="globber">The globber.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <returns>
    ///   <see cref="Path" /> instances matching the specified pattern.
    /// </returns>
    public static IEnumerable<Path> Match(this IGlobber globber, GlobPattern pattern)
    {
        ArgumentNullException.ThrowIfNull(globber);
        return globber.Match(pattern, new GlobberSettings());
    }

    /// <summary>
    /// Gets all files matching the specified pattern.
    /// </summary>
    /// <param name="globber">The globber.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The files matching the specified pattern.</returns>
    public static IEnumerable<FilePath> GetFiles(this IGlobber globber, string pattern)
    {
        ArgumentNullException.ThrowIfNull(globber);
        return globber.Match(pattern).OfType<FilePath>();
    }

    /// <summary>
    /// Gets all directories matching the specified pattern.
    /// </summary>
    /// <param name="globber">The globber.</param>
    /// <param name="pattern">The pattern.</param>
    /// <returns>The directories matching the specified pattern.</returns>
    public static IEnumerable<DirectoryPath> GetDirectories(this IGlobber globber, string pattern)
    {
        ArgumentNullException.ThrowIfNull(globber);
        return globber.Match(pattern).OfType<DirectoryPath>();
    }

    /// <summary>
    /// Returns <see cref="Path" /> instances matching the specified pattern.
    /// </summary>
    /// <param name="globber">The globber.</param>
    /// <param name="pattern">The pattern to match.</param>
    /// <returns>
    ///   <see cref="Path" /> instances matching the specified pattern.
    /// </returns>
    public static IEnumerable<Path> Match(this IGlobber globber, string pattern)
    {
        ArgumentNullException.ThrowIfNull(globber);
        return globber.Match(pattern, new GlobberSettings());
    }
}
