// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins environment information for the current build.
    /// </summary>
    public sealed class JenkinsEnvironmentInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets Jenkins build information.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public JenkinsBuildInfo Build { get; }

        /// <summary>
        /// Gets Jenkins repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public JenkinsRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets Jenkins job information.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public JenkinsJobInfo Job { get; }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public JenkinsNodeInfo Node { get; }

        /// <summary>
        /// Gets the change.
        /// </summary>
        /// <value>
        /// The change.
        /// </value>
        public JenkinsChangeInfo Change { get; }

        /// <summary>
        /// Gets the absolute path of the directory assigned on the master node for Jenkins to store data.
        /// </summary>
        /// <value>
        /// The absolute path of the directory assigned on the master node for Jenkins to store data.
        /// </value>
        public string JenkinsHome => GetEnvironmentString("JENKINS_HOME");

        /// <summary>
        /// Gets the full URL of Jenkins (note: only available if Jenkins URL is set in system configuration).
        /// </summary>
        /// <value>
        /// The full URL of Jenkins.
        /// </value>
        public string JenkinsUrl => GetEnvironmentString("JENKINS_URL");

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsEnvironmentInfo" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            Build = new JenkinsBuildInfo(environment);
            Repository = new JenkinsRepositoryInfo(environment);
            Node = new JenkinsNodeInfo(environment);
            Job = new JenkinsJobInfo(environment);
            Change = new JenkinsChangeInfo(environment);
        }
    }
}