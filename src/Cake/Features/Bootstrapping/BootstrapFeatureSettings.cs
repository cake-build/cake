// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Features.Bootstrapping
{
    /// <summary>
    /// Settings for the bootstrap feature.
    /// </summary>
    public class BootstrapFeatureSettings
    {
        /// <summary>
        /// Gets or sets the script file path.
        /// </summary>
        public FilePath Script { get; set; }

        /// <summary>
        /// Gets or sets the verbosity level.
        /// </summary>
        public Verbosity Verbosity { get; set; }
    }
}
