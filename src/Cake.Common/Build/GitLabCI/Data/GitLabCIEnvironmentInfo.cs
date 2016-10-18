// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitLabCI.Data
{
    /// <summary>
    /// Provides GitLab CI environment information for a current build.
    /// </summary>
    public sealed class GitLabCIEnvironmentInfo : GitLabCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Server = new GitLabCIServerInfo(environment);
            Build = new Data.GitLabCIBuildInfo(environment);
            Project = new Data.GitLabCIProjectInfo(environment);
            Runner = new Data.GitLabCIRunnerInfo(environment);
        }

        /// <summary>
        /// Gets the GitLab CI runner information.
        /// </summary>
        /// <value>
        /// The GitLab CI runner information.
        /// </value>
        public GitLabCIRunnerInfo Runner { get; }

        /// <summary>
        /// Gets the GitLab CI server information.
        /// </summary>
        /// <value>
        /// The GitLab CI server information.
        /// </value>
        public GitLabCIServerInfo Server { get; }

        /// <summary>
        /// Gets the GitLab CI build information.
        /// </summary>
        /// <value>
        /// The GitLab CI build information.
        /// </value>
        public GitLabCIBuildInfo Build { get; }

        /// <summary>
        /// Gets the GitLab CI project information.
        /// </summary>
        /// <value>
        /// The GitLab CI project information.
        /// </value>
        public GitLabCIProjectInfo Project { get; }
    }
}
