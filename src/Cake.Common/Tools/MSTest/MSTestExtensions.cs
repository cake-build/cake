using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSTest
{
    public static class MSTestExtensions
    {
        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            MSTest(context, assemblies);
        }

        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, string pattern, MSTestSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            MSTest(context, assemblies, settings);
        }

        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths)
        {
            MSTest(context, assemblyPaths, new MSTestSettings());            
        }

        [CakeMethodAlias]
        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths, MSTestSettings settings)
        {
            var runner = new MSTestRunner(context.FileSystem, context.Environment, context.ProcessRunner);
            foreach (var assembly in assemblyPaths)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}