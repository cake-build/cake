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
        private readonly JenkinsBuildInfo _buildProvider;
        private readonly JenkinsRepositoryInfo _repositoryProvider;
        private readonly JenkinsNodeInfo _nodeProvider;
        private readonly JenkinsJobInfo _jobProvider;

        /// <summary>
        /// Gets Jenkins build information.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public JenkinsBuildInfo Build
        {
            get { return _buildProvider; }
        }

        /// <summary>
        /// Gets Jenkins repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public JenkinsRepositoryInfo Repository
        {
            get { return _repositoryProvider; }
        }

        /// <summary>
        /// Gets Jenkins job information.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public JenkinsJobInfo Job
        {
            get { return _jobProvider; }
        }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        public JenkinsNodeInfo Node
        {
            get { return _nodeProvider; }
        }

        /// <summary>
        /// Gets the jenkins home.
        /// </summary>
        /// <value>
        /// The jenkins home.
        /// </value>
        public string JenkinsHome
        {
            get { return GetEnvironmentString("JENKINS_HOME"); }
        }

        /// <summary>
        /// Gets the jenkins URL.
        /// </summary>
        /// <value>
        /// The jenkins URL.
        /// </value>
        public string JenkinsUrl
        {
            get { return GetEnvironmentString("JENKINS_URL"); }
        }

        /// <summary>
        /// Gets the executor number.
        /// </summary>
        /// <value>
        /// The executor number.
        /// </value>
        public int ExecutorNumber
        {
            get { return GetEnvironmentInteger("EXECUTOR_NUMBER"); }
        }

        /// <summary>
        /// Gets the workspace.
        /// </summary>
        /// <value>
        /// The workspace.
        /// </value>
        public string Workspace
        {
            get { return GetEnvironmentString("WORKSPACE"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsEnvironmentInfo" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            _buildProvider = new JenkinsBuildInfo(environment);
            _repositoryProvider = new JenkinsRepositoryInfo(environment);
            _nodeProvider = new JenkinsNodeInfo(environment);
            _jobProvider = new JenkinsJobInfo(environment);
        }
    }
}
