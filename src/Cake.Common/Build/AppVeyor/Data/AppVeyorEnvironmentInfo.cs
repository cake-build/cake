// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor environment information for a current build.
    /// </summary>
    public sealed class AppVeyorEnvironmentInfo : AppVeyorInfo
    {
        private readonly AppVeyorProjectInfo _projectProvider;
        private readonly AppVeyorBuildInfo _buildProvider;
        private readonly AppVeyorPullRequestInfo _pullRequestProvider;
        private readonly AppVeyorRepositoryInfo _repositoryProvider;

        /// <summary>
        /// Gets the AppVeyor build agent API URL.
        /// </summary>
        /// <value>
        ///   The AppVeyor build agent API URL.
        /// </value>
        public string ApiUrl
        {
            get { return GetEnvironmentString("APPVEYOR_API_URL"); }
        }

        /// <summary>
        /// Gets the AppVeyor unique job ID.
        /// </summary>
        /// <value>
        ///   The AppVeyor unique job ID.
        /// </value>
        public string JobId
        {
            get { return GetEnvironmentString("APPVEYOR_JOB_ID"); }
        }

        /// <summary>
        /// Gets the AppVeyor Job Name.
        /// </summary>
        /// <value>
        ///   The AppVeyor Job Name.
        /// </value>
        public string JobName
        {
            get { return GetEnvironmentString("APPVEYOR_JOB_NAME"); }
        }

        /// <summary>
        /// Gets a value indicating whether the build runs by scheduler.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the build runs by scheduler; otherwise, <c>false</c>.
        /// </value>
        public bool ScheduledBuild
        {
            get { return GetEnvironmentBoolean("APPVEYOR_SCHEDULED_BUILD"); }
        }

        /// <summary>
        /// Gets the platform name set on build tab of project settings (or through platform parameter in appveyor.yml).
        /// </summary>
        /// <value>
        ///   The platform name set on build tab of project settings (or through platform parameter in appveyor.yml).
        /// </value>
        public string Platform
        {
            get { return GetEnvironmentString("PLATFORM"); }
        }

        /// <summary>
        /// Gets the configuration name set on build tab of project settings (or through configuration parameter in appveyor.yml).
        /// </summary>
        /// <value>
        ///   The configuration name set on build tab of project settings (or through configuration parameter in appveyor.yml).
        /// </value>
        public string Configuration
        {
            get { return GetEnvironmentString("CONFIGURATION"); }
        }

        /// <summary>
        /// Gets AppVeyor project information.
        /// </summary>
        /// <value>
        ///   The AppVeyor project information.
        /// </value>
        public AppVeyorProjectInfo Project
        {
            get { return _projectProvider; }
        }

        /// <summary>
        /// Gets AppVeyor build information.
        /// </summary>
        /// <value>
        ///   The AppVeyor build information.
        /// </value>
        public AppVeyorBuildInfo Build
        {
            get { return _buildProvider; }
        }

        /// <summary>
        /// Gets AppVeyor pull request information.
        /// </summary>
        /// <value>
        ///   The AppVeyor pull request information.
        /// </value>
        public AppVeyorPullRequestInfo PullRequest
        {
            get { return _pullRequestProvider; }
        }

        /// <summary>
        /// Gets AppVeyor repository information.
        /// </summary>
        /// <value>
        ///   The AppVeyor repository information.
        /// </value>
        public AppVeyorRepositoryInfo Repository
        {
            get { return _repositoryProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _projectProvider = new AppVeyorProjectInfo(environment);
            _buildProvider = new AppVeyorBuildInfo(environment);
            _pullRequestProvider = new AppVeyorPullRequestInfo(environment);
            _repositoryProvider = new AppVeyorRepositoryInfo(environment);
        }
    }
}
