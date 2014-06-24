using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public static class NuGetResolver
    {
        public static FilePath GetToolPath(ICakeEnvironment environment, IGlobber globber, FilePath toolpath)
        {
            if (toolpath != null)
            {
                return toolpath.MakeAbsolute(environment);
            }
            var expression = string.Format("./tools/**/NuGet.exe");
            var nugetExePath = globber.GetFiles(expression).FirstOrDefault();
            if (nugetExePath == null)
            {
                throw new CakeException("Could not find NuGet.exe.");
            }
            return nugetExePath;
        }

        public static  ProcessStartInfo GetProcessStartInfo(ICakeEnvironment environment, FilePath nugetExePath, Func<ICollection<string>> getParameters)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            if (nugetExePath == null)
            {
                throw new ArgumentNullException("nugetExePath");
            }

            if (getParameters == null)
            {
                throw new ArgumentNullException("getParameters");
            }

            var parameters = getParameters();

            return new ProcessStartInfo(nugetExePath.FullPath)
            {
                WorkingDirectory = environment.WorkingDirectory.FullPath,
                Arguments = string.Join(" ", parameters),
                UseShellExecute = false
            };
        }
    }
}
