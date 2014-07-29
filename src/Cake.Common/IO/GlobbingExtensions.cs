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
    public static class GlobbingExtensions
    {
        /// <summary>
        /// Gets all files matching the specified pattern.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pattern"></param>
        /// <returns>A <see cref="FilePathCollection"/>.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Globbing")]
        public static FilePathCollection GetFiles(this ICakeContext context, string pattern)
        {
            return new FilePathCollection(context.Globber.Match(pattern).OfType<FilePath>(),
                new PathComparer(context.Environment.IsUnix()));
        }

        /// <summary>
        /// Gets all directory matching the specified pattern.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pattern"></param>
        /// <returns>A <see cref="DirectoryPathCollection"/>.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Directories")]
        public static DirectoryPathCollection GetDirectories(this ICakeContext context, string pattern)
        {
            return new DirectoryPathCollection(context.Globber.Match(pattern).OfType<DirectoryPath>(),
                new PathComparer(context.Environment.IsUnix()));
        }
    }
}
