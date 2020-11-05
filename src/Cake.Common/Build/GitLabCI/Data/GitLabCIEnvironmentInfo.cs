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
            PullRequest = new GitLabCIPullRequestInfo(environment);
            Project = new Data.GitLabCIProjectInfo(environment);
            Runner = new Data.GitLabCIRunnerInfo(environment);
        }

        /// <summary>
        /// Gets the GitLab CI runner information.
        /// </summary>
        /// <value>
        /// The GitLab CI runner information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"Runner:
        ///         Id: {0}
        ///         Description: {1}
        ///         Tags: {2}",
        ///         BuildSystem.GitLabCI.Environment.Runner.Id,
        ///         BuildSystem.GitLabCI.Environment.Runner.Description,
        ///         BuildSystem.GitLabCI.Environment.Runner.Tags
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitLabCI.</para>
        /// <example>
        /// <code>
        /// if (GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"Runner:
        ///         Id: {0}
        ///         Description: {1}
        ///         Tags: {2}",
        ///         GitLabCI.Environment.Runner.Id,
        ///         GitLabCI.Environment.Runner.Description,
        ///         GitLabCI.Environment.Runner.Tags
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        public GitLabCIRunnerInfo Runner { get; }

        /// <summary>
        /// Gets the GitLab CI server information.
        /// </summary>
        /// <value>
        /// The GitLab CI server information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"Server:
        ///         Name: {0}
        ///         Version: {1}
        ///         Revision: {2}",
        ///         BuildSystem.GitLabCI.Environment.Server.Name,
        ///         BuildSystem.GitLabCI.Environment.Server.Version,
        ///         BuildSystem.GitLabCI.Environment.Server.Revision
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitLabCI.</para>
        /// <example>
        /// <code>
        /// if (GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"Server:
        ///         Name: {0}
        ///         Version: {1}
        ///         Revision: {2}",
        ///         GitLabCI.Environment.Server.Name,
        ///         GitLabCI.Environment.Server.Version,
        ///         GitLabCI.Environment.Server.Revision
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        public GitLabCIServerInfo Server { get; }

        /// <summary>
        /// Gets the GitLab CI build information.
        /// </summary>
        /// <value>
        /// The GitLab CI build information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Reference: {1}
        ///         Tag: {2}
        ///         Name: {3}
        ///         Stage: {4}",
        ///         BuildSystem.GitLabCI.Environment.Build.Id,
        ///         BuildSystem.GitLabCI.Environment.Build.Reference,
        ///         BuildSystem.GitLabCI.Environment.Build.Tag,
        ///         BuildSystem.GitLabCI.Environment.Build.Tag,
        ///         BuildSystem.GitLabCI.Environment.Build.Stage
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitLabCI.</para>
        /// <example>
        /// <code>
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Reference: {1}
        ///         Tag: {2}
        ///         Name: {3}
        ///         Stage: {4}",
        ///         GitLabCI.Environment.Build.Id,
        ///         GitLabCI.Environment.Build.Reference,
        ///         GitLabCI.Environment.Build.Tag,
        ///         GitLabCI.Environment.Build.Tag,
        ///         GitLabCI.Environment.Build.Stage
        ///         );
        /// </code>
        /// </example>
        public GitLabCIBuildInfo Build { get; }

        /// <summary>
        /// Gets GitLab CI pull request information.
        /// </summary>
        /// <value>
        /// The GitLab CI pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}
        ///         IId: {2}",
        ///         BuildSystem.GitLabCI.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.GitLabCI.Environment.PullRequest.Id,
        ///         BuildSystem.GitLabCI.Environment.PullRequest.IId
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitLabCI.</para>
        /// <example>
        /// <code>
        /// if (GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}
        ///         IId: {2}",
        ///         GitLabCI.Environment.PullRequest.IsPullRequest,
        ///         GitLabCI.Environment.PullRequest.Id,
        ///         GitLabCI.Environment.PullRequest.IId
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        public GitLabCIPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets the GitLab CI project information.
        /// </summary>
        /// <value>
        /// The GitLab CI project information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Id: {0}
        ///         Name: {1}
        ///         Namespace: {2}
        ///         Path: {3}
        ///         Url: {4}
        ///         Directory: {5}",
        ///         BuildSystem.GitLabCI.Environment.Project.Id,
        ///         BuildSystem.GitLabCI.Environment.Project.Name,
        ///         BuildSystem.GitLabCI.Environment.Project.Namespace,
        ///         BuildSystem.GitLabCI.Environment.Project.Path,
        ///         BuildSystem.GitLabCI.Environment.Project.Url,
        ///         BuildSystem.GitLabCI.Environment.Project.Directory
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitLabCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitLabCI.</para>
        /// <example>
        /// <code>
        ///     Information(
        ///         @"Project:
        ///         Id: {0}
        ///         Name: {1}
        ///         Namespace: {2}
        ///         Path: {3}
        ///         Url: {4}
        ///         Directory: {5}",
        ///         GitLabCI.Environment.Project.Id,
        ///         GitLabCI.Environment.Project.Name,
        ///         GitLabCI.Environment.Project.Namespace,
        ///         GitLabCI.Environment.Project.Path,
        ///         GitLabCI.Environment.Project.Url,
        ///         GitLabCI.Environment.Project.Directory
        ///         );
        /// </code>
        /// </example>
        public GitLabCIProjectInfo Project { get; }
    }
}
