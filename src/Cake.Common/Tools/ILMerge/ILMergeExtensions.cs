using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.ILMerge
{
    /// <summary>
    /// Contains functionality related to ILMerge.
    /// </summary>
    public static class ILMergeExtensions
    {
        /// <summary>
        /// Merges the specified assemblies.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="primaryAssembly">The primary assembly.</param>
        /// <param name="assemblyPaths">The assembly paths.</param>
        [CakeMethodAlias]
        public static void ILMerge(this ICakeContext context, FilePath outputFile, FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths)
        {
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
        [CakeMethodAlias]
        public static void ILMerge(this ICakeContext context, FilePath outputFile, FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings)
        {
            var merger = new ILMergeRunner(context.FileSystem, context.Environment, context.Globber, context.ProcessRunner);
            merger.Merge(outputFile, primaryAssembly, assemblyPaths, settings);
        }
    }
}
