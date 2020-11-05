// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Uninstall
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyUninstaller"/>.
    /// </summary>
    public sealed class ChocolateyUninstallSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether it fails on standard error output.
        /// </summary>
        /// <value>The fail on standard error flag.</value>
        public bool FailOnStandardError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an external process is used instead of built in PowerShell host.
        /// </summary>
        /// <value>The use system powershell flag.</value>
        public bool UseSystemPowershell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to uninstall all versions.
        /// </summary>
        /// <value>The all versions flag.</value>
        public bool AllVersions { get; set; }

        /// <summary>
        /// Gets or sets the uninstall arguments to pass to native installer.
        /// </summary>
        public string UninstallArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether install arguments should be applied to dependent packages.
        /// </summary>
        /// <value>The global arguments flag.</value>
        public bool GlobalArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether global parameters are passed to the package.
        /// </summary>
        /// <value>The global package parameters flag.</value>
        public bool GlobalPackageParameters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force dependencies.
        /// </summary>
        /// <value>The force dependencies flag.</value>
        public bool ForceDependencies { get; set; }

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
        /// Gets or sets a value indicating whether to use auto uninstaller service when uninstalling.
        /// </summary>
        /// <value>The use auto uninstaller flag.</value>
        public bool UseAutoUninstaller { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip auto uninstaller service when uninstalling.
        /// </summary>
        /// <value>The skip auto uninstaller flag.</value>
        public bool SkipAutoUninstaller { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fail the package uninstall if the auto
        /// uninstaller reports and error.
        /// </summary>
        /// <value>The fail auto uninstaller flag.</value>
        public bool FailOnAutoUninstaller { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not fail the package if auto
        /// uninstaller reports an error.
        /// </summary>
        /// <value>The ignore auto uninstaller flag.</value>
        public bool IgnoreAutoUninstaller { get; set; }
    }
}
