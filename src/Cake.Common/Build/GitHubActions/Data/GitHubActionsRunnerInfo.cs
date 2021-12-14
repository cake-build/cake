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

        /// <summary>
        /// Gets the runner image OS on hosted agents.
        /// </summary>
        /// <value>
        /// The runner image OS on hosted agents.
        /// </value>
        public string ImageOS => GetEnvironmentString("ImageOS");

        /// <summary>
        /// Gets the runner image version on hosted agents.
        /// </summary>
        /// <value>
        /// The runner image version on hosted agents.
        /// </value>
        public string ImageVersion => GetEnvironmentString("ImageVersion");

        /// <summary>
        /// Gets the runner user name.
        /// </summary>
        /// <value>
        /// The runner user name.
        /// </value>
        public string User => GetEnvironmentString("RUNNER_USER");

        /// <summary>
        /// Gets the runner architecture of the runner executing the job.
        /// </summary>
        /// <value>
        /// The runner architecture of the runner executing the job. Possible values are X86, X64, ARM, and ARM64.
        /// </value>
        public GitHubActionsArchitecture Architecture => GetEnvironmentString("RUNNER_ARCH")
                                                            ?.ToUpperInvariant() switch
                                                            {
                                                                "X86" => GitHubActionsArchitecture.X86,
                                                                "X64" => GitHubActionsArchitecture.X64,
                                                                "ARM" => GitHubActionsArchitecture.ARM,
                                                                "ARM64" => GitHubActionsArchitecture.ARM64,
                                                                _ => GitHubActionsArchitecture.Unknown
                                                            };
    }
}
