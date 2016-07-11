// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor build information for a current build.
    /// </summary>
    public sealed class AppVeyorBuildInfo : AppVeyorInfo
    {
        /// <summary>
        /// Gets the path to the clone directory.
        /// </summary>
        /// <value>
        /// The path to the clone directory.
        /// </value>
        public string Folder
        {
            get { return GetEnvironmentString("APPVEYOR_BUILD_FOLDER"); }
        }

        /// <summary>
        /// Gets the AppVeyor unique build ID.
        /// </summary>
        /// <value>
        /// The AppVeyor unique build ID.
        /// </value>
        public string Id
        {
            get { return GetEnvironmentString("APPVEYOR_BUILD_ID"); }
        }

        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public int Number
        {
            get { return GetEnvironmentInteger("APPVEYOR_BUILD_NUMBER"); }
        }

        /// <summary>
        /// Gets the build version.
        /// </summary>
        /// <value>
        /// The build version.
        /// </value>
        public string Version
        {
            get { return GetEnvironmentString("APPVEYOR_BUILD_VERSION"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
