// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins repository information for the current build.
    /// </summary>
    public sealed class JenkinsRepositoryInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets the branch name which will be build in a multibranch project.
        /// </summary>
        /// <value>
        /// The branch name.
        /// </value>
        public string BranchName => GetEnvironmentString("BRANCH_NAME");

        /// <summary>
        /// Gets the Git commit sha.
        /// </summary>
        /// <value>
        /// The Git commit sha.
        /// </value>
        public string GitCommitSha => GetEnvironmentString("GIT_COMMIT");

        /// <summary>
        /// Gets the previous Git commit sha.
        /// </summary>
        /// <value>
        /// The previous Git commit sha.
        /// </value>
        public string GitPreviousCommitSha => GetEnvironmentString("GIT_PREVIOUS_COMMIT");

        /// <summary>
        /// Gets the previous successfull Git commit sha.
        /// </summary>
        /// <value>
        /// The previous successfull Git commit sha.
        /// </value>
        public string GitPreviousSuccessfullCommitSha => GetEnvironmentString("GIT_PREVIOUS_SUCCESSFUL_COMMIT");

        /// <summary>
        /// Gets the Git branch.
        /// </summary>
        /// <value>
        /// The Git branch.
        /// </value>
        public string GitBranch => GetEnvironmentString("GIT_BRANCH");

        /// <summary>
        /// Gets the Git local branch.
        /// </summary>
        /// <value>
        /// The Git local branch.
        /// </value>
        public string GitLocalBranch => GetEnvironmentString("GIT_LOCAL_BRANCH");

        /// <summary>
        /// Gets the Git checkout directory.
        /// </summary>
        /// <value>
        /// The Git checkout directory.
        /// </value>
        public string GitCheckoutDirectory => GetEnvironmentString("GIT_CHECKOUT_DIR");

        /// <summary>
        /// Gets the Git remote URL.
        /// </summary>
        /// <value>
        /// The Git remote URL.
        /// </value>
        public string GitUrl => GetEnvironmentString("GIT_URL");

        /// <summary>
        /// Gets the SVN revision.
        /// </summary>
        /// <value>
        /// The SVN revision.
        /// </value>
        public string SvnRevision => GetEnvironmentString("SVN_REVISION");

        /// <summary>
        /// Gets the CVS branch.
        /// </summary>
        /// <value>
        /// The CVS branch.
        /// </value>
        public string CvsBranch => GetEnvironmentString("CVS_BRANCH");

        /// <summary>
        /// Gets the SVN URL.
        /// </summary>
        /// <value>
        /// The SVN URL.
        /// </value>
        public string SvnUrl => GetEnvironmentString("SVN_URL");

        /// <summary>
        /// Gets the full id of the Mercurial revision.
        /// </summary>
        /// <value>
        /// The full id of the Mercurial revision.
        /// </value>
        public string MercurialRevision => GetEnvironmentString("MERCURIAL_REVISION");

        /// <summary>
        /// Gets the abbreviated id of the Mercurial revision.
        /// </summary>
        /// <value>
        /// The abbreviated id of the Mercurial revision.
        /// </value>
        public string MercurialRevisionShort => GetEnvironmentString("MERCURIAL_REVISION_SHORT");

        /// <summary>
        /// Gets the Mercurial revision number.
        /// </summary>
        /// <value>
        /// The Mercurial revision number.
        /// </value>
        public string MercurialRevisionNumber => GetEnvironmentString("MERCURIAL_REVISION_NUMBER");

        /// <summary>
        /// Gets the Mercurial revision branch.
        /// </summary>
        /// <value>
        /// The Mercurial revision branch.
        /// </value>
        public string MercurialRevisionBranch => GetEnvironmentString("MERCURIAL_REVISION_BRANCH");

        /// <summary>
        /// Gets the Mercurial repository URL.
        /// </summary>
        /// <value>
        /// The Mercurial repository URL.
        /// </value>
        public string MercurialRepositoryUrl => GetEnvironmentString("MERCURIAL_REPOSITORY_URL");

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsRepositoryInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}