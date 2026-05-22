using System;

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
        /// Gets the value for the specified key as a boolean.
        /// Returns <c>true</c> only when the value equals "true" (case-insensitive).
        /// </summary>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">The value to return when the key is missing or not a recognized boolean.</param>
        /// <returns><c>true</c> when the configuration value is "true" (case-insensitive); otherwise <paramref name="defaultValue"/>.</returns>
        public static bool GetBoolValue(this ICakeConfiguration configuration, string key, bool defaultValue = false)
        {
            if (configuration == null)
            {
                return defaultValue;
            }

            var value = configuration.GetValue(key);
            return value != null && value.Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase) ? true : defaultValue;
        }

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
                return new DirectoryPath(toolPath).MakeAbsolute(environment).ExpandShortPath();
            }
            return defaultRoot.Combine("tools").ExpandShortPath();
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
                return new DirectoryPath(modulePath).MakeAbsolute(environment).ExpandShortPath();
            }
            var toolPath = configuration.GetToolPath(defaultRoot, environment).ExpandShortPath();
            return toolPath.Combine("Modules").Collapse();
        }
    }
}
