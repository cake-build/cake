using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    public static class GlobbingExtensions
    {
        [CakeMethodAlias]
        public static FilePathCollection GetFiles(this ICakeContext context, string pattern)
        {
            return new FilePathCollection(context.Globber.Match(pattern).OfType<FilePath>(),
                new PathComparer(context.Environment.IsUnix()));
        }

        [CakeMethodAlias]
        public static DirectoryPathCollection GetDirectories(this ICakeContext context, string pattern)
        {
            return new DirectoryPathCollection(context.Globber.Match(pattern).OfType<DirectoryPath>(),
                new PathComparer(context.Environment.IsUnix()));
        }
    }
}
