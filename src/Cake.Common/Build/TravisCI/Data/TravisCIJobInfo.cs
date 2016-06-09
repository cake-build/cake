// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.TravisCI.Data
{
    /// <summary>
    /// Provides Travis CI job information for the current build.
    /// </summary>
    public sealed class TravisCIJobInfo : TravisCIInfo
    {
        /// <summary>
        /// Gets the job identifier for the current job..
        /// </summary>
        /// <value>
        /// The job identifier.
        /// </value>
        public string JobId
        {
            get { return GetEnvironmentString("TRAVIS_JOB_ID"); }
        }

        /// <summary>
        /// Gets the job number for the current job.
        /// </summary>
        /// <value>
        /// The job number.
        /// </value>
        public string JobNumber
        {
            get { return GetEnvironmentString("TRAVIS_JOB_NUMBER"); }
        }

        /// <summary>
        /// Gets the name of the operating system for the current job.
        /// </summary>
        /// <value>
        /// The name of the os.
        /// </value>
        public string OSName
        {
            get { return GetEnvironmentString("TRAVIS_OS_NAME"); }
        }

        /// <summary>
        /// Gets a value indicating whether encrypted environment variables are being used for the current job.
        /// </summary>
        /// <value>
        /// <c>true</c> if [secure environment variables are in use]; otherwise, <c>false</c>.
        /// </value>
        public bool SecureEnvironmentVariables
        {
            get { return GetEnvironmentBoolean("TRAVIS_SECURE_ENV_VARS"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravisCIJobInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TravisCIJobInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
