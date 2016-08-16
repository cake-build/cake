﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains functionality related to file system globbing.
    /// </summary>
    [CakeAliasCategory("Globbing")]
    public static class GlobbingAliases
    {
        /// <summary>
        /// Gets all files matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// var files = GetFiles("./**/Cake.*.dll");
        /// foreach(var file in files)
        /// {
        ///     Information("File: {0}", file);
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <returns>A <see cref="FilePathCollection" />.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Files")]
        public static FilePathCollection GetFiles(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new FilePathCollection(context.Globber.Match(pattern).OfType<FilePath>(),
                new PathComparer(context.Environment.Platform.IsUnix()));
        }

        /// <summary>
        /// Gets all files matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// Func&lt;IFileSystemInfo, bool&gt; exclude_node_modules =
        ///     fileSystemInfo => !fileSystemInfo.Path.FullPath.EndsWith(
        ///         "node_modules", StringComparison.OrdinalIgnoreCase);
        ///
        /// var files = GetFiles("./**/Cake.*.dll", exclude_node_modules);
        /// foreach(var file in files)
        /// {
        ///     Information("File: {0}", file);
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <param name="predicate">The predicate used to filter directories based on file system information.</param>
        /// <returns>A <see cref="FilePathCollection" />.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Files")]
        public static FilePathCollection GetFiles(this ICakeContext context, string pattern, Func<IDirectory, bool> predicate)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new FilePathCollection(context.Globber.Match(pattern, predicate).OfType<FilePath>(),
                new PathComparer(context.Environment.Platform.IsUnix()));
        }

        /// <summary>
        /// Gets all directory matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// var directories = GetDirectories("./src/**/obj/*");
        /// foreach(var directory in directories)
        /// {
        ///     Information("Directory: {0}", directory);
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <returns>A <see cref="DirectoryPathCollection" />.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Directories")]
        public static DirectoryPathCollection GetDirectories(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new DirectoryPathCollection(context.Globber.Match(pattern).OfType<DirectoryPath>(),
                new PathComparer(context.Environment.Platform.IsUnix()));
        }

        /// <summary>
        /// Gets all directory matching the specified pattern.
        /// </summary>
        /// <example>
        /// <code>
        /// Func&lt;IFileSystemInfo, bool&gt; exclude_node_modules =
        ///     fileSystemInfo => !fileSystemInfo.Path.FullPath.EndsWith(
        ///         "node_modules", StringComparison.OrdinalIgnoreCase);
        ///
        /// var directories = GetDirectories("./src/**/obj/*", exclude_node_modules);
        /// foreach(var directory in directories)
        /// {
        ///     Information("Directory: {0}", directory);
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <param name="predicate">The predicate used to filter directories based on file system information.</param>
        /// <returns>A <see cref="DirectoryPathCollection" />.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Directories")]
        public static DirectoryPathCollection GetDirectories(this ICakeContext context, string pattern, Func<IDirectory, bool> predicate)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            return new DirectoryPathCollection(context.Globber.Match(pattern, predicate).OfType<DirectoryPath>(),
                new PathComparer(context.Environment.Platform.IsUnix()));
        }
    }
}