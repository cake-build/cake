// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// Provides GoCD pipeline information for a current build.
    /// </summary>
    public sealed class GoCDPipelineInfo : GoCDInfo
    {
        /// <summary>
        /// Gets the name of the pipeline.
        /// </summary>
        /// <value>
        /// The name of the pipeline.
        /// </value>
        public string Name => GetEnvironmentString("GO_PIPELINE_NAME");

        /// <summary>
        /// Gets the pipeline counter, showing how many times the current pipeline has been run.
        /// </summary>
        /// <value>
        /// The pipeline counter.
        /// </value>
        public int Counter => GetEnvironmentInteger("GO_PIPELINE_COUNTER");

        /// <summary>
        /// Gets the pipeline label. By default, this is set to the pipeline count.
        /// </summary>
        /// <value>
        /// The pipeline label.
        /// </value>
        public string Label => GetEnvironmentString("GO_PIPELINE_LABEL");

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDPipelineInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GoCDPipelineInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}