using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Xunit
{
    public partial class Assert
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
            NotNull(path);
            IsType<T>(path);
        }
    }
}