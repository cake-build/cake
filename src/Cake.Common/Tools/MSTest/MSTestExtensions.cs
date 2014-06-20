using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSTest
{
    public static class MSTestExtensions
    {
        [CakeScriptMethod]
        public static void MSTest(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            MSTest(context, assemblies);
        }

        [CakeScriptMethod]
        public static void MSTest(this ICakeContext context, string pattern, MSTestSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            MSTest(context, assemblies, settings);
        }

        [CakeScriptMethod]
        public static void MSTest(this ICakeContext context, IEnumerable<FilePath> assemblyPaths)
        {
            MSTest(context, assemblyPaths, new MSTestSettings());            
        }

        [CakeScriptMethod]
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