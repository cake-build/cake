// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Uninstall
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyUninstaller"/>.
    /// </summary>
    public sealed class ChocolateyUninstallSettings : ChocolateySharedSettings
    {
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
        /// Gets or sets a value indicating whether to force dependencies.
        /// </summary>
        /// <value>The force dependencies flag.</value>
        public bool ForceDependencies { get; set; }

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
        public bool IgnoreAutoUninstallerFailure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to uninstall a program from programs and features.
        /// </summary>
        public bool FromProgramsAndFeatures { get; set; }
    }
}
