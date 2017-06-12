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

        public static void ContainsFilePath(IEnumerable<Path> paths, FilePath expected)
        {
            ContainsPath(paths, expected);
        }

        public static void ContainsDirectoryPath(IEnumerable<Path> paths, DirectoryPath expected)
        {
            ContainsPath(paths, expected);
        }

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