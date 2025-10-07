// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Install
{
    /// <summary>
    /// The Chocolatey package installer used to install Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyInstaller : ChocolateyTool<ChocolateyInstallSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package configuration file and settings.
        /// </summary>
        /// <param name="packageConfigPath">Path to package configuration to use for install.</param>
        /// <param name="settings">The settings.</param>
        public void InstallFromConfig(FilePath packageConfigPath, ChocolateyInstallSettings settings)
        {
            ArgumentNullException.ThrowIfNull(packageConfigPath);

            ArgumentNullException.ThrowIfNull(settings);

            var packageId = packageConfigPath.MakeAbsolute(_environment).FullPath;

            Run(settings, GetArguments(packageId, settings));
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="packageId">The source package id.</param>
        /// <param name="settings">The settings.</param>
        public void Install(string packageId, ChocolateyInstallSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetArguments(packageId, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageId, ChocolateyInstallSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("install");
            builder.AppendQuoted(packageId);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Add shared arguments using the inherited method
            AddSharedArguments(settings, builder);

            // Prerelease
            if (settings.Prerelease)
            {
                builder.Append("--pre");
            }

            // Forcex86
            if (settings.Forcex86)
            {
                builder.Append("--forcex86");
            }

            // Install Arguments
            if (!string.IsNullOrWhiteSpace(settings.InstallArguments))
            {
                builder.AppendSwitchQuoted("--install-arguments", separator, settings.InstallArguments);
            }

            // Allow Downgrade
            if (settings.AllowDowngrade)
            {
                builder.Append("--allow-downgrade");
            }

            // Ignore Dependencies
            if (settings.IgnoreDependencies)
            {
                builder.Append("--ignore-dependencies");
            }

            // Force Dependencies
            if (settings.ForceDependencies)
            {
                builder.Append("--force-dependencies");
            }

            // User
            if (!string.IsNullOrWhiteSpace(settings.User))
            {
                builder.AppendSwitchQuoted("--user", separator, settings.User);
            }

            // Password
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.AppendSwitchQuoted("--password", separator, settings.Password);
            }

            // Certificate
            if (settings.Certificate != null)
            {
                builder.AppendSwitchQuoted("--cert", separator, settings.Certificate.MakeAbsolute(_environment).FullPath);
            }

            // Certificate Password
            if (!string.IsNullOrEmpty(settings.CertificatePassword))
            {
                builder.AppendSwitchQuoted("--certpassword", separator, settings.CertificatePassword);
            }

            // Ignore Checksums
            if (settings.IgnoreChecksums)
            {
                builder.Append("--ignore-checksums");
            }

            // Allow Empty Checksums
            if (settings.AllowEmptyChecksums)
            {
                builder.Append("--allow-empty-checksums");
            }

            // Allow Empty Checksums Secure
            if (settings.AllowEmptyChecksumsSecure)
            {
                builder.Append("--allow-empty-checksums-secure");
            }

            // Require Checksums
            if (settings.RequireChecksums)
            {
                builder.Append("--require-checksums");
            }

            // Checksum
            if (!string.IsNullOrEmpty(settings.Checksum))
            {
                builder.AppendSwitchQuoted("--download-checksum", separator, settings.Checksum);
            }

            // Checksum 64
            if (!string.IsNullOrEmpty(settings.Checksum64))
            {
                builder.AppendSwitchQuoted("--download-checksum-x64", separator, settings.Checksum64);
            }

            // Checksum Type
            if (!string.IsNullOrEmpty(settings.ChecksumType))
            {
                builder.AppendSwitchQuoted("--download-checksum-type", separator, settings.ChecksumType);
            }

            // Checksum Type 64
            if (!string.IsNullOrEmpty(settings.ChecksumType64))
            {
                builder.AppendSwitchQuoted("--download-checksum-type-x64", separator, settings.ChecksumType64);
            }

            // Disable Repository Optimizations
            if (settings.DisableRepositoryOptimizations)
            {
                builder.Append("--disable-repository-optimizations");
            }

            // Pin
            if (settings.Pin)
            {
                builder.Append("--pin-package");
            }

            // Skip Download Cache
            if (settings.SkipDownloadCache)
            {
                builder.Append("--skip-download-cache");
            }

            // Use Download Cache
            if (settings.UseDownloadCache)
            {
                builder.Append("--use-download-cache");
            }

            // Skip Virus Check
            if (settings.SkipVirusCheck)
            {
                builder.Append("--skip-virus-check");
            }

            // Virus Check
            if (settings.VirusCheck)
            {
                builder.Append("--virus-check");
            }

            // Virus Positive Minimum
            if (settings.VirusPositivesMinimum != 0)
            {
                builder.AppendSwitchQuoted("--virus-positives-minimum", separator, settings.VirusPositivesMinimum.ToString(CultureInfo.InvariantCulture));
            }

            // Install Arguments Sensitive
            if (!string.IsNullOrWhiteSpace(settings.InstallArgumentsSensitive))
            {
                builder.AppendSwitchQuoted("--install-arguments-sensitive", separator, settings.InstallArgumentsSensitive);
            }

            // Package Parameters Sensitive
            if (!string.IsNullOrEmpty(settings.PackageParametersSensitive))
            {
                builder.AppendSwitchQuoted("--package-parameters-sensitive", separator, settings.PackageParametersSensitive);
            }

            // Install Directory
            if (settings.InstallDirectory != null)
            {
                builder.AppendSwitchQuoted("--install-directory", separator, settings.InstallDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Maximum Download Bits Per Second
            if (settings.MaximumDownloadBitsPerSecond != 0)
            {
                builder.AppendSwitchQuoted("--maximum-download-bits-per-second", separator, settings.MaximumDownloadBitsPerSecond.ToString(CultureInfo.InvariantCulture));
            }

            // Reduce Package Size
            if (settings.ReducePackageSize)
            {
                builder.Append("--reduce-package-size");
            }

            // No Reduce Package Size
            if (settings.NoReducePackageSize)
            {
                builder.Append("--no-reduce-package-size");
            }

            // Reduce Nupkg Only
            if (settings.ReduceNupkgOnly)
            {
                builder.Append("--reduce-nupkg-only");
            }

            // Pin Reason
            if (!string.IsNullOrEmpty(settings.PinReason))
            {
                builder.AppendSwitchQuoted("--pin-reason", separator, settings.PinReason);
            }

            return builder;
        }
    }
}