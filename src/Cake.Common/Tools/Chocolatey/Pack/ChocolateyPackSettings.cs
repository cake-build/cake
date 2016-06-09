// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyPacker"/>.
    /// </summary>
    public sealed class ChocolateyPackSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the package ID.
        /// </summary>
        /// <value>The package ID.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the package title.
        /// </summary>
        /// <value>The package title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Nuspec version.
        /// </summary>
        /// <value>The Nuspec version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the package authors.
        /// </summary>
        /// <value>The package authors.</value>
        public ICollection<string> Authors { get; set; }

        /// <summary>
        /// Gets or sets the package owners.
        /// </summary>
        /// <value>The package owners.</value>
        public ICollection<string> Owners { get; set; }

        /// <summary>
        /// Gets or sets the package summary.
        /// </summary>
        /// <value>The package summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the package description.
        /// </summary>
        /// <value>The package description.</value>
        /// <remarks>Markdown format is allowed for this property</remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the package project URL.
        /// </summary>
        /// <value>The package project URL.</value>
        public Uri ProjectUrl { get; set; }

        /// <summary>
        /// Gets or sets the package Source URL.
        /// </summary>
        /// <value>The package Source URL.</value>
        /// <remarks>Requires at least Chocolatey 0.9.9.7.</remarks>
        public Uri PackageSourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the package project Source URL.
        /// </summary>
        /// <value>The package project Source URL.</value>
        /// <remarks>Requires at least Chocolatey 0.9.9.7.</remarks>
        public Uri ProjectSourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the package documentation URL.
        /// </summary>
        /// <value>The package documenation URL.</value>
        /// <remarks>Requires at least Chocolatey 0.9.9.7.</remarks>
        public Uri DocsUrl { get; set; }

        /// <summary>
        /// Gets or sets the package mailing list URL.
        /// </summary>
        /// <value>The package mailing list URL.</value>
        /// <remarks>Requires at least Chocolatey 0.9.9.7.</remarks>
        public Uri MailingListUrl { get; set; }

        /// <summary>
        /// Gets or sets the package bug tracker URL.
        /// </summary>
        /// <value>The package bug tracker URL.</value>
        /// <remarks>Requires at least Chocolatey 0.9.9.7.</remarks>
        public Uri BugTrackerUrl { get; set; }

        /// <summary>
        /// Gets or sets the package tags.
        /// </summary>
        /// <value>The package tags.</value>
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the package copyright.
        /// </summary>
        /// <value>The package copyright.</value>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the package license URL.
        /// </summary>
        /// <value>The package license URL.</value>
        public Uri LicenseUrl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users has to accept the package license.
        /// </summary>
        /// <value>
        /// <c>true</c> if users has to accept the package license; otherwise, <c>false</c>.
        /// </value>
        public bool RequireLicenseAcceptance { get; set; }

        /// <summary>
        /// Gets or sets the package icon URL.
        /// </summary>
        /// <value>The package icon URL.</value>
        public Uri IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the package release notes.
        /// </summary>
        /// <value>The package release notes.</value>
        /// <remarks>Markdown format is allowed for this property</remarks>
        public ICollection<string> ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the package files.
        /// </summary>
        /// <value>The package files.</value>
        public ICollection<ChocolateyNuSpecContent> Files { get; set; }

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
        /// Gets or sets a value indicating the Working Directory that should be used while running choco.exe.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }
    }
}
