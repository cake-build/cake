// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Init
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetInitSettings"/>.
    /// </summary>
    public class NuGetInitSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether a package added to an offline feed is also expanded.
        /// </summary>
        /// <value><c>true</c> if package should also be expanded; otherwise, <c>false</c>.</value>
        public bool Expand { get; set; }

        /// <summary>
        /// Gets or sets the output verbosity.
        /// </summary>
        /// <value>The output verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file.
        /// If not specified, the file <c>%AppData%\NuGet\NuGet.config</c> is used as the configuration file.
        /// </summary>
        /// <value>The NuGet configuration file.</value>
        public FilePath ConfigFile { get; set; }
    }
}
