// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetPacker"/>.
    /// </summary>
    public sealed class NuGetPackSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>The base path.</value>
        public DirectoryPath BasePath { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether package analysis should be performed.
        /// Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if package analysis should be performed; otherwise, <c>false</c>.
        /// </value>
        public bool NoPackageAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether referenced projects should be included.
        /// Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if referenced projects should be included; otherwise, <c>false</c>.
        /// </value>
        public bool IncludeReferencedProjects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a symbol package should be created.
        /// Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a symbol package should be created; otherwise, <c>false</c>.
        /// </value>
        public bool Symbols { get; set; }

        /// <summary>
        /// Gets or sets the package ID.
        /// </summary>
        /// <value>The package ID.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the Nuspec version.
        /// </summary>
        /// <value>The Nuspec version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the package title.
        /// </summary>
        /// <value>The package title.</value>
        public string Title { get; set; }

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
        /// Gets or sets the package description.
        /// </summary>
        /// <value>The package description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the package summary.
        /// </summary>
        /// <value>The package summary.</value>
        public string Summary { get; set; }

        /// <summary>
        /// Gets or sets the package project URL.
        /// </summary>
        /// <value>The package project URL.</value>
        public Uri ProjectUrl { get; set; }

        /// <summary>
        /// Gets or sets the package icon URL.
        /// </summary>
        /// <value>The package icon URL.</value>
        public Uri IconUrl { get; set; }

        /// <summary>
        /// Gets or sets the package license URL.
        /// </summary>
        /// <value>The package license URL.</value>
        public Uri LicenseUrl { get; set; }

        /// <summary>
        /// Gets or sets the package copyright.
        /// </summary>
        /// <value>The package copyright.</value>
        public string Copyright { get; set; }

        /// <summary>
        /// Gets or sets the package release notes.
        /// </summary>
        /// <value>The package release notes.</value>
        public ICollection<string> ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the package tags.
        /// </summary>
        /// <value>The package tags.</value>
        public ICollection<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this package should be marked as a development dependency.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a development dependency; otherwise, <c>false</c>.
        /// </value>
        public bool DevelopmentDependency { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether users has to accept the package license.
        /// </summary>
        /// <value>
        /// <c>true</c> if users has to accept the package license; otherwise, <c>false</c>.
        /// </value>
        public bool RequireLicenseAcceptance { get; set; }

        /// <summary>
        /// Gets or sets the package files.
        /// </summary>
        /// <value>The package files.</value>
        public ICollection<NuSpecContent> Files { get; set; }

        /// <summary>
        /// Gets or sets the package dependencies.
        /// </summary>
        /// <value>The package files.</value>
        public ICollection<NuSpecDependency> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public IDictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets or sets the version of MSBuild to be used with this command.
        /// By default the MSBuild in your path is picked, otherwise it defaults to the highest installed version of MSBuild.
        /// This setting requires NuGet V3 or later.
        /// </summary>
        /// <value>The version of MSBuild to be used with this command.</value>
        public NuGetMSBuildVersion? MSBuildVersion { get; set; }
    }
}
