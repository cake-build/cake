// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise repository information for the current build.
    /// </summary>
    public class BitriseRepositoryInfo : BitriseInfo
    {
        /// <summary>
        /// Gets the Git repository URL.
        /// </summary>
        /// <value>
        /// The Git repository URL.
        /// </value>
        public string GitRepositoryUrl => GetEnvironmentString("GIT_REPOSITORY_URL");

        /// <summary>
        /// Gets the Git branch.
        /// </summary>
        /// <value>
        /// The Git branch.
        /// </value>
        public string GitBranch => GetEnvironmentString("BITRISE_GIT_BRANCH");

        /// <summary>
        /// Gets the Git tag.
        /// </summary>
        /// <value>
        /// The Git tag.
        /// </value>
        public string GitTag => GetEnvironmentString("BITRISE_GIT_TAG");

        /// <summary>
        /// Gets the Git commit.
        /// </summary>
        /// <value>
        /// The Git commit.
        /// </value>
        public string GitCommit => GetEnvironmentString("BITRISE_GIT_COMMIT");

        /// <summary>
        /// Gets the pull request.
        /// </summary>
        /// <value>
        /// The pull request.
        /// </value>
        public string PullRequest => GetEnvironmentString("BITRISE_PULL_REQUEST");

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseRepositoryInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}