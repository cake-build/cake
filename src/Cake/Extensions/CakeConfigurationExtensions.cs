using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;

namespace Cake
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeConfiguration"/>.
    /// </summary>
    public static class CakeConfigurationExtensions
    {
        /// <summary>
        /// Gets the script cache directory path.
        /// </summary>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="defaultRoot">The default root path.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>The script cache directory path.</returns>
        public static DirectoryPath GetScriptCachePath(this ICakeConfiguration configuration, DirectoryPath defaultRoot, ICakeEnvironment environment)
        {
            var cachePath = configuration.GetValue(Constants.Cache.Path);
            if (!string.IsNullOrWhiteSpace(cachePath))
            {
                return new DirectoryPath(cachePath).MakeAbsolute(environment);
            }
            var toolPath = configuration.GetToolPath(defaultRoot, environment);
            return toolPath.Combine("cache").Collapse();
        }
    }
}
