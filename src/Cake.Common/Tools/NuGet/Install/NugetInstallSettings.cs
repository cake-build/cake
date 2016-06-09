// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Install
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetInstaller"/>.
    /// </summary>
    public sealed class NuGetInstallSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the directory in which packages will be installed.
        /// If none is specified, the current directory will be used.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the version of the package to install.
        /// If none specified, the latest will be used.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude the version number from the package folder.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to exclude the version number from the package folder; otherwise, <c>false</c>.
        /// </value>
        public bool ExcludeVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow installation of prerelease packages.
        /// This flag is not required when restoring packages by installing from packages.config.
        /// </summary>
        /// <value>
        ///   <c>true</c> to allow installation of prerelease packages; otherwise, <c>false</c>.
        /// </value>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to check if package
        /// install consent is granted before installing a package.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to check if package install consent is granted before installing a package; otherwise, <c>false</c>.
        /// </value>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// Gets or sets the solution directory path for package restore.
        /// </summary>
        /// <value>
        /// The solution directory path.
        /// </value>
        public DirectoryPath SolutionDirectory { get; set; }

        /// <summary>
        /// Gets or sets a list of packages sources to use for this command.
        /// </summary>
        /// <value>The list of packages sources to use for this command.</value>
        public ICollection<string> Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use the machine cache as the first package source.
        /// </summary>
        /// <value>
        ///   <c>true</c> to not use the machine cache as the first package source; otherwise, <c>false</c>.
        /// </value>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable parallel processing of packages for this command.
        /// Disable parallel processing of packages for this command.
        /// </summary>
        /// <value>
        ///   <c>true</c> to disable parallel processing of packages for this command; otherwise, <c>false</c>.
        /// </value>
        public bool DisableParallelProcessing { get; set; }

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

        /// <summary>
        /// Gets or sets a list of packages sources to use as fallbacks for this command.
        /// This setting requires NuGet V3 or later.
        /// </summary>
        /// <value>The list of packages sources to use as fallbacks for this command.</value>
        public ICollection<string> FallbackSource { get; set; }
    }
}
