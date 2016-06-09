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
        private readonly BitriseApplicationInfo _applicationProvider;
        private readonly BitriseBuildInfo _buildProvider;
        private readonly BitriseProvisioningInfo _provisioningProvider;
        private readonly BitriseRepositoryInfo _repositoryProvider;
        private readonly BitriseWorkflowInfo _workflowProvider;
        private readonly BitriseDirectoryInfo _directoryProvider;

        /// <summary>
        /// Gets Bitrise application information.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        public BitriseApplicationInfo Application
        {
            get { return _applicationProvider; }
        }

        /// <summary>
        /// Gets Bitrise build information.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public BitriseBuildInfo Build
        {
            get { return _buildProvider; }
        }

        /// <summary>
        /// Gets Bitrise directory information.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        public BitriseDirectoryInfo Directory
        {
            get { return _directoryProvider; }
        }

        /// <summary>
        /// Gets Bitrise provisioning information.
        /// </summary>
        /// <value>
        /// The provisioning.
        /// </value>
        public BitriseProvisioningInfo Provisioning
        {
            get { return _provisioningProvider; }
        }

        /// <summary>
        /// Gets Bitrise repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public BitriseRepositoryInfo Repository
        {
            get { return _repositoryProvider; }
        }

        /// <summary>
        /// Gets Bitrise workflow information.
        /// </summary>
        /// <value>
        /// The workflow.
        /// </value>
        public BitriseWorkflowInfo Workflow
        {
            get { return _workflowProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            _applicationProvider = new BitriseApplicationInfo(environment);
            _buildProvider = new BitriseBuildInfo(environment);
            _provisioningProvider = new BitriseProvisioningInfo(environment);
            _repositoryProvider = new BitriseRepositoryInfo(environment);
            _workflowProvider = new BitriseWorkflowInfo(environment);
            _directoryProvider = new BitriseDirectoryInfo(environment);
        }
    }
}
