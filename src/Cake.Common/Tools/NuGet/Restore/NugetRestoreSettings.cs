// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Restore
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetRestorer"/>.
    /// </summary>
    public sealed class NuGetRestoreSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether package restore consent is granted before installing a package.
        /// </summary>
        /// <value>
        ///   <c>true</c> if package restore consent is granted; otherwise, <c>false</c>.
        /// </value>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// Gets or sets the packages folder.
        /// </summary>
        public DirectoryPath PackagesDirectory { get; set; }

        /// <summary>
        /// Gets or sets a list of packages sources to use for this command.
        /// </summary>
        public ICollection<string> Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use the machine cache as the first package source.
        /// </summary>
        /// <value>
        ///   <c>true</c> to not use the machine cache as the first package source; otherwise, <c>false</c>.
        /// </value>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to disable parallel processing of packages for this command.
        /// </summary>
        /// <value>
        /// <c>true</c> to disable parallel processing; otherwise, <c>false</c>.
        /// </value>
        public bool DisableParallelProcessing { get; set; }

        /// <summary>
        /// Gets or sets the amount of output details.
        /// </summary>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file.
        /// If not specified, the file <c>%AppData%\NuGet\NuGet.config</c> is used as the configuration file.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a list of packages sources to use as fallbacks for this command.
        /// This setting requires NuGet V3 or later.
        /// </summary>
        /// <value>The list of packages sources to use as fallbacks for this command.</value>
        public ICollection<string> FallbackSource { get; set; }

        /// <summary>
        /// Gets or sets the version of MSBuild to be used with this command.
        /// By default the MSBuild in your path is picked, otherwise it defaults to the highest installed version of MSBuild.
        /// This setting requires NuGet V3 or later.
        /// </summary>
        /// <value>The version of MSBuild to be used with this command.</value>
        public NuGetMSBuildVersion? MSBuildVersion { get; set; }
    }
}
