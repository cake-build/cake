// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    public partial class AssertEx
    {
        private static readonly PathComparer _comparer = new PathComparer(false);

        /// <summary>
        /// Verifies that the collection contains the specified file path.
        /// </summary>
        /// <param name="paths">The collection of paths to search.</param>
        /// <param name="expected">The expected file path.</param>
        public static void ContainsFilePath(IEnumerable<Path> paths, FilePath expected)
        {
            ContainsPath(paths, expected);
        }

        /// <summary>
        /// Verifies that the collection contains the specified directory path.
        /// </summary>
        /// <param name="paths">The collection of paths to search.</param>
        /// <param name="expected">The expected directory path.</param>
        public static void ContainsDirectoryPath(IEnumerable<Path> paths, DirectoryPath expected)
        {
            ContainsPath(paths, expected);
        }

        /// <summary>
        /// Verifies that the collection contains the specified path.
        /// </summary>
        /// <typeparam name="T">The type of path to search for.</typeparam>
        /// <param name="paths">The collection of paths to search.</param>
        /// <param name="expected">The expected path.</param>
        public static void ContainsPath<T>(IEnumerable<Path> paths, T expected)
            where T : Path
        {
            // Find the path.
            var path = paths.FirstOrDefault(x => _comparer.Equals(x, expected));

            // Assert
            Assert.NotNull(path);
            Assert.IsType<T>(path);
        }
    }
}