// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Rwx.Data
{
    /// <summary>
    /// Provides RWX run information for the current build.
    /// </summary>
    public sealed class RwxRunInfo : RwxInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RwxRunInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public RwxRunInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the RWX identifier of the current run.
        /// </summary>
        /// <value>
        /// The run identifier.
        /// </value>
        public string Id => GetEnvironmentString("RWX_RUN_ID");

        /// <summary>
        /// Gets the title of the current RWX run.
        /// </summary>
        /// <value>
        /// The run title.
        /// </value>
        public string Title => GetEnvironmentString("RWX_RUN_TITLE");

        /// <summary>
        /// Gets the link to the current run in RWX.
        /// </summary>
        /// <value>
        /// The run URL.
        /// </value>
        public string Url => GetEnvironmentString("RWX_RUN_URL");
    }
}
