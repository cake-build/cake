// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provides GitHub Actions environment information for a current build.
    /// </summary>
    public sealed class GitHubActionsEnvironmentInfo : GitHubActionsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitHubActionsEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Runner = new GitHubActionsRunnerInfo(environment);
            Workflow = new GitHubActionsWorkflowInfo(environment);
            PullRequest = new GitHubActionsPullRequestInfo(environment);
            Runtime = new GitHubActionsRuntimeInfo(environment);
        }

        /// <summary>
        /// Gets the GitHub Actions home directory.
        /// </summary>
        /// <value>
        /// The home.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Home: {0}",
        ///         BuildSystem.GitHubActions.Environment.Home
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Home: {0}",
        ///         GitHubActions.Environment.Home
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        public DirectoryPath Home => GetEnvironmentDirectoryPath("HOME");

        /// <summary>
        /// Gets GitHub Actions runner information.
        /// </summary>
        /// <value>
        /// The GitHub Actions runner information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Runner:
        ///         OS: {0}
        ///         Temp: {1}
        ///         ToolCache: {2}
        ///         Workspace: {3}",
        ///         BuildSystem.GitHubActions.Environment.Runner.OS,
        ///         BuildSystem.GitHubActions.Environment.Runner.Temp,
        ///         BuildSystem.GitHubActions.Environment.Runner.ToolCache,
        ///         BuildSystem.GitHubActions.Environment.Runner.Workspace
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Runner:
        ///         OS: {0}
        ///         Temp: {1}
        ///         ToolCache: {2}
        ///         Workspace: {3}",
        ///         GitHubActions.Environment.Runner.OS,
        ///         GitHubActions.Environment.Runner.Temp,
        ///         GitHubActions.Environment.Runner.ToolCache,
        ///         GitHubActions.Environment.Runner.Workspace
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        public GitHubActionsRunnerInfo Runner { get; }

        /// <summary>
        /// Gets the GitHub Actions workflow information.
        /// </summary>
        /// <value>
        /// The GitHub Actions workflow information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Workflow:
        ///         Workflow: {0}
        ///         Action: {1}
        ///         Actor: {2}",
        ///         BuildSystem.GitHubActions.Environment.Workflow.Workflow,
        ///         BuildSystem.GitHubActions.Environment.Workflow.Action,
        ///         BuildSystem.GitHubActions.Environment.Workflow.Actor
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        ///     Information(
        ///         @"Workflow:
        ///         Workflow: {0}
        ///         Action: {1}
        ///         Actor: {2}",
        ///         GitHubActions.Environment.Workflow.Workflow,
        ///         GitHubActions.Environment.Workflow.Action,
        ///         GitHubActions.Environment.Workflow.Actor
        ///         );
        /// </code>
        /// </example>
        public GitHubActionsWorkflowInfo Workflow { get; }

        /// <summary>
        /// Gets GitHub Actions pull request information.
        /// </summary>
        /// <value>
        /// The GitHub Actions pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}",
        ///         BuildSystem.GitHubActions.Environment.PullRequest.IsPullRequest
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}",
        ///         GitHubActions.Environment.PullRequest.IsPullRequest
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHubActions");
        /// }
        /// </code>
        /// </example>
        public GitHubActionsPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets GitHub Actions runtime information.
        /// </summary>
        /// <value>
        /// The GitHub Actions runtime information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// // TODO
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// // TODO
        /// </code>
        /// </example>
        public GitHubActionsRuntimeInfo Runtime { get; }
    }
}
