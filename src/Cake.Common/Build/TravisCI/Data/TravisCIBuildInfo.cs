// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.TravisCI.Data
{
    /// <summary>
    /// Provides Travis CI build information for the current build.
    /// </summary>
    public sealed class TravisCIBuildInfo : TravisCIInfo
    {
        /// <summary>
        /// Gets the branch for the current build.
        /// </summary>
        /// <value>
        /// The branch.
        /// </value>
        public string Branch
        {
            get { return GetEnvironmentString("TRAVIS_BRANCH"); }
        }

        /// <summary>
        /// Gets the build directory for the current build.
        /// </summary>
        /// <value>
        /// The build directory.
        /// </value>
        public string BuildDirectory
        {
            get { return GetEnvironmentString("TRAVIS_BUILD_DIR"); }
        }

        /// <summary>
        /// Gets the build identifier for the current build.
        /// </summary>
        /// <value>
        /// The build identifier.
        /// </value>
        public string BuildId
        {
            get { return GetEnvironmentString("TRAVIS_BUILD_ID"); }
        }

        /// <summary>
        /// Gets the build number for the current build.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public int BuildNumber
        {
            get { return GetEnvironmentInteger("TRAVIS_BUILD_NUMBER"); }
        }

        /// <summary>
        /// Gets the test result indicating if the current build is successful or broken.
        /// </summary>
        /// <value>
        /// The test result.
        /// </value>
        public int TestResult
        {
            get { return GetEnvironmentInteger("TRAVIS_TEST_RESULT"); }
        }

        /// <summary>
        /// Gets the tag name for the current build.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public string Tag
        {
            get { return GetEnvironmentString("TRAVIS_TAG"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravisCIBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TravisCIBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
