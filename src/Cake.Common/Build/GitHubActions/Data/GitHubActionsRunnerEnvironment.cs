// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Build.GitHubActions.Data
{
    /// <summary>
    /// The GitHub Actions runner environment.
    /// </summary>
    public enum GitHubActionsRunnerEnvironment
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// GitHub-hosted runner provided by GitHub.
        /// </summary>
        GitHubHosted,

        /// <summary>
        /// Self-hosted runner configured by the repository owner.
        /// </summary>
        SelfHosted
    }
}
