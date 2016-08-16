// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise environment information for the current build.
    /// </summary>
    public class BitriseEnvironmentInfo : BitriseInfo
    {
        /// <summary>
        /// Gets Bitrise application information.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public BitriseApplicationInfo Application { get; }

        /// <summary>
        /// Gets Bitrise build information.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public BitriseBuildInfo Build { get; }

        /// <summary>
        /// Gets Bitrise directory information.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public BitriseDirectoryInfo Directory { get; }

        /// <summary>
        /// Gets Bitrise provisioning information.
        /// </summary>
        /// <value>
        /// The provisioning.
        /// </value>
        public BitriseProvisioningInfo Provisioning { get; }

        /// <summary>
        /// Gets Bitrise repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public BitriseRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets Bitrise workflow information.
        /// </summary>
        /// <value>
        /// The workflow.
        /// </value>
        public BitriseWorkflowInfo Workflow { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            Application = new BitriseApplicationInfo(environment);
            Build = new BitriseBuildInfo(environment);
            Provisioning = new BitriseProvisioningInfo(environment);
            Repository = new BitriseRepositoryInfo(environment);
            Workflow = new BitriseWorkflowInfo(environment);
            Directory = new BitriseDirectoryInfo(environment);
        }
    }
}