using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            _environment = environment;
            Environment = new GitLabCIEnvironmentInfo(environment);
        }

        /// <summary>
        /// Gets the GitLab CI environment.
        /// </summary>
        /// <value>
        /// The GitLab CI environment.
        /// </value>
        public GitLabCIEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on GitLab CI.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on GitLab CI; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnGitLabCI => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("GITLAB_CI"));
    }
}
