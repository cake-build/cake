// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TFBuild.Data
{
    /// <summary>
    /// Provides TF Build Environment information for the current build.
    /// </summary>
    public sealed class TFBuildEnvironmentInfo : TFInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TFBuildEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TFBuildEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Repository = new TFBuildRepositoryInfo(environment);
            BuildDefinition = new TFBuildDefinitionInfo(environment);
            Build = new TFBuildInfo(environment);
            Agent = new TFBuildAgentInfo(environment);
            TeamProject = new TFBuildTeamProjectInfo(environment);
        }

        /// <summary>
        /// Gets TF Build repository information
        /// </summary>
        /// <value>
        /// The TF Build repository information.
        /// </value>
        public TFBuildRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets TF Build Definition information.
        /// </summary>
        /// <value>
        /// The TF Build Definition.
        /// </value>
        public TFBuildDefinitionInfo BuildDefinition { get; }

        /// <summary>
        /// Gets TF Build information.
        /// </summary>
        /// <value>
        /// The TF Build.
        /// </value>
        public TFBuildInfo Build { get; }

        /// <summary>
        /// Gets TF Team Project information.
        /// </summary>
        /// <value>
        /// The TF Team Project.
        /// </value>
        public TFBuildTeamProjectInfo TeamProject { get; }

        /// <summary>
        /// Gets TF Build agent information.
        /// </summary>
        /// <value>
        /// The TF Build agent.
        /// </value>
        public TFBuildAgentInfo Agent { get; }
    }
}
