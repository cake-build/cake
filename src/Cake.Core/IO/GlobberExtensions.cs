// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
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
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }
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
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }
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
        public static IEnumerable<Path> Match(this IGlobber globber, GlobPattern pattern)
        {
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }
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
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }
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
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }
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
            if (globber == null)
            {
                throw new ArgumentNullException(nameof(globber));
            }
            return globber.Match(pattern, new GlobberSettings());
        }
    }
}