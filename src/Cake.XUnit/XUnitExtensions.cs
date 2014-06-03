using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Extensions;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public static class XUnitExtensions
    {
        public static void XUnit(this ICakeContext context, string pattern)
        {
            var assemblies = context.GetFiles(pattern);
            var settings = new XUnitSettings(assemblies);
            new XUnitRunner().Run(context, settings);
        }

        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            var settings = new XUnitSettings(assemblies);
            new XUnitRunner().Run(context, settings);
        }
    }
}
