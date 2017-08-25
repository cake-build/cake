// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
            var builder = new ProcessArgumentBuilder();

            builder.Append("download");
            builder.AppendQuoted(packageId);

            AddCommonArguments(settings, builder);

            // Prerelease
            if (settings.Prerelease)
            {
                builder.Append("--pre");
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--outputdirectory");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Ignore Dependencies
            if (settings.IgnoreDependencies)
            {
                builder.Append("-i");
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
                    builder.AppendSwitchQuoted("--resources-location", "=", settings.ResourcesLocation);

                    // Download Location
                    if (!string.IsNullOrWhiteSpace(settings.DownloadLocation))
                    {
                        builder.AppendSwitchQuoted("--download-location", "=", settings.DownloadLocation);
                    }
                }

                // Append -UseOriginalLocation
                if (settings.AppendUseOriginalLocation)
                {
                    builder.Append("--append-useoriginallocation");
                }
            }

            // User
            if (!string.IsNullOrWhiteSpace(settings.User))
            {
                builder.Append("-u");
                builder.AppendQuoted(settings.User);
            }

            // Password
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append("-p");
                builder.AppendQuoted(settings.Password);
            }

            return builder;
        }
    }
}