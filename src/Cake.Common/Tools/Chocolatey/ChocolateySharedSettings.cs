// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateySharedSettings"/>.
    /// </summary>
    public class ChocolateySharedSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets the source to find the package(s).
        /// </summary>
        /// <value>The URL or source name.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the specific version of the package.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether install arguments be used exclusively without appending to current package passed arguments.
        /// </summary>
        public bool OverrideArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to process package silently.
        /// </summary>
        public bool NotSilent { get; set; }

        /// <summary>
        /// Gets or sets parameters to pass to the package.
        /// </summary>
        public string PackageParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether install arguments be applied to dependent packages.
        /// </summary>
        public bool ApplyInstallArgumentsToDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether package parameters be applied to dependent packages.
        /// </summary>
        public bool ApplyPackageParametersToDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multiple versions of a package be installed.
        /// </summary>
        public bool SideBySide { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run chocolateyInstall.ps1.
        /// </summary>
        public bool SkipPowerShell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Exit with a 0 for success and 1 for non-success
        /// no matter what package scripts provide for exit codes.
        /// </summary>
        /// <value>The ignore package exit codes flag.</value>
        public bool IgnorePackageExitCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use package exit codes.
        /// </summary>
        /// <value>The use package exit codes flag.</value>
        public bool UsePackageExitCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stop of the first failure of a package.
        /// </summary>
        /// <value>The stop of first failure flag.</value>
        public bool StopOnFirstFailure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exit when a reboot is detected.
        /// </summary>
        /// <value>The exit when reboot detected flag.</value>
        public bool ExitWhenRebootDetected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore detected reboots.
        /// </summary>
        /// <value>The ignore detected reboots flag.</value>
        public bool IgnoreDetectedReboot { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip the running of package hook scripts.
        /// </summary>
        /// <value>The skip hooks flag.</value>
        public bool SkipHooks { get; set; }
    }
}
