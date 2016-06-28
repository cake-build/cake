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
        /// <summary>
        /// Gets the AppVeyor build agent API URL.
        /// </summary>
        /// <value>
        ///   The AppVeyor build agent API URL.
        /// </value>
        public string ApiUrl => GetEnvironmentString("APPVEYOR_API_URL");

        /// <summary>
        /// Gets the AppVeyor unique job ID.
        /// </summary>
        /// <value>
        ///   The AppVeyor unique job ID.
        /// </value>
        public string JobId => GetEnvironmentString("APPVEYOR_JOB_ID");

        /// <summary>
        /// Gets the AppVeyor Job Name.
        /// </summary>
        /// <value>
        ///   The AppVeyor Job Name.
        /// </value>
        public string JobName => GetEnvironmentString("APPVEYOR_JOB_NAME");

        /// <summary>
        /// Gets a value indicating whether the build runs by scheduler.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the build runs by scheduler; otherwise, <c>false</c>.
        /// </value>
        public bool ScheduledBuild => GetEnvironmentBoolean("APPVEYOR_SCHEDULED_BUILD");

        /// <summary>
        /// Gets the platform name set on build tab of project settings (or through platform parameter in appveyor.yml).
        /// </summary>
        /// <value>
        ///   The platform name set on build tab of project settings (or through platform parameter in appveyor.yml).
        /// </value>
        public string Platform => GetEnvironmentString("PLATFORM");

        /// <summary>
        /// Gets the configuration name set on build tab of project settings (or through configuration parameter in appveyor.yml).
        /// </summary>
        /// <value>
        ///   The configuration name set on build tab of project settings (or through configuration parameter in appveyor.yml).
        /// </value>
        public string Configuration => GetEnvironmentString("CONFIGURATION");

        /// <summary>
        /// Gets AppVeyor project information.
        /// </summary>
        /// <value>
        ///   The AppVeyor project information.
        /// </value>
        public AppVeyorProjectInfo Project { get; }

        /// <summary>
        /// Gets AppVeyor build information.
        /// </summary>
        /// <value>
        ///   The AppVeyor build information.
        /// </value>
        public AppVeyorBuildInfo Build { get; }

        /// <summary>
        /// Gets AppVeyor pull request information.
        /// </summary>
        /// <value>
        ///   The AppVeyor pull request information.
        /// </value>
        public AppVeyorPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets AppVeyor repository information.
        /// </summary>
        /// <value>
        ///   The AppVeyor repository information.
        /// </value>
        public AppVeyorRepositoryInfo Repository { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Project = new AppVeyorProjectInfo(environment);
            Build = new AppVeyorBuildInfo(environment);
            PullRequest = new AppVeyorPullRequestInfo(environment);
            Repository = new AppVeyorRepositoryInfo(environment);
        }
    }
}