// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines Team Project information for the current build.
    /// </summary>
    public sealed class AzurePipelinesTeamProjectInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesTeamProjectInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesTeamProjectInfo(ICakeEnvironment environment) : base(environment)
        {
        }

        /// <summary>
        /// Gets the name of the team project that contains this build.
        /// </summary>
        /// <value>
        /// The name of the team project that contains this build.
        /// </value>
        public string Name => GetEnvironmentString("SYSTEM_TEAMPROJECT");

        /// <summary>
        /// Gets the ID of the team project that contains this build.
        /// </summary>
        /// <value>
        /// The ID of the team project that contains this build.
        /// </value>
        public string Id => GetEnvironmentString("SYSTEM_TEAMPROJECTID");

        /// <summary>
        /// Gets the URI of the team foundation collection.
        /// </summary>
        /// <value>
        /// The URI of the team foundation collection.
        /// </value>
        public Uri CollectionUri => GetEnvironmentUri("SYSTEM_TEAMFOUNDATIONCOLLECTIONURI");
    }
}
