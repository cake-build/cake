// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines pull request information for the current build.
    /// </summary>
    public sealed class AzurePipelinesPullRequestInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesPullRequestInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the pull request is from a fork of the repository.
        /// </summary>
        public bool IsFork => GetEnvironmentBoolean("SYSTEM_PULLREQUEST_ISFORK");

        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        public bool IsPullRequest => Id > 0;

        /// <summary>
        /// Gets the ID of the pull request that caused this build.
        /// This value is set only if the build ran because of a Git PR affected by a branch policy.
        /// </summary>
        /// <value>
        ///   The ID of the pull request that caused this build.
        /// </value>
        public int Id => GetEnvironmentInteger("SYSTEM_PULLREQUEST_PULLREQUESTID");

        /// <summary>
        /// Gets the number of the pull request that caused this build.
        /// This value is set for pull requests from GitHub which have a different pull request ID and pull request number.
        /// </summary>
        /// <value>
        ///   The number of the pull request that caused this build.
        /// </value>
        public int Number => GetEnvironmentInteger("SYSTEM_PULLREQUEST_PULLREQUESTNUMBER");

        /// <summary>
        /// Gets the branch that is being reviewed in a pull request.
        /// </summary>
        /// <remarks>
        /// This property is populated only if the build ran because of a Git PR affected by a branch policy.
        /// </remarks>
        public string SourceBranch => GetEnvironmentString("SYSTEM_PULLREQUEST_SOURCEBRANCH");

        /// <summary>
        /// Gets the URL to the repo that contains the pull requests.
        /// </summary>
        /// <remarks>
        /// This property is populated only if the build ran because of a Git PR affected by a branch policy. It is not initialized for GitHub PRs.
        /// </remarks>
        public Uri SourceRepositoryUri => GetEnvironmentUri("SYSTEM_PULLREQUEST_SOURCEREPOSITORYURI");

        /// <summary>
        /// Gets the branch that is the target of a pull request.
        /// </summary>
        /// <remarks>
        /// This property is populated only if the build ran because of a Git PR affected by a branch policy.
        /// </remarks>
        public string TargetBranch => GetEnvironmentString("SYSTEM_PULLREQUEST_TARGETBRANCH");
    }
}