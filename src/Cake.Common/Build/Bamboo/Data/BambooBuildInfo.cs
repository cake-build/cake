// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo build information for a current build.
    /// </summary>
    public sealed class BambooBuildInfo : BambooInfo
    {
        private readonly BambooCustomBuildInfo _customBuildProvider;

        /// <summary>
        /// Gets the path to the clone directory.
        /// </summary>
        /// <value>
        /// The path to the clone directory.
        /// </value>
        public string Folder
        {
            get { return GetEnvironmentString("bamboo_build_working_directory"); }
        }

        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public int Number
        {
            get { return GetEnvironmentInteger("bamboo_buildNumber"); }
        }

        /// <summary>
        /// Gets the job key for the current job, in the form PROJECT-PLAN-JOB, e.g. BAM-MAIN-JOBX
        /// </summary>
        /// <value>
        ///   The Bamboo Build Key.
        /// </value>
        public string BuildKey
        {
            get { return GetEnvironmentString("bamboo_buildKey"); }
        }

        /// <summary>
        /// Gets the Bamboo Build Result Key.
        /// The result key when this job executes, in the form PROJECT-PLAN-JOB-BUILD e.g. BAM-BOO-JOB1-8, where '8' is the build number.
        /// For deployment projects this variable will not have the JOB component e.g. PROJ-TP-6.
        /// </summary>
        /// <value>
        ///   The Build Result Key.
        /// </value>
        public string ResultKey
        {
            get { return GetEnvironmentString("bamboo_buildResultKey"); }
        }

        /// <summary>
        /// Gets the URL of the result in Bamboo once the job has finished executing.
        /// </summary>
        /// <value>
        ///   The Bamboo build result url.
        /// </value>
        public string ResultsUrl
        {
            get { return GetEnvironmentString("bamboo_buildResultsUrl"); }
        }

        /// <summary>
        /// Gets the time when build was started in ISO 8601 format e.g. 2010-01-01T01:00:00.000+01:00.
        /// </summary>
        /// <value>
        ///   The Bamboo build timestamp.
        /// </value>
        public string BuildTimestamp
        {
            get { return GetEnvironmentString("bamboo_buildTimeStamp"); }
        }

        /// <summary>
        /// Gets Bamboo custom build information.
        /// </summary>
        /// <value>
        ///   The Bamboo custom build information.
        /// </value>
        public BambooCustomBuildInfo CustomBuild
        {
            get { return _customBuildProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _customBuildProvider = new BambooCustomBuildInfo(environment);
        }
    }
}
