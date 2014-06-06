using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.IO;
using Cake.MSBuild;

namespace Cake.Scripting.Host
{
    public sealed partial class ScriptHost
    {
        public void MSBuild(FilePath solution)
        {
            ((ICakeContext)this).MSBuild(solution);
        }

        public void MSBuild(FilePath solution, Action<MSBuildSettings> configurator)
        {
            ((ICakeContext)this).MSBuild(solution, configurator);
        }
    }
}
