using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    public static class GlobbingExtensions
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
