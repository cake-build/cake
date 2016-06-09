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
        /// Gets the git commit sha.
        /// </summary>
        /// <value>
        /// The git commit sha.
        /// </value>
        public string GitCommitSha
        {
            get { return GetEnvironmentString("GIT_COMMIT"); }
        }

        /// <summary>
        /// Gets the git branch.
        /// </summary>
        /// <value>
        /// The git branch.
        /// </value>
        public string GitBranch
        {
            get { return GetEnvironmentString("GIT_BRANCH"); }
        }

        /// <summary>
        /// Gets the SVN revision.
        /// </summary>
        /// <value>
        /// The SVN revision.
        /// </value>
        public string SvnRevision
        {
            get { return GetEnvironmentString("SVN_REVISION"); }
        }

        /// <summary>
        /// Gets the CVS branch.
        /// </summary>
        /// <value>
        /// The CVS branch.
        /// </value>
        public string CvsBranch
        {
            get { return GetEnvironmentString("CVS_BRANCH"); }
        }

        /// <summary>
        /// Gets the SVN URL.
        /// </summary>
        /// <value>
        /// The SVN URL.
        /// </value>
        public string SvnUrl
        {
            get
            {
                return GetEnvironmentString("SVN_URL");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsRepositoryInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
