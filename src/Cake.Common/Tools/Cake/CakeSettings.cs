// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Cake
{
    /// <summary>
    /// Contains settings used by <see cref="CakeRunner"/>.
    /// </summary>
    public sealed class CakeSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public Verbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets cake additional arguments.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, string> Arguments { get; set; }
    }
}
