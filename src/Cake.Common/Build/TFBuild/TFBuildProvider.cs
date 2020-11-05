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

        /// <inheritdoc/>
        public bool IsRunningOnAzurePipelines
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && !IsHostedAgent;

        /// <inheritdoc/>
        public bool IsRunningOnAzurePipelinesHosted
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && IsHostedAgent;

        /// <inheritdoc/>
        public TFBuildEnvironmentInfo Environment { get; }

        /// <inheritdoc/>
        public ITFBuildCommands Commands { get; }

        private bool IsHostedAgent => Environment.Agent.IsHosted;
    }
}
