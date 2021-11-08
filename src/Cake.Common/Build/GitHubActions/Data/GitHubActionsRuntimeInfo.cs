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
        public bool IsRuntimeAvailable
            => !string.IsNullOrWhiteSpace(Token) && !string.IsNullOrWhiteSpace(Url);

        /// <summary>
        /// Gets the current runtime API authorization token.
        /// </summary>
        /// <value>
        /// The current runtime API authorization token.
        /// </value>
        public string Token => GetEnvironmentString("ACTIONS_RUNTIME_TOKEN");

        /// <summary>
        /// Gets the current runtime API endpoint url.
        /// </summary>
        /// <value>
        /// The current runtime API endpoint url.
        /// </value>
        public string Url => GetEnvironmentString("ACTIONS_RUNTIME_URL");

        /// <summary>
        /// Gets the path to environment file to set an environment variable that the following steps in a job can use.
        /// </summary>
        /// <value>
        /// The path to environment file to set an environment variable that the following steps in a job can use.
        /// </value>
        public FilePath EnvPath => GetEnvironmentFilePath("GITHUB_ENV");

        /// <summary>
        /// Gets the path to path file to add a path to system path that the following steps in a job can use.
        /// </summary>
        /// <value>
        /// The path to path file to add a path to system path that the following steps in a job can use.
        /// </value>
        public FilePath SystemPath => GetEnvironmentFilePath("GITHUB_PATH");
    }
}