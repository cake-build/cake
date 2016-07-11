// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins job information for a current build.
    /// </summary>
    public class JenkinsJobInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets the name of the job.
        /// </summary>
        /// <value>
        /// The name of the job.
        /// </value>
        public string JobName
        {
            get { return GetEnvironmentString("JOB_NAME"); }
        }

        /// <summary>
        /// Gets the name of the job.
        /// </summary>
        /// <value>
        /// The name of the job.
        /// </value>
        public string JobUrl
        {
            get { return GetEnvironmentString("JOB_URL"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsJobInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsJobInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
