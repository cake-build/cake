// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.Chocolatey.Upgrade
{
    using global::Cake.Core.IO;

    /// <summary>
    /// Contains settings used by <see cref="ChocolateyUpgrader"/>.
    /// </summary>
    public sealed class ChocolateyUpgradeSettings : ChocolateySharedSettings
    {
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
        /// Gets or sets a value indicating whether to fail on unfound packages.
        /// </summary>
        /// <value>The skip powershell flag.</value>
        public bool FailOnUnfound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore unfound packages if downloading more than one at a time.
        /// </summary>
        public bool IgnoreUnfound { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fail on not installed packages.
        /// </summary>
        /// <value>The skip powershell flag.</value>
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
        /// Gets or sets the path to a PFX certificate for use with x509 authenticated feeds.
        /// </summary>
        public FilePath Certificate { get; set; }

        /// <summary>
        /// Gets or sets the password for the <see cref="Certificate"/>.
        /// </summary>
        public string CertificatePassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore checksums.
        /// </summary>
        /// <value>The ignore checksums flag.</value>
        public bool IgnoreChecksums { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow empty checksums for bare HTTP URLs during package installation.
        /// </summary>
        public bool AllowEmptyChecksums { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow empty checksums for HTTPS URLs during package installation.
        /// </summary>
        public bool AllowEmptyChecksumsSecure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether checksums are required during package installation.
        /// </summary>
        public bool RequireChecksums { get; set; }

        /// <summary>
        /// Gets or sets the checksum for 32 bit installation.
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// Gets or sets the checksum for 64 bit installation.
        /// </summary>
        public string Checksum64 { get; set; }

        /// <summary>
        /// Gets or sets the checksum type to use for 32 bit installation.
        /// </summary>
        public string ChecksumType { get; set; }

        /// <summary>
        /// Gets or sets the checksum type to use for 64 bit installation.
        /// </summary>
        public string ChecksumType64 { get; set; }

        /// <summary>
        /// Gets or sets a comma separated list of package names that should not be upgraded when running upgrade all.
        /// </summary>
        public string Except { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a package will be skipped if it is not installed during upgrade operation.
        /// </summary>
        public bool SkipIfNotInstalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to install a package that is not installed during the upgrade operation.
        /// </summary>
        public bool InstallIfNotInstalled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether prerelease packages should be ignored for upgrades.
        /// </summary>
        public bool ExcludePrerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the remembered arguments for a package.
        /// </summary>
        public bool UseRememberedArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the remembered arguments for package.
        /// </summary>
        public bool IgnoreRememeredArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use optimizations for reducing bandwidth when communicating with repositories.
        /// </summary>
        public bool DisableRepositoryOptimizations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to pin the package once installed.
        /// </summary>
        public bool Pin { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip the package download cache.
        /// </summary>
        public bool SkipDownloadCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the package download cache.
        /// </summary>
        public bool UseDownloadCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether skip the virus checking for a package.
        /// </summary>
        public bool SkipVirusCheck { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force virus checking for a package.
        /// </summary>
        public bool VirusCheck { get; set; }

        /// <summary>
        /// Gets or sets the minimum allowed number of virus positives.
        /// </summary>
        public int VirusPositivesMinimum { get; set; }

        /// <summary>
        /// Gets or sets the install arguments sensitive to pass to native installer.
        /// </summary>
        public string InstallArgumentsSensitive { get; set; }

        /// <summary>
        /// Gets or sets sensitive parameters to pass to the package.
        /// </summary>
        public string PackageParametersSensitive { get; set; }

        /// <summary>
        /// Gets or sets the default application installation directory.
        /// </summary>
        public DirectoryPath InstallDirectory { get; set; }

        /// <summary>
        /// Gets or sets the maximum download bits per second when downloading a package.
        /// </summary>
        public int MaximumDownloadBitsPerSecond { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether package size should be reduced on installation.
        /// </summary>
        public bool ReducePackageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there should be no reduction in package size on installation.
        /// </summary>
        public bool NoReducePackageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only the nupkg file size should be reduced.
        /// </summary>
        public bool ReduceNupkgOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude all Chocolatey owned packages during upgrade all operation.
        /// </summary>
        public bool ExcludeChocolateyPackagesDuringUpgradeAll { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to include all Chocolatey owned packages during upgrade all operation.
        /// </summary>
        public bool IncludeChocolateyPackagesDuringUpgradeAll { get; set; }

        /// <summary>
        /// Gets or sets a reason for pinning a package during installation.
        /// </summary>
        public string PinReason { get; set; }
    }
}