// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise workflow information for the current build.
    /// </summary>
    public class BitriseWorkflowInfo : BitriseInfo
    {
        /// <summary>
        /// Gets the workflow identifier.
        /// </summary>
        /// <value>
        /// The workflow identifier.
        /// </value>
        public string WorkflowId
        {
            get { return GetEnvironmentString("BITRISE_TRIGGERED_WORKFLOW_ID"); }
        }

        /// <summary>
        /// Gets the workflow title.
        /// </summary>
        /// <value>
        /// The workflow title.
        /// </value>
        public string WorkflowTitle
        {
            get { return GetEnvironmentString("BITRISE_TRIGGERED_WORKFLOW_TITLE"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseWorkflowInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseWorkflowInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
