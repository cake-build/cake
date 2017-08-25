// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity environment information for current build
    /// </summary>
    public class TeamCityEnvironmentInfo : TeamCityInfo
    {
        /// <summary>
        /// Gets TeamCity project information.
        /// </summary>
        /// <value>
        /// The TeamCity project information.
        /// </value>
        public TeamCityProjectInfo Project { get; }

        /// <summary>
        /// Gets TeamCity build information.
        /// </summary>
        /// <value>
        /// The TeamCity build information.
        /// </value>
        public TeamCityBuildInfo Build { get; }

        /// <summary>
        /// Gets TeamCity pull-request information.
        /// </summary>
        /// <value>
        /// The TeamCity pull-request information.
        /// </value>
        public TeamCityPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TeamCityEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Project = new TeamCityProjectInfo(environment);
            Build = new TeamCityBuildInfo(environment);
            PullRequest = new TeamCityPullRequestInfo(environment);
        }
    }
}