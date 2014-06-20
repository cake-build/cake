using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    public static class NUnitExtensions
    {
        [CakeScriptMethod]
        public static void NUnit(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            NUnit(context, assemblies, new NUnitSettings());
        }

        [CakeScriptMethod]
        public static void NUnit(this ICakeContext context, string pattern, NUnitSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            NUnit(context, assemblies, settings);
        }

        [CakeScriptMethod]
        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            NUnit(context, assemblies, new NUnitSettings());
        }

        [CakeScriptMethod]
        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies, NUnitSettings settings)
        {
            var runner = new NUnitRunner(context.Environment, context.Globber, new ProcessRunner());            
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}
