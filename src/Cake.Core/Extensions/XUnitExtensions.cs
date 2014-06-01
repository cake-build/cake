using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.XUnit;

namespace Cake.Core.Extensions
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
