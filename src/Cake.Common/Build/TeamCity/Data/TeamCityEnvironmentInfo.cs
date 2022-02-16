// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity environment information for current build.
    /// </summary>
    public class TeamCityEnvironmentInfo : TeamCityInfo
    {
        /// <summary>
        /// Gets TeamCity project information.
        /// </summary>
        /// <value>
        /// The TeamCity project information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Name: {0}",
        ///         BuildSystem.TeamCity.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Name: {0}",
        ///         TeamCity.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        public TeamCityProjectInfo Project { get; }

        /// <summary>
        /// Gets TeamCity build information.
        /// </summary>
        /// <value>
        /// The TeamCity build information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Build:
        ///         BuildConfName: {0}
        ///         Number: {1}",
        ///         BuildSystem.TeamCity.Environment.Build.BuildConfName,
        ///         BuildSystem.TeamCity.Environment.Build.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Build:
        ///         BuildConfName: {0}
        ///         Number: {1}",
        ///         TeamCity.Environment.Build.BuildConfName,
        ///         TeamCity.Environment.Build.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        public TeamCityBuildInfo Build { get; }

        /// <summary>
        /// Gets TeamCity pull-request information.
        /// </summary>
        /// <value>
        /// The TeamCity pull-request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Number: {1}",
        ///         BuildSystem.TeamCity.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TeamCity.Environment.PullRequest.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Number: {1}",
        ///         TeamCity.Environment.PullRequest.IsPullRequest,
        ///         TeamCity.Environment.PullRequest.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        public TeamCityPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        public TeamCityEnvironmentInfo(ICakeEnvironment environment, IFileSystem fileSystem)
            : base(environment)
        {
            Project = new TeamCityProjectInfo(environment);
            Build = new TeamCityBuildInfo(environment, fileSystem);
            PullRequest = new TeamCityPullRequestInfo(environment, Build);
        }
    }
}