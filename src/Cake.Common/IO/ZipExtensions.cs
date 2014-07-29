using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains functionality related to compress files to Zip.
    /// </summary>
    [CakeAliasCategory("Compression")]
    public static class ZipExtensions
    {
        /// <summary>
        /// Zips the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        [CakeMethodAlias]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath)
        {
            var filePaths = context.GetFiles(string.Concat(rootPath, "/**/*"));
            Zip(context, rootPath, outputPath, filePaths);
        }

        /// <summary>
        /// Zips the files matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="pattern">The pattern.</param>
        [CakeMethodAlias]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
        {
            var filePaths = context.GetFiles(pattern);
            Zip(context, rootPath, outputPath, filePaths);
        }

        /// <summary>
        /// Zips the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="filePaths">The file paths.</param>
        [CakeMethodAlias]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
        {
            var zipper = new Zipper(context.FileSystem, context.Environment, context.Log);
            zipper.Zip(rootPath, outputPath, filePaths);
        }
    }
}
