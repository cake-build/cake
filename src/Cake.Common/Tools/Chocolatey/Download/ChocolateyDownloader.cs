// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Download
{
    /// <summary>
    /// The Chocolatey package downloader used to download Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyDownloader : ChocolateyTool<ChocolateyDownloadSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyDownloader"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyDownloader(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Downloads Chocolatey packages using the specified package id and settings.
        /// Requires Chocolatey licensed edition.
        /// Features requiring Chocolatey for Business or a minimum version are documented
        /// in <see cref="ChocolateyDownloadSettings"/>.
        /// </summary>
        /// <param name="packageId">The source package id.</param>
        /// <param name="settings">The settings.</param>
        public void Download(string packageId, ChocolateyDownloadSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(packageId, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageId, ChocolateyDownloadSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("download");
            builder.AppendQuoted(packageId);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Source
            if (!string.IsNullOrEmpty(settings.Source))
            {
                builder.AppendSwitchQuoted("--source", separator, settings.Source);
            }

            // Version
            if (!string.IsNullOrEmpty(settings.Version))
            {
                builder.AppendSwitchQuoted("--version", separator, settings.Version);
            }

            // Prerelease
            if (settings.Prerelease)
            {
                builder.Append("--pre");
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

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.AppendSwitchQuoted("--output-directory", separator, settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Ignore Dependencies
            if (settings.IgnoreDependencies)
            {
                builder.Append("--ignore-dependencies");
            }

            // Installed
            if (settings.Installed)
            {
                builder.Append("--installed-packages");
            }

            // Ignore Unfound
            if (settings.IgnoreUnfound)
            {
                builder.Append("--ignore-unfound");
            }

            // Disable Repository Optimizations
            if (settings.DisableRepositoryOptimizations)
            {
                builder.Append("--disable-repository-optimizations");
            }

            // Internalize
            if (settings.Internalize)
            {
                builder.Append("--internalize");

                // Internalize All
                if (settings.InternalizeAllUrls)
                {
                    builder.Append("--internalize-all-urls");
                }

                // Resources Location
                if (!string.IsNullOrWhiteSpace(settings.ResourcesLocation))
                {
                    builder.AppendSwitchQuoted("--resources-location", separator, settings.ResourcesLocation);

                    // Download Location
                    if (!string.IsNullOrWhiteSpace(settings.DownloadLocation))
                    {
                        builder.AppendSwitchQuoted("--download-location", separator, settings.DownloadLocation);
                    }
                }

                // Append -UseOriginalLocation
                if (settings.AppendUseOriginalLocation)
                {
                    builder.Append("--append-use-original-location");
                }
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

            return builder;
        }
    }
}