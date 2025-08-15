// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI workflow information for the current build.
    /// </summary>
    public class WoodpeckerCIWorkflowInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIWorkflowInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCIWorkflowInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the workflow name.
        /// </summary>
        /// <value>
        /// The workflow name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Workflow Name: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Workflow.Name
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
        ///         @"Workflow Name: {0}",
        ///         WoodpeckerCI.Environment.Workflow.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public string Name => GetEnvironmentString("CI_WORKFLOW_NAME");
    }
}
