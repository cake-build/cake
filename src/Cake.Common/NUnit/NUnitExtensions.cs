using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.NUnit
{
    public static class NUnitExtensions
    {
        public static void NUnit(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            NUnit(context, assemblies, new NUnitSettings());
        }

        public static void NUnit(this ICakeContext context, string pattern, NUnitSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            NUnit(context, assemblies, settings);
        }

        public static void NUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            NUnit(context, assemblies, new NUnitSettings());
        }

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
