// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.TFBuild.Data;
using Cake.Core;

namespace Cake.Common.Build.TFBuild
{
    /// <summary>
    /// Responsible for communicating with Team Foundation Build (VSTS or TFS).
    /// </summary>
    public sealed class TFBuildProvider : ITFBuildProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="TFBuildProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="writer">The build system service message writer.</param>
        public TFBuildProvider(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Environment = new TFBuildEnvironmentInfo(environment);
            Commands = new TFBuildCommands(environment, writer);
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on TFS.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TFS; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Please use TFBuildProvider.IsRunningOnAzurePipelines instead.")]
        public bool IsRunningOnTFS => IsRunningOnAzurePipelines;

        /// <summary>
        /// Gets a value indicating whether the current build is running on VSTS.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on VSTS; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Please use TFBuildProvider.IsRunningOnAzurePipelinesHosted instead.")]
        public bool IsRunningOnVSTS => IsRunningOnAzurePipelinesHosted;

        /// <summary>
        /// Gets a value indicating whether the current build is running on Azure Pipelines.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Azure Pipelines; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAzurePipelines
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && !IsHostedAgent;

        /// <summary>
        /// Gets a value indicating whether the current build is running on hosted Azure Pipelines.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on hosted Azure Pipelines; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAzurePipelinesHosted
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && IsHostedAgent;

        /// <summary>
        /// Gets the TF Build environment.
        /// </summary>
        /// <value>
        /// The TF Build environment.
        /// </value>
        public TFBuildEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the TF Build Commands provider.
        /// </summary>
        /// <value>
        /// The TF Build commands provider.
        /// </value>
        public ITFBuildCommands Commands { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on a hosted build agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on a hosted agent; otherwise, <c>false</c>.
        /// </value>
        private bool IsHostedAgent => Environment.Agent.IsHosted;
    }
}
