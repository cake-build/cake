// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Rwx.Data
{
    /// <summary>
    /// Provides RWX git information for the current build.
    /// </summary>
    /// <remarks>
    /// These values are populated by the RWX <c>git/clone</c> package and are
    /// only available to tasks that depend (directly or transitively) on a task
    /// that ran <c>git/clone</c>. When unavailable, properties return empty strings.
    /// </remarks>
    public sealed class RwxGitInfo : RwxInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RwxGitInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RwxGitInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the URL of the git repository that was cloned.
        /// </summary>
        /// <value>
        /// The <c>repository</c> parameter provided to <c>git/clone</c>.
        /// </value>
        public string RepositoryUrl => GetEnvironmentString("RWX_GIT_REPOSITORY_URL");

        /// <summary>
        /// Gets the repository identifier extracted from the repository URL.
        /// </summary>
        /// <value>
        /// The repository identifier (for example, <c>cake-build/cake</c>).
        /// </value>
        public string RepositoryName => GetEnvironmentString("RWX_GIT_REPOSITORY_NAME");

        /// <summary>
        /// Gets the SHA of the resolved commit.
        /// </summary>
        /// <value>
        /// The resolved commit SHA.
        /// </value>
        public string CommitSha => GetEnvironmentString("RWX_GIT_COMMIT_SHA");

        /// <summary>
        /// Gets the full message of the resolved commit.
        /// </summary>
        /// <value>
        /// The resolved commit message.
        /// </value>
        public string CommitMessage => GetEnvironmentString("RWX_GIT_COMMIT_MESSAGE");

        /// <summary>
        /// Gets the summary line of the resolved commit's message.
        /// </summary>
        /// <value>
        /// The first line of the resolved commit message.
        /// </value>
        public string CommitSummary => GetEnvironmentString("RWX_GIT_COMMIT_SUMMARY");

        /// <summary>
        /// Gets the committer name associated with the resolved commit.
        /// </summary>
        /// <value>
        /// The committer name.
        /// </value>
        public string CommitterName => GetEnvironmentString("RWX_GIT_COMMITTER_NAME");

        /// <summary>
        /// Gets the committer email associated with the resolved commit.
        /// </summary>
        /// <value>
        /// The committer email.
        /// </value>
        public string CommitterEmail => GetEnvironmentString("RWX_GIT_COMMITTER_EMAIL");

        /// <summary>
        /// Gets the unresolved ref associated with the commit.
        /// </summary>
        /// <value>
        /// The unresolved ref (for example, <c>refs/heads/main</c> or <c>refs/tags/v1.0.0</c>).
        /// </value>
        public string Ref => GetEnvironmentString("RWX_GIT_REF");

        /// <summary>
        /// Gets the name of the unresolved ref associated with the commit.
        /// </summary>
        /// <value>
        /// The short ref name (for example, the branch or tag name extracted from the full ref path).
        /// </value>
        public string RefName => GetEnvironmentString("RWX_GIT_REF_NAME");
    }
}
