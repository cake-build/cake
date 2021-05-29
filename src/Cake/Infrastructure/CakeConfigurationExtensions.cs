// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.IO;

namespace Cake.Infrastructure
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeConfiguration"/>.
    /// </summary>
    internal static class CakeConfigurationExtensions
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
            var cachePath = configuration.GetValue(Constants.Paths.Cache);
            if (!string.IsNullOrWhiteSpace(cachePath))
            {
                return new DirectoryPath(cachePath).MakeAbsolute(environment);
            }
            var toolPath = configuration.GetToolPath(defaultRoot, environment);
            return toolPath.Combine("cache").Collapse();
        }
    }
}
