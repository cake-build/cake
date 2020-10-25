// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.GitLabCI.Data;
using Cake.Core;

namespace Cake.Common.Build.GitLabCI
{
    /// <summary>
    /// Responsible for communicating with GitLab CI.
    /// </summary>
    public class GitLabCIProvider : IGitLabCIProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIProvider(ICakeEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Environment = new GitLabCIEnvironmentInfo(environment);
        }

        /// <inheritdoc/>
        public bool IsRunningOnGitLabCI => _environment.GetEnvironmentVariable("CI_SERVER")?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? false;

        /// <inheritdoc/>
        public GitLabCIEnvironmentInfo Environment { get; }
    }
}
