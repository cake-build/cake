using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.MSBuild;
using Cake.XUnit;

namespace Cake.Scripting.Host
{
    public sealed partial class ScriptHost
    {
        public void XUnit(string pattern)
        {
            ((ICakeContext)this).XUnit(pattern);
        }

        public void XUnit(IEnumerable<FilePath> assemblies)
        {
            ((ICakeContext)this).XUnit(assemblies);
        }
    }
}
