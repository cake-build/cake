using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public static class XUnitExtensions
    {
        public static void XUnit(this ICakeContext context, string pattern)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            XUnit(context, assemblies, new XUnitSettings());
        }

        public static void XUnit(this ICakeContext context, string pattern, XUnitSettings settings)
        {
            var assemblies = context.Globber.GetFiles(pattern);
            XUnit(context, assemblies, settings);
        }

        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {            
            XUnit(context, assemblies, new XUnitSettings());
        }

        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies, XUnitSettings settings)
        {
            var runner = new XUnitRunner(context.Environment, context.Globber);
            foreach (var assembly in assemblies)
            {
                runner.Run(assembly, settings);
            }
        }
    }
}
