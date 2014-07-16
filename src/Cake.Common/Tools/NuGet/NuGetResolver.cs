using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Contains functionality related to resolving the NuGet executable.
    /// </summary>
    public static class NuGetResolver
    {
        /// <summary>
        /// Gets the tool path.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="toolpath">The toolpath.</param>
        /// <returns>The path to NuGet.exe</returns>
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

        /// <summary>
        /// Gets the process start information.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="nugetExePath">The nuget executable path.</param>
        /// <param name="getParameters">The get parameters.</param>
        /// <returns>The process start information.</returns>
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
