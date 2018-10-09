using System;
using System.Collections.Generic;

namespace Cake.Common.NuSpec
{
    /// <summary>
    /// Contains settings for the nuspec />.
    /// </summary>
    public class NuSpecSettings
    {
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
        /// Gets or sets the package repository data
        /// </summary>
        public NuSpecRepository Repository { get; set; }

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
        /// Gets or sets the package references.
        /// </summary>
        /// <value>The package references.</value>
        public ICollection<NuSpecReference> References { get; set; }

        /// <summary>
        /// Gets or sets the package types.
        /// </summary>
        /// <value>The package types.</value>
        public ICollection<NuSpecPackageType> PackageTypes { get; set; }

        /// <summary>
        /// Gets or sets the framework assemblies.
        /// </summary>
        /// <value>The framework assemblies.</value>
        public ICollection<NuSpecFrameworkAssembly> FrameworkAssemblies { get; set; }

        /// <summary>
        /// Gets or sets the content files.
        /// </summary>
        /// <value>The content files.</value>
        public ICollection<NuSpecContentFile> ContentFiles { get; set; }

        /// <summary>
        /// Gets or sets the package language.
        /// </summary>
        /// <value>The package language.</value>
        public string Language { get; set; }

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
    }
}
