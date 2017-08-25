// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.MSBuild;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Represents the Distributed Logging Model with a central logger and forwarding logger.
    /// </summary>
    public class MSBuildDistributedLogger
    {
        /// <summary>
        /// Gets or sets the logger to use as the central logger.
        /// </summary>
        public MSBuildLogger CentralLogger { get; set; }

        /// <summary>
        /// Gets or sets the logger to use as the forwarding logger.
        /// </summary>
        public MSBuildLogger ForwardingLogger { get; set; }
    }
}