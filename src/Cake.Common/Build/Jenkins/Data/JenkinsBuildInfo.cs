// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins build information for a current build.
    /// </summary>
    public sealed class JenkinsBuildInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public int BuildNumber
        {
            get { return GetEnvironmentInteger("BUILD_NUMBER"); }
        }

        /// <summary>
        /// Gets the build identifier.
        /// </summary>
        /// <value>
        /// The build identifier.
        /// </value>
        public string BuildId
        {
            get { return GetEnvironmentString("BUILD_ID"); }
        }

        /// <summary>
        /// Gets the display name of the build.
        /// </summary>
        /// <value>
        /// The display name of the build.
        /// </value>
        public string BuildDisplayName
        {
            get { return GetEnvironmentString("BUILD_DISPLAY_NAME"); }
        }

        /// <summary>
        /// Gets the build URL.
        /// </summary>
        /// <value>
        /// The build URL.
        /// </value>
        public string BuildTag
        {
            get { return GetEnvironmentString("BUILD_TAG"); }
        }

        /// <summary>
        /// Gets the build URL.
        /// </summary>
        /// <value>
        /// The build URL.
        /// </value>
        public string BuildUrl
        {
            get { return GetEnvironmentString("BUILD_URL"); }
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
        ///  Initializes a new instance of the <see cref="JenkinsBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsBuildInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
