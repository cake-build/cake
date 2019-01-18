// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitLabCI.Data
{
    /// <summary>
    /// Provides GitLab CI pull request information for the current build.
    /// </summary>
    public sealed class GitLabCIPullRequestInfo : GitLabCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIPullRequestInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        public bool IsPullRequest => Id > 0;

        /// <summary>
        /// Gets the pull request id that GitLab CI uses internally.
        /// </summary>
        /// <value>
        ///   The pull request id.
        /// </value>
        public int Id => GetEnvironmentInteger("CI_MERGE_REQUEST_ID");

        /// <summary>
        /// Gets the pull request id scoped to the project.
        /// </summary>
        /// <value>
        ///   The pull request id.
        /// </value>
        public int IId => GetEnvironmentInteger("CI_MERGE_REQUEST_IID");
    }
}