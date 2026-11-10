// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI.Data;

namespace Cake.Common.Build.GitLabCI;

/// <summary>
/// Represents a GitLab CI provider.
/// </summary>
public interface IGitLabCIProvider
{
    /// <summary>
    /// Gets a value indicating whether the current build is running on GitLab CI.
    /// </summary>
    /// <value>
    /// <c>true</c> if the current build is running on GitLab CI; otherwise, <c>false</c>.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
    /// {
    ///     Information("Running on GitLab CI");
    /// }
    /// else
    /// {
    ///     Information("Not running on GitLab CI");
    /// }
    /// </code>
    /// </example>
    /// <para>Via GitLabCI.</para>
    /// <example>
    /// <code>
    /// if (GitLabCI.IsRunningOnGitLabCI)
    /// {
    ///     Information("Running on GitLab CI");
    /// }
    /// else
    /// {
    ///     Information("Not running on GitLab CI");
    /// }
    /// </code>
    /// </example>
    bool IsRunningOnGitLabCI { get; }

    /// <summary>
    /// Gets the GitLab CI environment.
    /// </summary>
    /// <value>
    /// The GitLab CI environment.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
    /// {
    ///     var commitHash = BuildSystem.GitLabCI.Environment.Build.Reference;
    /// }
    /// </code>
    /// </example>
    /// <para>Via GitLabCI.</para>
    /// <example>
    /// <code>
    /// if (GitLabCI.IsRunningOnGitLabCI)
    /// {
    ///     var commitHash = GitLabCI.Environment.Build.Reference;
    /// }
    /// </code>
    /// </example>
    GitLabCIEnvironmentInfo Environment { get; }

    /// <summary>
    /// Gets the GitLab CI commands.
    /// </summary>
    /// <value>
    /// The GitLab CI commands.
    /// </value>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.GitLabCI.IsRunningOnGitLabCI)
    /// {
    ///     BuildSystem.GitLabCI.Commands.SetEnvironmentVariable(
    ///         "./gitlab.env",
    ///         "MY_VAR",
    ///         "value");
    /// }
    /// </code>
    /// </example>
    /// <para>Via GitLabCI.</para>
    /// <example>
    /// <code>
    /// if (GitLabCI.IsRunningOnGitLabCI)
    /// {
    ///     GitLabCI.Commands.SetEnvironmentVariable(
    ///         "./gitlab.env",
    ///         "MY_VAR",
    ///         "value");
    /// }
    /// </code>
    /// </example>
    public GitLabCICommands Commands { get; }
}
