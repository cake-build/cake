using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public static class XUnitExtensions
    {
        public static void XUnit(this ICakeContext context, string pattern)
        {
            var runner = new XUnitRunner();
            var assemblies = context.GetFiles(pattern);
            foreach (var assembly in assemblies)
            {
                var settings = new XUnitSettings(assembly);
                runner.Run(context, settings);   
            }
        }

        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            var runner = new XUnitRunner();
            foreach (var assembly in assemblies)
            {
                var settings = new XUnitSettings(assembly);
                runner.Run(context, settings);                   
            }
        }
    }
}
