using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.IO
{
    public static class GlobberExtensions
    {
        public static IEnumerable<FilePath> GetFiles(this IGlobber globber, string pattern)
        {
            return globber.Match(pattern).OfType<FilePath>();
        }

        public static IEnumerable<DirectoryPath> GetDirectories(this IGlobber globber, string pattern)
        {
            return globber.Match(pattern).OfType<DirectoryPath>();
        }
    }
}
