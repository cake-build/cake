// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// Provides Go.CD commit information for a current build.
    /// </summary>
    public sealed class GoCDStageInfo : GoCDInfo
    {
        /// <summary>
        /// Gets the name of the current stage being run.
        /// </summary>
        /// <value>
        /// The stage name.
        /// </value>
        public string Name => GetEnvironmentString("GO_STAGE_NAME");

        /// <summary>
        /// Gets the count of the number of times the current stage has been run.
        /// </summary>
        /// <value>
        /// The stage counter.
        /// </value>
        public int Counter => GetEnvironmentInteger("GO_STAGE_COUNTER");

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDStageInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GoCDStageInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}