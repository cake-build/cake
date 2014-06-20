using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.ILMerge
{
    public static class ILMergeExtensions
    {
        [CakeScriptMethod]
        public static void ILMerge(this ICakeContext context, FilePath outputFile, FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths)
        {
            var merger = new ILMergeRunner(context.Environment, context.Globber, new ProcessRunner());
            merger.Merge(outputFile, primaryAssembly, assemblyPaths);
        }

        [CakeScriptMethod]
        public static void ILMerge(this ICakeContext context, FilePath outputFile, FilePath primaryAssembly,
            IEnumerable<FilePath> assemblyPaths, ILMergeSettings settings)
        {
            var merger = new ILMergeRunner(context.Environment, context.Globber, new ProcessRunner());
            merger.Merge(outputFile, primaryAssembly, assemblyPaths, settings);
        }
    }
}
