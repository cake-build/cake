// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.Download
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyDownloader"/>.
    /// </summary>
    public sealed class ChocolateyDownloadSettings : ChocolateySettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to allow downloading of prerelease packages.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the directory for the downloaded Chocolatey packages.
        /// By default the current working directory is used.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore dependencies.
        /// Requires Chocolatey licensed edition.
        /// </summary>
        public bool IgnoreDependencies { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all external resources should be downloaded
        /// and the package be recompiled to use the local resources instead.
        /// Requires Chocolatey business edition.
        /// </summary>
        public bool Internalize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all URLs, not only from known helpers, should be internalized.
        /// Requires Chocolatey business edition (Licensed version 1.11.1+).
        /// </summary>
        public bool InternalizeAllUrls { get; set; }

        /// <summary>
        /// Gets or sets the location for downloaded resource when <see cref="Internalize"/> is set.
        /// <c>null</c> or <see cref="string.Empty"/> if downloaded resources should be embedded in the package.
        /// Can be a file share or an internal URL location.
        /// When it is a file share, it will attempt to download to that location.
        /// When it is an internal url, it will download locally and give further instructions
        /// where it should be uploaded to match package edits.
        /// By default resources are embedded in the package.
        /// Requires Chocolatey business edition.
        /// </summary>
        public string ResourcesLocation { get; set; }

        /// <summary>
        /// Gets or sets the location where resources should be downloaded to when <see cref="ResourcesLocation"/> is set.
        /// <c>null</c> or <see cref="string.Empty"/> if <see cref="ResourcesLocation"/> should be used.
        /// By default <see cref="ResourcesLocation"/> is used.
        /// Requires Chocolatey business edition.
        /// </summary>
        public string DownloadLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <c>-useOriginalLocation</c> parameter will be passed to any
        /// <c>Install-ChocolateyPackage</c> call in the package and avoiding downloading of resources when
        /// <see cref="Internalize"/> is set.
        /// Overrides the global <c>internalizeAppendUseOriginalLocation</c> feature.
        /// By default set to <c>false</c>.
        /// Requires Chocolatey business edition and Chocolatey v0.10.1 or newer.
        /// </summary>
        public bool AppendUseOriginalLocation { get; set; }

        /// <summary>
        /// Gets or sets the user for authenticated feeds.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the password for authenticated feeds.
        /// </summary>
        public string Password { get; set; }
    }
}