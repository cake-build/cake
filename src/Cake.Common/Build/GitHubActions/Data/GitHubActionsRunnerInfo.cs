// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// Provides GitHub Actions runner information for the current build.
    /// </summary>
    public sealed class GitHubActionsRunnerInfo : GitHubActionsInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitHubActionsRunnerInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitHubActionsRunnerInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the name of the runner executing the job.
        /// </summary>
        /// <value>
        /// The name of the runner executing the job.
        /// </value>
        public string Name => GetEnvironmentString("RUNNER_NAME");

        /// <summary>
        /// Gets the operating system of the runner executing the job.
        /// </summary>
        /// <value>
        /// The operating system of the runner executing the job.
        /// </value>
        // ReSharper disable once InconsistentNaming
        public string OS => GetEnvironmentString("RUNNER_OS");

        /// <summary>
        /// Gets the path of the temporary directory for the runner.
        /// </summary>
        /// <value>
        /// The path of the temporary directory for the runner.
        /// This directory is guaranteed to be empty at the start of each job, even on self-hosted runners.
        /// </value>
        public DirectoryPath Temp => GetEnvironmentDirectoryPath("RUNNER_TEMP");

        /// <summary>
        /// Gets the path of the directory containing some of the pre-installed tools for GitHub-hosted runners.
        /// </summary>
        /// <value>
        /// The path of the directory containing some of the pre-installed tools for GitHub-hosted runners.
        /// </value>
        public DirectoryPath ToolCache => GetEnvironmentDirectoryPath("RUNNER_TOOL_CACHE");

        /// <summary>
        /// Gets the runner workspace directory path.
        /// </summary>
        /// <value>
        /// The runner workspace directory path.
        /// </value>
        public DirectoryPath Workspace => GetEnvironmentDirectoryPath("RUNNER_WORKSPACE");
    }
}
