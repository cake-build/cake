// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Upgrade
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyUpgrader"/>.
    /// </summary>
    public sealed class ChocolateyUpgradeSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to run in debug mode.
        /// </summary>
        /// <value>The debug flag</value>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in verbose mode.
        /// </summary>
        /// <value>The verbose flag.</value>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to accept license for package.
        /// </summary>
        /// <value>The accept license flag</value>
        public bool AcceptLicense { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in forced mode.
        /// </summary>
        /// <value>The force flag</value>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in noop mode.
        /// </summary>
        /// <value>The noop flag.</value>
        public bool Noop { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in limited output mode.
        /// </summary>
        /// <value>The limit output flag</value>
        public bool LimitOutput { get; set; }

        /// <summary>
        /// Gets or sets the execution timeout value.
        /// </summary>
        /// <value>The execution timeout</value>
        /// <remarks>Default is 2700 seconds</remarks>
        public int ExecutionTimeout { get; set; }

        /// <summary>
        /// Gets or sets the location of the download cache.
        /// </summary>
        /// <value>The download cache location</value>
        public string CacheLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run in allow unofficial mode.
        /// </summary>
        /// <value>The allow unofficial flag</value>
        public bool AllowUnofficial { get; set; }

        /// <summary>
        /// Gets or sets the source to use for this command.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the Nuspec version.
        /// </summary>
        /// <value>The Nuspec version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow upgrading to prerelease versions.
        /// This flag is not required when updating prerelease packages that are already installed.
        /// </summary>
        /// <value>
        ///   <c>true</c> to allow updating to prerelease versions; otherwise, <c>false</c>.
        /// </value>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force installation of the x86 version of package.
        /// </summary>
        /// <value>The force x86 flag</value>
        public bool Forcex86 { get; set; }

        /// <summary>
        /// Gets or sets the install arguments to pass to native installer.
        /// </summary>
        public string InstallArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to override the passed arguments.
        /// </summary>
        /// <value>The override arguments flag</value>
        public bool OverrideArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to install silently.
        /// </summary>
        /// <value>The not silent flag</value>
        public bool NotSilent { get; set; }

        /// <summary>
        /// Gets or sets the parameters to pass to the package.
        /// </summary>
        public string PackageParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow downgrade of package.
        /// </summary>
        /// <value>The downgrade package flag</value>
        public bool AllowDowngrade { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow side by side installation.
        /// </summary>
        /// <value>The side by side installation flag</value>
        public bool SideBySide { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore dependencies.
        /// </summary>
        /// <value>The ignore dependencies flag</value>
        public bool IgnoreDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip the PowerShell installation of package.
        /// </summary>
        /// <value>The skip powershell flag</value>
        public bool SkipPowerShell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fail on unfound packages.
        /// </summary>
        /// <value>The skip powershell flag</value>
        public bool FailOnUnfound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fail on not installed packages.
        /// </summary>
        /// <value>The skip powershell flag</value>
        public bool FailOnNotInstalled { get; set; }

        /// <summary>
        /// Gets or sets the user for authenticated feeds.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the password for authenticated feeds.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore checksums.
        /// </summary>
        /// <value>The ignore checksums flag</value>
        public bool IgnoreChecksums { get; set; }
    }
}
