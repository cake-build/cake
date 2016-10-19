// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI.Data;

namespace Cake.Common.Build.GitLabCI
{
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
        bool IsRunningOnGitLabCI { get; }

        /// <summary>
        /// Gets the GitLab CI environment.
        /// </summary>
        /// <value>
        /// The GitLab CI environment.
        /// </value>
        GitLabCIEnvironmentInfo Environment { get; }
    }
}
