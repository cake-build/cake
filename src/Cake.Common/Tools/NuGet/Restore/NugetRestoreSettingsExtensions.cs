using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Restore
{
    /// <summary>
    /// Contains functionality for restoring NuGet packages in solution.
    /// </summary>
    public static class NuGetRestoreSettingsExtensions
    {
        /// <summary>
        /// Disables the local package cache.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="NuGetRestoreSettingsExtensions"/> instance so that multiple calls can be chained.</returns>
        public static NuGetRestoreSettings WithNoCache(this NuGetRestoreSettings settings)
        {
            settings.NoCache = true;
            return settings;
        }

        /// <summary>
        /// Sets detailed verbosity.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="NuGetRestoreSettingsExtensions"/> instance so that multiple calls can be chained.</returns>
        public static NuGetRestoreSettings WithVerbosityDetailed(this NuGetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Detailed;
            return settings;
        }

        /// <summary>
        /// Sets quiet verbosity.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="NuGetRestoreSettingsExtensions"/> instance so that multiple calls can be chained.</returns>
        public static NuGetRestoreSettings WithVerbosityQuiet(this NuGetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Quiet;
            return settings;
        }

        /// <summary>
        /// Sets normal verbosity.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="NuGetRestoreSettingsExtensions"/> instance so that multiple calls can be chained.</returns>
        public static NuGetRestoreSettings WithVerbosityNormal(this NuGetRestoreSettings settings)
        {
            settings.Verbosity = NuGetVerbosity.Normal;
            return settings;
        }


        /// <summary>
        /// Sets the NuGet tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="tooPath">The tool path.</param>
        /// <returns>The same <see cref="NuGetRestoreSettingsExtensions"/> instance so that multiple calls can be chained.</returns>
        public static NuGetRestoreSettings SetToolPath(this NuGetRestoreSettings settings, FilePath tooPath)
        {
            settings.ToolPath = tooPath;
            return settings;
        }
    }
}
