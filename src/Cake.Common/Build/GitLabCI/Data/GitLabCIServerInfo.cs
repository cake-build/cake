// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitLabCI.Data
{
    /// <summary>
    /// Provides GitLab CI server information for a current build.
    /// </summary>
    public sealed class GitLabCIServerInfo : GitLabCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIServerInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIServerInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the name of CI server that is used to coordinate builds.
        /// </summary>
        /// <value>
        /// The name of CI server that is used to coordinate builds.
        /// </value>
        public string Name => GetEnvironmentString("CI_SERVER_NAME");

        /// <summary>
        /// Gets the GitLab version that is used to schedule builds.
        /// </summary>
        /// <value>
        /// The GitLab version that is used to schedule builds.
        /// </value>
        public string Version => GetEnvironmentString("CI_SERVER_VERSION");

        /// <summary>
        /// Gets the GitLab revision that is used to schedule builds.
        /// </summary>
        /// <value>
        /// The GitLab revision that is used to schedule builds.
        /// </value>
        public string Revision => GetEnvironmentString("CI_SERVER_REVISION");
    }
}
