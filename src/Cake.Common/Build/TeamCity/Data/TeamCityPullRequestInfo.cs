// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity pull request information for current build.
    /// </summary>
    public class TeamCityPullRequestInfo : TeamCityInfo
    {
        private readonly TeamCityBuildInfo _buildInfo;

        private bool InferIsPullRequest()
        {
            var gitReferenceName = GetBranchRef();

            if (string.IsNullOrEmpty(gitReferenceName))
            {
                return false;
            }

            var branchSlices = gitReferenceName.Split('/');

            if (branchSlices.Length >= 3)
            {
                switch (branchSlices[1].ToUpper())
                {
                    case "CHANGES":
                    case "MERGE-REQUESTS":
                    case "PULL":
                    case "PULL-REQUESTS":
                        return true;
                    default:
                        return false;
                }
            }

            return false;
        }

        private int? GetPullRequestNumber()
        {
            var gitReferenceName = GetBranchRef();

            if (string.IsNullOrEmpty(gitReferenceName))
            {
                return null;
            }

            var branchSlices = gitReferenceName.Split('/');

            if (int.TryParse(branchSlices[2], out var pullRequestNumber))
            {
                return pullRequestNumber;
            }

            return null;
        }

        private string GetBranchRef()
        {
            var gitBranch = GetEnvironmentString("Git_Branch");

            if (string.IsNullOrWhiteSpace(gitBranch))
            {
                gitBranch = _buildInfo.VcsBranchName;
            }

            return gitBranch;
        }

        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// <c>env.Git_Branch</c> is a required parameter in TeamCity for this to work.
        /// </remarks>
        public bool IsPullRequest => InferIsPullRequest();

        /// <summary>
        /// Gets the pull request number.
        /// </summary>
        /// <value>
        /// The pull request number.
        /// </value>
        /// <remarks>
        /// <c>env.Git_Branch</c> is a required parameter in TeamCity for this to work.
        /// </remarks>
        public int? Number => IsPullRequest ? GetPullRequestNumber() : null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="buildInfo">The TeamCity build info.</param>
        public TeamCityPullRequestInfo(ICakeEnvironment environment, TeamCityBuildInfo buildInfo)
            : base(environment)
        {
            _buildInfo = buildInfo;
        }
    }
}