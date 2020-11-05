// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines definition information for the current build.
    /// </summary>
    public sealed class AzurePipelinesDefinitionInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesDefinitionInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesDefinitionInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the build definition ID.
        /// </summary>
        /// <value>
        /// The build definition ID.
        /// </value>
        public int Id => GetEnvironmentInteger("SYSTEM_DEFINITIONID");

        /// <summary>
        /// Gets the build definition name.
        /// </summary>
        /// <value>
        /// The build definition name.
        /// </value>
        public string Name => GetEnvironmentString("BUILD_DEFINITIONNAME");

        /// <summary>
        /// Gets the build definition version.
        /// </summary>
        /// <value>
        /// The build definition version.
        /// </value>
        public int Version => GetEnvironmentInteger("BUILD_DEFINITIONVERSION");
    }
}
