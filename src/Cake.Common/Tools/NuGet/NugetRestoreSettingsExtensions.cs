using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public static class NugetRestoreSettingsExtensions
    {
        public static NugetRestoreSettings WithNoCache(this NugetRestoreSettings settings)
        {
            settings.NoCache = true;
            return settings;
        }

        public static NugetRestoreSettings WithVerbosityDetailed(this NugetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Detailed;
            return settings;
        }

        public static NugetRestoreSettings WithVerbosityQuiet(this NugetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Quiet;
            return settings;
        }

        public static NugetRestoreSettings WithVerbosityNormal(this NugetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Normal;
            return settings;
        }

        
        public static NugetRestoreSettings SetToolPath(this NugetRestoreSettings settings, FilePath tooPath)
        {
            settings.ToolPath = tooPath;
            return settings;
        }
    }
}
