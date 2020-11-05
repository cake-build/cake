// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Install
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyInstaller"/>.
    /// </summary>
    public sealed class ChocolateyInstallSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow installation of prerelease packages.
        /// This flag is not required when restoring packages by installing from packages.config.
        /// </summary>
        /// <value>
        ///   <c>true</c> to allow installation of prerelease packages; otherwise, <c>false</c>.
        /// </value>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force installation of the x86 version of package.
        /// </summary>
        /// <value>The force x86 flag.</value>
        public bool Forcex86 { get; set; }

        /// <summary>
        /// Gets or sets the install arguments to pass to native installer.
        /// </summary>
        public string InstallArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow downgrade of package.
        /// </summary>
        /// <value>The downgrade package flag.</value>
        public bool AllowDowngrade { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore dependencies.
        /// </summary>
        /// <value>The ignore dependencies flag.</value>
        public bool IgnoreDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force dependencies.
        /// </summary>
        /// <value>The force dependencies flag.</value>
        public bool ForceDependencies { get; set; }

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
        /// <value>The ignore checksums flag.</value>
        public bool IgnoreChecksums { get; set; }
    }
}