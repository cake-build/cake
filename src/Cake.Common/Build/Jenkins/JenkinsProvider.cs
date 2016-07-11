// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Build.Jenkins.Data;
using Cake.Core;

namespace Cake.Common.Build.Jenkins
{
    /// <summary>
    /// Responsible for communicating with Jenkins.
    /// </summary>
    public sealed class JenkinsProvider : IJenkinsProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly JenkinsEnvironmentInfo _jenkinsEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsProvider(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            _environment = environment;
            _jenkinsEnvironment = new JenkinsEnvironmentInfo(_environment);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running on jenkins.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running on jenkins; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnJenkins
        {
            get { return !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("JENKINS_URL")); }
        }

        /// <summary>
        /// Gets the Jenkins environment.
        /// </summary>
        /// <value>
        /// The Jenkins environment.
        /// </value>
        public JenkinsEnvironmentInfo Environment
        {
            get { return _jenkinsEnvironment; }
        }
    }
}
