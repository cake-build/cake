// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI pipeline information for the current build.
    /// </summary>
    public class WoodpeckerCIPipelineInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIPipelineInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCIPipelineInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the pipeline number.
        /// </summary>
        /// <value>
        /// The pipeline number.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Pipeline Number: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Pipeline.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Pipeline Number: {0}",
        ///         WoodpeckerCI.Environment.Pipeline.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public int Number => GetEnvironmentInteger("CI_PIPELINE_NUMBER");

        /// <summary>
        /// Gets the pipeline parent number.
        /// </summary>
        /// <value>
        /// The pipeline parent number.
        /// </value>
        public int Parent => GetEnvironmentInteger("CI_PIPELINE_PARENT");

        /// <summary>
        /// Gets the pipeline event.
        /// </summary>
        /// <value>
        /// The pipeline event.
        /// </value>
        public string Event => GetEnvironmentString("CI_PIPELINE_EVENT");

        /// <summary>
        /// Gets the pipeline URL.
        /// </summary>
        /// <value>
        /// The pipeline URL.
        /// </value>
        public Uri Url => GetEnvironmentUri("CI_PIPELINE_URL");

        /// <summary>
        /// Gets the pipeline forge URL.
        /// </summary>
        /// <value>
        /// The pipeline forge URL.
        /// </value>
        public Uri ForgeUrl => GetEnvironmentUri("CI_PIPELINE_FORGE_URL");

        /// <summary>
        /// Gets the pipeline deploy target.
        /// </summary>
        /// <value>
        /// The pipeline deploy target.
        /// </value>
        public string DeployTarget => GetEnvironmentString("CI_PIPELINE_DEPLOY_TARGET");

        /// <summary>
        /// Gets the pipeline deploy task.
        /// </summary>
        /// <value>
        /// The pipeline deploy task.
        /// </value>
        public string DeployTask => GetEnvironmentString("CI_PIPELINE_DEPLOY_TASK");

        /// <summary>
        /// Gets the pipeline created timestamp.
        /// </summary>
        /// <value>
        /// The pipeline created timestamp.
        /// </value>
        public DateTimeOffset Created => GetEnvironmentDateTimeOffset("CI_PIPELINE_CREATED");

        /// <summary>
        /// Gets the pipeline started timestamp.
        /// </summary>
        /// <value>
        /// The pipeline started timestamp.
        /// </value>
        public DateTimeOffset Started => GetEnvironmentDateTimeOffset("CI_PIPELINE_STARTED");

        /// <summary>
        /// Gets the pipeline files.
        /// </summary>
        /// <value>
        /// The pipeline files.
        /// </value>
        public string Files => GetEnvironmentString("CI_PIPELINE_FILES");

        /// <summary>
        /// Gets the pipeline author username.
        /// </summary>
        /// <value>
        /// The pipeline author username.
        /// </value>
        public string Author => GetEnvironmentString("CI_PIPELINE_AUTHOR");

        /// <summary>
        /// Gets the pipeline author avatar.
        /// </summary>
        /// <value>
        /// The pipeline author avatar.
        /// </value>
        public Uri Avatar => GetEnvironmentUri("CI_PIPELINE_AVATAR");
    }
}
