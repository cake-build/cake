// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitHubActions.Commands;
using Cake.Common.Build.GitHubActions.Data;

namespace Cake.Common.Build.GitHubActions
{
    /// <summary>
    /// Represents a GitHub Actions provider.
    /// </summary>
    public interface IGitHubActionsProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on GitHub Actions.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on GitHub Actions; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information("Running on GitHub Actions");
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHub Actions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     GitHubActions.Commands.Debug("Running on GitHub Actions");
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHub Actions");
        /// }
        /// </code>
        /// </example>
        bool IsRunningOnGitHubActions { get; }

        /// <summary>
        /// Gets the GitHub Actions environment.
        /// </summary>
        /// <value>
        /// The GitHub Actions environment.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Workflow:
        ///         Workflow: {0}
        ///         Repository: {1}
        ///         Actor: {2}
        ///         Runner OS: {3}
        ///         Runner Environment: {4}
        ///         Runner Architecture: {5}",
        ///         BuildSystem.GitHubActions.Environment.Workflow.Workflow,
        ///         BuildSystem.GitHubActions.Environment.Workflow.Repository,
        ///         BuildSystem.GitHubActions.Environment.Workflow.Actor,
        ///         BuildSystem.GitHubActions.Environment.Runner.OS,
        ///         BuildSystem.GitHubActions.Environment.Runner.Environment,
        ///         BuildSystem.GitHubActions.Environment.Runner.Architecture
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHub Actions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(
        ///         @"Workflow:
        ///         Workflow: {0}
        ///         Repository: {1}
        ///         Actor: {2}
        ///         Runner OS: {3}
        ///         Runner Environment: {4}
        ///         Runner Architecture: {5}",
        ///         GitHubActions.Environment.Workflow.Workflow,
        ///         GitHubActions.Environment.Workflow.Repository,
        ///         GitHubActions.Environment.Workflow.Actor,
        ///         GitHubActions.Environment.Runner.OS,
        ///         GitHubActions.Environment.Runner.Environment,
        ///         GitHubActions.Environment.Runner.Architecture
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHub Actions");
        /// }
        /// </code>
        /// </example>
        GitHubActionsEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the GitHub Actions commands.
        /// </summary>
        /// <value>
        /// The GitHub Actions commands.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     BuildSystem.GitHubActions.Commands.Notice("Build started");
        ///     BuildSystem.GitHubActions.Commands.SetOutputParameter(
        ///         "cake_version",
        ///         Context.Environment.Runtime.CakeVersion.ToString(3));
        ///     BuildSystem.GitHubActions.Commands.SetStepSummary("## Cake Version\n" +
        ///         Context.Environment.Runtime.CakeVersion.ToString(3));
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHub Actions");
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     GitHubActions.Commands.Notice("Build started");
        ///     GitHubActions.Commands.SetOutputParameter(
        ///         "cake_version",
        ///         Context.Environment.Runtime.CakeVersion.ToString(3));
        ///     GitHubActions.Commands.SetStepSummary("## Cake Version\n" +
        ///         Context.Environment.Runtime.CakeVersion.ToString(3));
        /// }
        /// else
        /// {
        ///     Information("Not running on GitHub Actions");
        /// }
        /// </code>
        /// </example>
        public GitHubActionsCommands Commands { get; }
    }
}
