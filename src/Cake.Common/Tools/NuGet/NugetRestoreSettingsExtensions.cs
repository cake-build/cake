using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public static class NuGetRestoreSettingsExtensions
    {
        public static NuGetRestoreSettings WithNoCache(this NuGetRestoreSettings settings)
        {
            settings.NoCache = true;
            return settings;
        }

        public static NuGetRestoreSettings WithVerbosityDetailed(this NuGetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Detailed;
            return settings;
        }

        public static NuGetRestoreSettings WithVerbosityQuiet(this NuGetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Quiet;
            return settings;
        }

        public static NuGetRestoreSettings WithVerbosityNormal(this NuGetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Normal;
            return settings;
        }

        
        public static NuGetRestoreSettings SetToolPath(this NuGetRestoreSettings settings, FilePath tooPath)
        {
            settings.ToolPath = tooPath;
            return settings;
        }
    }
}
