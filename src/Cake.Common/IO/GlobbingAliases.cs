using System;
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
    public static class GlobbingAliases
    {
        /// <summary>
        /// Gets all files matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <returns>A <see cref="FilePathCollection" />.</returns>
        /// <example>
        /// <code>
        /// var files = GetFiles("./**/Cake.*.dll");
        /// foreach(var file in files)
        /// {
        ///     Information("File: {0}", file);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Files")]
        public static FilePathCollection GetFiles(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new FilePathCollection(context.Globber.Match(pattern).OfType<FilePath>(),
                new PathComparer(context.Environment.IsUnix()));
        }

        /// <summary>
        /// Gets all directory matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <returns>A <see cref="DirectoryPathCollection" />.</returns>
        /// <example>
        /// <code>
        /// var directories = GetDirectories("./src/**/obj/*");
        /// foreach(var directory in directories)
        /// {
        ///     Information("Directory: {0}", directory);
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Directories")]
        public static DirectoryPathCollection GetDirectories(this ICakeContext context, string pattern)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            return new DirectoryPathCollection(context.Globber.Match(pattern).OfType<DirectoryPath>(),
                new PathComparer(context.Environment.IsUnix()));
        }
    }
}
