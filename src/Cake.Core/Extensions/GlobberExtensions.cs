using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.Core.Extensions
{
    public static class GlobberExtensions
    {
        public static IEnumerable<FilePath> GetFiles(this ICakeContext context, string pattern)
        {
            return context.Globber.Match(pattern).OfType<FilePath>();
        }

        public static IEnumerable<DirectoryPath> GetDirectories(this ICakeContext context, string pattern)
        {
            return context.Globber.Match(pattern).OfType<DirectoryPath>();
        }
    }
}
