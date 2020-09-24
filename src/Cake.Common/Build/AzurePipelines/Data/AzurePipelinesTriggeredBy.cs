// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines Trigger information for the current build.
    /// </summary>
    /// <remarks>
    /// Only populated if the build was triggered by another build.
    /// </remarks>
    public class AzurePipelinesTriggeredBy : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesTriggeredBy"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesTriggeredBy(ICakeEnvironment environment) : base(environment)
        {
        }

        /// <summary>
        /// Gets the BuildID of the triggering build.
        /// </summary>
        public int BuildId => GetEnvironmentInteger("BUILD_TRIGGEREDBY_BUILDID");

        /// <summary>
        /// Gets the DefinitionID of the triggering build.
        /// </summary>
        public int DefinitionId => GetEnvironmentInteger("BUILD_TRIGGEREDBY_DEFINITIONID");

        /// <summary>
        /// Gets the name of the triggering build pipeline.
        /// </summary>
        public string DefinitionName => GetEnvironmentString("BUILD_TRIGGEREDBY_DEFINITIONNAME");

        /// <summary>
        /// Gets the number of the triggering build.
        /// </summary>
        public string BuildNumber => GetEnvironmentString("BUILD_TRIGGEREDBY_BUILDNUMBER");

        /// <summary>
        /// Gets the ID of the project that contains the triggering build.
        /// </summary>
        public string ProjectId => GetEnvironmentString("BUILD_TRIGGEREDBY_PROJECTID");
    }
}