using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// <para>Contains functionality related to ILMerge.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and install from NuGet.org, or specify the ToolPath within the ILMergeSettings class:
    /// <code>
    /// #tool "nuget:?package=ILRepack"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ILMerge")]
    public static class ILMergeAliases
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
        /// ILMerge("./MergedCake.exe", "./Cake.exe", assemblyPaths);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ILMerge(this ICakeContext context, FilePath outputFile, FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var merger = new ILMergeRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
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
        /// ILMerge(
        ///     "./MergedCake.exe",
        ///     "./Cake.exe",
        ///     assemblyPaths,
        ///     new ILMergeSettings { Internalize = true });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ILMerge(this ICakeContext context, FilePath outputFile, FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var merger = new ILMergeRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
            merger.Merge(outputFile, primaryAssembly, assemblyPaths, settings);
        }
    }
}