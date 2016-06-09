// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extensions for <see cref="IDirectory"/>.
    /// </summary>
    public static class DirectoryExtensions
    {
        /// <summary>
        /// Gets directories matching the specified filter and scope, with option to exclude hidden directories.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <param name="predicate">Predicate used to filter directories based on file system information.</param>
        /// <returns>The directories matching the specified filter, scope and predicate.</returns>
        public static IEnumerable<IDirectory> GetDirectories(this IDirectory directory, string filter, SearchScope scope, Func<IFileSystemInfo, bool> predicate)
        {
            return GetDirectories(directory, filter, scope, predicate, null);
        }

        /// <summary>
        /// Gets directories matching the specified filter and scope, with option to exclude hidden directories.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <param name="predicate">Predicate used to filter directories based on file system information.</param>
        /// <param name="action">Callback if directory gets filtered by <paramref name="predicate"/>.</param>
        /// <returns>The directories matching the specified filter, scope and predicate.</returns>
        public static IEnumerable<IDirectory> GetDirectories(this IDirectory directory, string filter, SearchScope scope,
            Func<IFileSystemInfo, bool> predicate, Action<IFileSystemInfo> action)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }
            return directory.GetDirectories(filter, scope)
                .Where(entry => Filter(entry, predicate, action));
        }

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <param name="predicate">Predicate used to filter files based on file system information.</param>
        /// <returns>The files matching the specified filter, scope and predicate.</returns>
        public static IEnumerable<IFile> GetFiles(this IDirectory directory, string filter, SearchScope scope, Func<IFileSystemInfo, bool> predicate)
        {
            return GetFiles(directory, filter, scope, predicate, null);
        }

        /// <summary>
        /// Gets files matching the specified filter and scope.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="scope">The search scope.</param>
        /// <param name="predicate">Predicate used to filter files based on file system information.</param>
        /// <param name="action">Callback if file gets filtered by <paramref name="predicate"/>.</param>
        /// <returns>The files matching the specified filter, scope and predicate.</returns>
        public static IEnumerable<IFile> GetFiles(this IDirectory directory, string filter, SearchScope scope,
            Func<IFileSystemInfo, bool> predicate, Action<IFileSystemInfo> action)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory");
            }
            return directory.GetFiles(filter, scope)
                .Where(entry => Filter(entry, predicate, action));
        }

        private static bool Filter(IFileSystemInfo entry, Func<IFileSystemInfo, bool> wherePredicate,
            Action<IFileSystemInfo> predicateFiltered)
        {
            var include = wherePredicate == null || wherePredicate(entry);
            if (!include && predicateFiltered != null)
            {
                predicateFiltered(entry);
            }
            return include;
        }
    }
}
