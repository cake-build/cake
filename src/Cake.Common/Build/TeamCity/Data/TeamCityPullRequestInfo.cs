// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity pull request information for current build
    /// </summary>
    public class TeamCityPullRequestInfo : TeamCityInfo
    {
        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// <c>env.Git_Branch</c> is a required parameter in TeamCity for this to work
        /// </remarks>
        public bool IsPullRequest => GetEnvironmentString("Git_Branch").ToUpper().Contains("PULL-REQUEST");

        /// <summary>
        /// Gets the pull request number
        /// </summary>
        /// <value>
        /// The pull request number
        /// </value>
        /// <remarks>
        /// <c>env.Git_Branch</c> is a required parameter in TeamCity for this to work
        /// </remarks>
        public int? Number => IsPullRequest ? int.Parse(GetEnvironmentString("Git_Branch").Remove(GetEnvironmentString("Git_Branch").LastIndexOf('/')).Remove(0, "refs/pull-requests/".Length)) : (int?)null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TeamCityPullRequestInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}