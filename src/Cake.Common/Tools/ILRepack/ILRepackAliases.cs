// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.ILRepack
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/gluck/il-repack">ILRepack</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="ILRepackSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=ILRepack"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ILRepack")]
    public static class ILRepackAliases
    {
        /// <summary>
        /// Merges the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="primaryAssembly">The primary assembly.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <example>
        /// <code>
        /// var assemblyPaths = GetFiles("./**/Cake.*.dll");
        /// ILRepack("./MergedCake.exe", "./Cake.exe", assemblyPaths);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ILRepack(
            this ICakeContext context,
            FilePath outputFile,
            FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var merger = new ILRepackRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            merger.Merge(outputFile, primaryAssembly, assemblyPaths);
        }

        /// <summary>
        /// Merges the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="primaryAssembly">The primary assembly.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var assemblyPaths = GetFiles("./**/Cake.*.dll");
        /// ILRepack(
        ///     "./MergedCake.exe",
        ///     "./Cake.exe",
        ///     assemblyPaths,
        ///     new ILRepackSettings { Internalize = true });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ILRepack(
            this ICakeContext context,
            FilePath outputFile,
            FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths,
            ILRepackSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var merger = new ILRepackRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            merger.Merge(outputFile, primaryAssembly, assemblyPaths, settings);
        }
    }
}
