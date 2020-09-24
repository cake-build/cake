// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity project information for current build.
    /// </summary>
    public class TeamCityProjectInfo : TeamCityInfo
    {
        /// <summary>
        /// Gets the TeamCity project name.
        /// </summary>
        /// <value>
        /// The TeamCity project name.
        /// </value>
        public string Name => GetEnvironmentString("TEAMCITY_PROJECT_NAME");

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityProjectInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TeamCityProjectInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}