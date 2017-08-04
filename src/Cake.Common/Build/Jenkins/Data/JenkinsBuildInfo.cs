// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins build information for the current build.
    /// </summary>
    public sealed class JenkinsBuildInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public int BuildNumber => GetEnvironmentInteger("BUILD_NUMBER");

        /// <summary>
        /// Gets the build identifier which is identical to <see cref="BuildNumber"/> starting from Jenkins 1.597 and a YYYY-MM-DD_hh-mm-ss timestamp for older builds.
        /// </summary>
        /// <value>
        /// The build identifier.
        /// </value>
        public string BuildId => GetEnvironmentString("BUILD_ID");

        /// <summary>
        /// Gets the display name of the build.
        /// </summary>
        /// <value>
        /// The display name of the build.
        /// </value>
        public string BuildDisplayName => GetEnvironmentString("BUILD_DISPLAY_NAME");

        /// <summary>
        /// Gets the build tag which is a string of "jenkins-${JOB_NAME}-${BUILD_NUMBER}". All forward slashes (/) in the JOB_NAME are replaced with dashes (-).
        /// </summary>
        /// <value>
        /// The build tag.
        /// </value>
        public string BuildTag => GetEnvironmentString("BUILD_TAG");

        /// <summary>
        /// Gets the build URL.
        /// </summary>
        /// <value>
        /// The build URL.
        /// </value>
        public string BuildUrl => GetEnvironmentString("BUILD_URL");

        /// <summary>
        /// Gets the executor number.
        /// </summary>
        /// <value>
        /// The executor number.
        /// </value>
        public int ExecutorNumber => GetEnvironmentInteger("EXECUTOR_NUMBER");

        /// <summary>
        /// Gets the absolute path of the workspace directory assigned to the build.
        /// </summary>
        /// <value>
        /// The workspace directory path.
        /// </value>
        public string Workspace => GetEnvironmentString("WORKSPACE");

        /// <summary>
        ///  Initializes a new instance of the <see cref="JenkinsBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsBuildInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}