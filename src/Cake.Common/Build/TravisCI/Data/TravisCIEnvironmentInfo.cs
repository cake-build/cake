// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.TravisCI.Data
{
    /// <summary>
    /// Provides Travis CI environment information for the current build.
    /// </summary>
    public sealed class TravisCIEnvironmentInfo : TravisCIInfo
    {
        private readonly TravisCIBuildInfo _buildProvider;
        private readonly TravisCIJobInfo _jobProvider;
        private readonly TravisCIRepositoryInfo _repositoryProvider;

        /// <summary>
        /// Gets Travis CI build information for the current build.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        public TravisCIBuildInfo Build
        {
            get { return _buildProvider; }
        }

        /// <summary>
        /// Gets Travis CI job information for the current build.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        public TravisCIJobInfo Job
        {
            get { return _jobProvider; }
        }

        /// <summary>
        /// Gets Travis CI repository information for the current build.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public TravisCIRepositoryInfo Repository
        {
            get { return _repositoryProvider; }
        }

        /// <summary>
        /// Gets a value indicating whether the current build is continuous integration.
        /// </summary>
        /// <value>
        /// <c>true</c> if ci; otherwise, <c>false</c>.
        /// </value>
        public bool CI
        {
            get { return GetEnvironmentBoolean("CI"); }
        }

        /// <summary>
        /// Gets the Travis CI home directory.
        /// </summary>
        /// <value>
        /// The home.
        /// </value>
        public string Home
        {
            get { return GetEnvironmentString("HOME"); }
        }

        /// <summary>
        /// Gets a value indicating whether the environment is Travis.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Travis; otherwise, <c>false</c>.
        /// </value>
        public bool Travis
        {
            get { return GetEnvironmentBoolean("TRAVIS"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TravisCIEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TravisCIEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _buildProvider = new TravisCIBuildInfo(environment);
            _jobProvider = new TravisCIJobInfo(environment);
            _repositoryProvider = new TravisCIRepositoryInfo(environment);
        }
    }
}
