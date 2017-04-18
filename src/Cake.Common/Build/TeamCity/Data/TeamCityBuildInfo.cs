// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity build information for a current build.
    /// </summary>
    public class TeamCityBuildInfo : TeamCityInfo
    {
        /// <summary>
        /// Gets the build configuration name.
        /// </summary>
        /// <value>
        /// The build configuration name.
        /// </value>
        public string BuildConfName => GetEnvironmentString("TEAMCITY_BUILDCONF_NAME");

        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public string Number => GetEnvironmentString("BUILD_NUMBER");

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TeamCityBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}