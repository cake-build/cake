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
        bool IsRunningOnGitHubActions { get; }

        /// <summary>
        /// Gets the GitHub Actions environment.
        /// </summary>
        /// <value>
        /// The GitHub Actions environment.
        /// </value>
        GitHubActionsEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the GitHub Actions commands.
        /// </summary>
        /// <value>
        /// The GitHub Actions commands.
        /// </value>
        public GitHubActionsCommands Commands { get; }
    }
}
