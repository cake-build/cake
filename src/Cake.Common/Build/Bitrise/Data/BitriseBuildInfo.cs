// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise build information for the current build.
    /// </summary>
    public class BitriseBuildInfo : BitriseInfo
    {
        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public string BuildNumber
        {
            get { return GetEnvironmentString("BITRISE_BUILD_NUMBER"); }
        }

        /// <summary>
        /// Gets the build URL.
        /// </summary>
        /// <value>
        /// The build URL.
        /// </value>
        public string BuildUrl
        {
            get { return GetEnvironmentString("BITRISE_BUILD_URL"); }
        }

        /// <summary>
        /// Gets the build slug.
        /// </summary>
        /// <value>
        /// The build slug.
        /// </value>
        public string BuildSlug
        {
            get { return GetEnvironmentString("BITRISE_BUILD_SLUG"); }
        }

        /// <summary>
        /// Gets the build trigger timestamp.
        /// </summary>
        /// <value>
        /// The build trigger timestamp.
        /// </value>
        public string BuildTriggerTimestamp
        {
            get { return GetEnvironmentString("BITRISE_BUILD_TRIGGER_TIMESTAMP"); }
        }

        /// <summary>
        /// Gets a value indicating whether the build is passing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [build status]; otherwise, <c>false</c>.
        /// </value>
        public bool BuildStatus
        {
            get { return GetEnvironmentBoolean("BITRISE_BUILD_STATUS"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseBuildInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
