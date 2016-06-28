// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.BitbucketPipelines.Data
{
    /// <summary>
    /// Provides Bitbucket Pipelines repository information for the current build.
    /// </summary>
    public class BitbucketPipelinesRepositoryInfo : BitbucketPipelinesInfo
    {
        /// <summary>
        /// Gets the branch on which the build was kicked off. This value is only available on branches.
        /// </summary>
        /// <remarks>Note: <see cref="Branch"/> and <see cref="Tag"/> are mutually exclusive. If you use both, only one will have a value.</remarks>
        /// <value>
        /// The SCM branch.
        /// </value>
        public string Branch => GetEnvironmentString("BITBUCKET_BRANCH");

        /// <summary>
        /// Gets the tag on which the build was kicked off. This value is only available when tagged.
        /// </summary>
        /// <remarks>Note: <see cref="Branch"/> and <see cref="Tag"/> are mutually exclusive. If you use both, only one will have a value.</remarks>
        /// <value>
        /// The SCM tag.
        /// </value>
        public string Tag => GetEnvironmentString("BITBUCKET_TAG");

        /// <summary>
        /// Gets the commit hash of a commit that kicked off the build.
        /// </summary>
        /// <value>
        /// The SCM commit.
        /// </value>
        public string Commit => GetEnvironmentString("BITBUCKET_COMMIT");

        /// <summary>
        /// Gets the name of the account in which the repository lives.
        /// </summary>
        /// <value>
        /// The repository owner account.
        /// </value>
        public string RepoOwner => GetEnvironmentString("BITBUCKET_REPO_OWNER");

        /// <summary>
        /// Gets the URL-friendly version of a repository name.
        /// </summary>
        /// <value>
        /// The URL-friendly repository name.
        /// </value>
        public string RepoSlug => GetEnvironmentString("BITBUCKET_REPO_SLUG");

        /// <summary>
        /// Initializes a new instance of the <see cref="BitbucketPipelinesRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitbucketPipelinesRepositoryInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}