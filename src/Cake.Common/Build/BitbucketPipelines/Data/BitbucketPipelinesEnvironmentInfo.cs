// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.BitbucketPipelines.Data
{
    /// <summary>
    /// Provides Bitbucket Pipelines environment information for the current build.
    /// </summary>
    public class BitbucketPipelinesEnvironmentInfo : BitbucketPipelinesInfo
    {
        /// <summary>
        /// Gets Bitbucket Pipelines repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.BitbucketPipelines.IsRunningOnBitbucketPipelines)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         Tag: {1}
        ///         Commit: {2}
        ///         Repo Owner: {3}
        ///         Repo Slug: {4}",
        ///         BuildSystem.BitbucketPipelines.Environment.Repository.Branch,
        ///         BuildSystem.BitbucketPipelines.Environment.Repository.Tag,
        ///         BuildSystem.BitbucketPipelines.Environment.Repository.Commit,
        ///         BuildSystem.BitbucketPipelines.Environment.Repository.RepoOwner,
        ///         BuildSystem.BitbucketPipelines.Environment.Repository.RepoSlug
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on BitbucketPipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via BitbucketPipelines.</para>
        /// <example>
        /// <code>
        /// if (BitbucketPipelines.IsRunningOnBitbucketPipelines)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         Tag: {1}
        ///         Commit: {2}
        ///         Repo Owner: {3}
        ///         Repo Slug: {4}",
        ///         BitbucketPipelines.Environment.Repository.Branch,
        ///         BitbucketPipelines.Environment.Repository.Tag,
        ///         BitbucketPipelines.Environment.Repository.Commit,
        ///         BitbucketPipelines.Environment.Repository.RepoOwner,
        ///         BitbucketPipelines.Environment.Repository.RepoSlug
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on BitbucketPipelines");
        /// }
        /// </code>
        /// </example>
        public BitbucketPipelinesRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets Bitbucket Pipelines pull request information.
        /// </summary>
        /// <value>
        /// The Bitbucket Pipelines pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.BitbucketPipelines.IsRunningOnBitbucketPipelines)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}",
        ///         BuildSystem.BitbucketPipelines.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.BitbucketPipelines.Environment.PullRequest.Id
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on BitbucketPipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via BitbucketPipelines.</para>
        /// <example>
        /// <code>
        /// if (BitbucketPipelines.IsRunningOnBitbucketPipelines)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}",
        ///         BitbucketPipelines.Environment.PullRequest.IsPullRequest,
        ///         BitbucketPipelines.Environment.PullRequest.Id
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on BitbucketPipelines");
        /// }
        /// </code>
        /// </example>
        public BitbucketPipelinesPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitbucketPipelinesEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitbucketPipelinesEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            Repository = new BitbucketPipelinesRepositoryInfo(environment);
            PullRequest = new BitbucketPipelinesPullRequestInfo(environment);
        }
    }
}