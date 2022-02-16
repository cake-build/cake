// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Common.Build.AzurePipelines
{
    /// <summary>
    /// Responsible for communicating with Azure Pipelines.
    /// </summary>
    public sealed class AzurePipelinesProvider : IAzurePipelinesProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="writer">The build system service message writer.</param>
        public AzurePipelinesProvider(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Environment = new AzurePipelinesEnvironmentInfo(environment);
            Commands = new AzurePipelinesCommands(environment, writer);
        }

        /// <inheritdoc/>
        public bool IsRunningOnAzurePipelines
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD"));

        /// <inheritdoc/>
        public AzurePipelinesEnvironmentInfo Environment { get; }

        /// <inheritdoc/>
        public IAzurePipelinesCommands Commands { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on a hosted build agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on a hosted agent; otherwise, <c>false</c>.
        /// </value>
        private bool IsHostedAgent => Environment.Agent.IsHosted;
    }
}
