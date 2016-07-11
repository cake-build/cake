// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo environment information for a current build.
    /// </summary>
    public sealed class BambooEnvironmentInfo : BambooInfo
    {
        private readonly BambooPlanInfo _planProvider;
        private readonly BambooBuildInfo _buildProvider;
        private readonly BambooRepositoryInfo _repositoryProvider;

        /// <summary>
        /// Gets Bamboo plan information.
        /// </summary>
        /// <value>
        ///   The Bamboo plan information.
        /// </value>
        public BambooPlanInfo Plan
        {
            get { return _planProvider; }
        }

        /// <summary>
        /// Gets Bamboo build information.
        /// </summary>
        /// <value>
        ///   The Bamboo build information.
        /// </value>
        public BambooBuildInfo Build
        {
            get { return _buildProvider; }
        }

        /// <summary>
        /// Gets Bamboo repository information.
        /// </summary>
        /// <value>
        ///   The Bamboo repository information.
        /// </value>
        public BambooRepositoryInfo Repository
        {
            get { return _repositoryProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _planProvider = new BambooPlanInfo(environment);
            _buildProvider = new BambooBuildInfo(environment);
            _repositoryProvider = new BambooRepositoryInfo(environment);
        }
    }
}
