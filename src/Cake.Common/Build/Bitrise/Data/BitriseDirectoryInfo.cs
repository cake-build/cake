// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise directory information for the current build.
    /// </summary>
    public class BitriseDirectoryInfo : BitriseInfo
    {
        /// <summary>
        /// Gets the source directory.
        /// </summary>
        /// <value>
        /// The source directory.
        /// </value>
        public string SourceDirectory
        {
            get { return GetEnvironmentString("BITRISE_SOURCE_DIR"); }
        }

        /// <summary>
        /// Gets the deploy directory.
        /// </summary>
        /// <value>
        /// The deploy directory.
        /// </value>
        public string DeployDirectory
        {
            get { return GetEnvironmentString("BITRISE_DEPLOY_DIR"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseDirectoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseDirectoryInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
