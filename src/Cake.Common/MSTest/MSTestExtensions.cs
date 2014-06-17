using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.MSTest
{
    public static class MSTestExtensions
    {
        public static void MSTest(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            MSTest(context, assemblies);
        }

        public static void MSTest(this ICakeContext context, string pattern, MSTestSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            MSTest(context, assemblies, settings);
        }

        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths)
        {
            MSTest(context, assemblyPaths, new MSTestSettings());            
        }

        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths, MSTestSettings settings)
        {
            var runner = new MSTestRunner(context.FileSystem, context.Environment);
            foreach (var assembly in assemblyPaths)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}