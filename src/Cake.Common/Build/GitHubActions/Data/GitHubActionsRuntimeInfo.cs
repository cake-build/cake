// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provides GitHub Actions runtime information for the current build.
    /// </summary>
    public sealed class GitHubActionsRuntimeInfo : GitHubActionsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsRuntimeInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitHubActionsRuntimeInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the GitHub Actions Runtime is available for the current build.
        /// </summary>
        /// <value>
        /// <c>true</c> if the GitHub Actions Runtime is available for the current build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.Environment.Runtime.IsRuntimeAvailable)
        /// {
        ///     await BuildSystem.GitHubActions.Commands.UploadArtifact(path, name);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.Environment.Runtime.IsRuntimeAvailable)
        /// {
        ///     await GitHubActions.Commands.UploadArtifact(path, name);
        /// }
        /// </code>
        /// </example>
        public bool IsRuntimeAvailable
            => !string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(Url) && !string.IsNullOrWhiteSpace(ResultsUrl);

        /// <summary>
        /// Gets the current runtime API authorization token.
        /// </summary>
        /// <value>
        /// The current runtime API authorization token.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     var hasToken = !string.IsNullOrWhiteSpace(BuildSystem.GitHubActions.Environment.Runtime.Token);
        ///     Information(@"Runtime token configured: {0}", hasToken);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     var hasToken = !string.IsNullOrWhiteSpace(GitHubActions.Environment.Runtime.Token);
        ///     Information(@"Runtime token configured: {0}", hasToken);
        /// }
        /// </code>
        /// </example>
        public string Token => GetEnvironmentString("ACTIONS_RUNTIME_TOKEN");

        /// <summary>
        /// Gets the current runtime API endpoint url.
        /// </summary>
        /// <value>
        /// The current runtime API endpoint url.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime Url: {0}", BuildSystem.GitHubActions.Environment.Runtime.Url);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime Url: {0}", GitHubActions.Environment.Runtime.Url);
        /// }
        /// </code>
        /// </example>
        public string Url => GetEnvironmentString("ACTIONS_RUNTIME_URL");

        /// <summary>
        /// Gets the current runtime API endpoint url for the job.
        /// </summary>
        /// <value>
        /// The current runtime API endpoint url for the job.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime ResultsUrl: {0}", BuildSystem.GitHubActions.Environment.Runtime.ResultsUrl);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime ResultsUrl: {0}", GitHubActions.Environment.Runtime.ResultsUrl);
        /// }
        /// </code>
        /// </example>
        public string ResultsUrl => GetEnvironmentString("ACTIONS_RESULTS_URL");

        /// <summary>
        /// Gets the path to environment file to set an environment variable that the following steps in a job can use.
        /// </summary>
        /// <value>
        /// The path to environment file to set an environment variable that the following steps in a job can use.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime EnvPath: {0}", BuildSystem.GitHubActions.Environment.Runtime.EnvPath);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime EnvPath: {0}", GitHubActions.Environment.Runtime.EnvPath);
        /// }
        /// </code>
        /// </example>
        public FilePath EnvPath => GetEnvironmentFilePath("GITHUB_ENV");

        /// <summary>
        /// Gets the path to output file to set an output parameter that the following steps in a job can use.
        /// </summary>
        /// <value>
        /// The path to output file to set an output parameter that the following steps in a job can use.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime OutputPath: {0}", BuildSystem.GitHubActions.Environment.Runtime.OutputPath);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime OutputPath: {0}", GitHubActions.Environment.Runtime.OutputPath);
        /// }
        /// </code>
        /// </example>
        public FilePath OutputPath => GetEnvironmentFilePath("GITHUB_OUTPUT");

        /// <summary>
        /// Gets the path to output file to set the step summary.
        /// </summary>
        /// <value>
        /// The path to output file to set the step summary.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime StepSummary: {0}", BuildSystem.GitHubActions.Environment.Runtime.StepSummary);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime StepSummary: {0}", GitHubActions.Environment.Runtime.StepSummary);
        /// }
        /// </code>
        /// </example>
        public FilePath StepSummary => GetEnvironmentFilePath("GITHUB_STEP_SUMMARY");

        /// <summary>
        /// Gets the path to path file to add a path to system path that the following steps in a job can use.
        /// </summary>
        /// <value>
        /// The path to path file to add a path to system path that the following steps in a job can use.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime SystemPath: {0}", BuildSystem.GitHubActions.Environment.Runtime.SystemPath);
        /// }
        /// </code>
        /// </example>
        /// <para>Via GitHubActions.</para>
        /// <example>
        /// <code>
        /// if (GitHubActions.IsRunningOnGitHubActions)
        /// {
        ///     Information(@"Runtime SystemPath: {0}", GitHubActions.Environment.Runtime.SystemPath);
        /// }
        /// </code>
        /// </example>
        public FilePath SystemPath => GetEnvironmentFilePath("GITHUB_PATH");
    }
}
