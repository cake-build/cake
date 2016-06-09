// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains functionality related to compress files to Zip.
    /// </summary>
    [CakeAliasCategory("Compression")]
    public static class ZipAliases
    {
        /// <summary>
        /// Zips the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <example>
        /// <code>
        /// Zip("./publish", "publish.zip");
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// Zip("./", "xmlfiles.zip", "./*.xml");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
        {
            var filePaths = context.GetFiles(pattern);
            if (filePaths.Count == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }
            Zip(context, rootPath, outputPath, filePaths);
        }

        /// <summary>
        /// Zips the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <example>
        /// <code>
        /// var files = GetFiles("./**/Cake.*.dll");
        /// Zip("./", "cakeassemblies.zip", files);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var zipper = new Zipper(context.FileSystem, context.Environment, context.Log);
            zipper.Zip(rootPath, outputPath, filePaths);
        }

        /// <summary>
        /// Zips the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <example>
        /// <code>
        /// var files = new [] {
        ///     "./src/Cake/bin/Debug/Autofac.dll",
        ///     "./src/Cake/bin/Debug/Cake.Common.dll",
        ///     "./src/Cake/bin/Debug/Cake.Core.dll",
        ///     "./src/Cake/bin/Debug/Cake.exe"
        /// };
        /// Zip("./", "cakebinaries.zip", files);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Zip(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var paths = filePaths.Select(p => new FilePath(p));
            var zipper = new Zipper(context.FileSystem, context.Environment, context.Log);
            zipper.Zip(rootPath, outputPath, paths);
        }

        /// <summary>
        /// Unzips the specified file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="zipFile">Zip file to unzip.</param>
        /// <param name="outputPath">Output path to unzip into.</param>
        /// <example>
        /// <code>
        /// Unzip("Cake.zip", "./cake");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void Unzip(this ICakeContext context, FilePath zipFile, DirectoryPath outputPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var zipper = new Zipper(context.FileSystem, context.Environment, context.Log);
            zipper.Unzip(zipFile, outputPath);
        }
    }
}
