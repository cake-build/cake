// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Update
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetUpdater"/>.
    /// </summary>
    public sealed class NuGetUpdateSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the package ids to update.
        /// </summary>
        /// <value>The package ids to update.</value>
        public ICollection<string> Id { get; set; }

        /// <summary>
        /// Gets or sets a list of package sources to use for this command.
        /// </summary>
        public ICollection<string> Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to look for updates with the highest
        /// version available within the same major and minor version as the installed package.
        /// </summary>
        /// <value>
        ///   <c>true</c> if safe; otherwise, <c>false</c>.
        /// </value>
        public bool Safe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow updating to prerelease versions.
        /// This flag is not required when updating prerelease packages that are already installed.
        /// </summary>
        /// <value>
        ///   <c>true</c> to allow updating to prerelease versions; otherwise, <c>false</c>.
        /// </value>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the amount of output details.
        /// </summary>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the version of MSBuild to be used with this command.
        /// By default the MSBuild in your path is picked, otherwise it defaults to the highest installed version of MSBuild.
        /// This setting requires NuGet V3 or later.
        /// </summary>
        /// <value>The version of MSBuild to be used with this command.</value>
        public NuGetMSBuildVersion? MSBuildVersion { get; set; }
    }
}
