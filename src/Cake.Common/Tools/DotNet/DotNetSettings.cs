// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// Contains common settings used by <see cref="DotNetTool{TSettings}" />.
    /// </summary>
    public abstract class DotNetSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the verbosity of logging to use.
        /// </summary>
        public DotNetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not enable diagnostic output.
        /// </summary>
        public bool DiagnosticOutput { get; set; }

        /// <summary>
        /// Gets or sets the dotnet roll forward policy.
        /// </summary>
        public DotNetRollForward? RollForward { get; set; }
    }
}
