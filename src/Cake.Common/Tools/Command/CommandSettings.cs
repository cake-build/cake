// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Command
{
    /// <summary>
    /// Contains settings used by <see cref="CommandRunner" />.
    /// </summary>
    public class CommandSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the name of the tool.
        /// </summary>
        public virtual string ToolName { get; set; }

        /// <summary>
        /// Gets or sets the tool executable names.
        /// </summary>
        public virtual ICollection<string> ToolExecutableNames { get; set; } = Array.Empty<string>();
    }
}
