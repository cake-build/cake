using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.XUnit
{
    public static class XUnitExtensions
    {
        public static void XUnit(this ICakeContext context, IEnumerable<FilePath> assemblies)
        {
            var settings = new XUnitSettings(assemblies);
            new XUnitRunner().Run(context, settings);
        }
    }
}
