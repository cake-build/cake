// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// Contains common settings used by <see cref="DotNetCoreTool{TSettings}" />.
    /// </summary>
    public abstract class DotNetCoreSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the verbosity of logging to use.
        /// </summary>
        public DotNetCoreVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not enable diagnostic output.
        /// </summary>
        public bool DiagnosticOutput { get; set; }
    }
}
