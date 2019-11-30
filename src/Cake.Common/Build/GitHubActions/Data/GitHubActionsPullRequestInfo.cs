// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provides GitHub Actions pull request information for the current build.
    /// </summary>
    public sealed class GitHubActionsPullRequestInfo : GitHubActionsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitHubActionsPullRequestInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        public bool IsPullRequest => GetEnvironmentString("GITHUB_EVENT_NAME") == "pull_request";
    }
}
