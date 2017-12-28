using Cake.Core.Configuration;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeConfiguration"/>.
    /// </summary>
    public static class CakeConfigurationExtensions
    {
        /// <summary>
        /// Gets the tool directory path.
        /// </summary>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="defaultRoot">The default root path.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>The tool directory path.</returns>
        public static DirectoryPath GetToolPath(this ICakeConfiguration configuration, DirectoryPath defaultRoot, ICakeEnvironment environment)
        {
            var toolPath = configuration.GetValue(Constants.Paths.Tools);
            if (!string.IsNullOrWhiteSpace(toolPath))
            {
                return new DirectoryPath(toolPath).MakeAbsolute(environment);
            }
            return defaultRoot.Combine("tools");
        }

        /// <summary>
        /// Gets the module directory path.
        /// </summary>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="defaultRoot">The default root path.</param>
        /// <param name="environment">The environment.</param>
        /// <returns>The module directory path.</returns>
        public static DirectoryPath GetModulePath(this ICakeConfiguration configuration, DirectoryPath defaultRoot, ICakeEnvironment environment)
        {
            var modulePath = configuration.GetValue(Constants.Paths.Modules);
            if (!string.IsNullOrWhiteSpace(modulePath))
            {
                return new DirectoryPath(modulePath).MakeAbsolute(environment);
            }
            var toolPath = configuration.GetToolPath(defaultRoot, environment);
            return toolPath.Combine("Modules").Collapse();
        }
    }
}
