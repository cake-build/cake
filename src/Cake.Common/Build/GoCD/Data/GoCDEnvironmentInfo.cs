// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// Provides Go.CD environment information for a current build.
    /// </summary>
    public sealed class GoCDEnvironmentInfo : GoCDInfo
    {
        /// <summary>
        /// Gets GoCD pipeline information.
        /// </summary>
        /// <value>
        /// The GoCD pipeline information.
        /// </value>
        public GoCDPipelineInfo Pipeline { get; }

        /// <summary>
        /// Gets GoCD stage information.
        /// </summary>
        /// <value>
        ///   The GoCD stage information.
        /// </value>
        public GoCDStageInfo Stage { get; }

        /// <summary>
        /// Gets GoCD repository information.
        /// </summary>
        /// <value>
        ///   The GoCD repository information.
        /// </value>
        public GoCDRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets the Go.CD URL.
        /// </summary>
        /// <value>
        /// The Go.CD URL.
        /// </value>
        public string GoCDUrl => GetEnvironmentString("GO_SERVER_URL");

        /// <summary>
        /// Gets the environment name. This is only set if the environment is specified. Otherwise the variable is not set.
        /// </summary>
        /// <value>
        /// The environment name.
        /// </value>
        public string EnvironmentName => GetEnvironmentString("GO_ENVIRONMENT_NAME");

        /// <summary>
        /// Gets the name of the current job being run.
        /// </summary>
        /// <value>
        /// The job name.
        /// </value>
        public string JobName => GetEnvironmentString("GO_JOB_NAME");

        /// <summary>
        /// Gets the username of the user that triggered the build. This will have one of three possible values:
        /// anonymous - if there is no security.
        /// username of the user, who triggered the build.
        /// changes, if SCM changes auto-triggered the build.
        /// timer, if the pipeline is triggered at a scheduled time.
        /// </summary>
        /// <value>
        /// The username of the user that triggered the build.
        /// </value>
        public string User => GetEnvironmentString("GO_TRIGGER_USER");

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GoCDEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Pipeline = new GoCDPipelineInfo(environment);
            Stage = new GoCDStageInfo(environment);
            Repository = new GoCDRepositoryInfo(environment);
        }
    }
}