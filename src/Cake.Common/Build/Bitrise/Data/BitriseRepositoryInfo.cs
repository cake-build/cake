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
        /// Gets the git repository URL.
        /// </summary>
        /// <value>
        /// The git repository URL.
        /// </value>
        public string GitRepositoryUrl
        {
            get { return GetEnvironmentString("GIT_REPOSITORY_URL"); }
        }

        /// <summary>
        /// Gets the git branch.
        /// </summary>
        /// <value>
        /// The git branch.
        /// </value>
        public string GitBranch
        {
            get { return GetEnvironmentString("BITRISE_GIT_BRANCH"); }
        }

        /// <summary>
        /// Gets the git tag.
        /// </summary>
        /// <value>
        /// The git tag.
        /// </value>
        public string GitTag
        {
            get { return GetEnvironmentString("BITRISE_GIT_TAG"); }
        }

        /// <summary>
        /// Gets the git commit.
        /// </summary>
        /// <value>
        /// The git commit.
        /// </value>
        public string GitCommit
        {
            get { return GetEnvironmentString("BITRISE_GIT_COMMIT"); }
        }

        /// <summary>
        /// Gets the pull request.
        /// </summary>
        /// <value>
        /// The pull request.
        /// </value>
        public string PullRequest
        {
            get { return GetEnvironmentString("BITRISE_PULL_REQUEST"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseRepositoryInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
