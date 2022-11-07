// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey.New
{
    /// <summary>
    /// Contains settings used by <see cref="ChocolateyScaffolder"/>.
    /// </summary>
    public sealed class ChocolateyNewSettings : ChocolateySettings
    {
        private readonly Dictionary<string, string> _additionalPropertyValues = new Dictionary<string, string>();

        /// <summary>
        /// Gets or sets a value indicating whether to generate automatic package instead or normal.
        /// </summary>
        public bool AutomaticPackage { get; set; }

        /// <summary>
        /// Gets or sets the name of the template to generate new package with.
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// Gets or sets the path where the package will be created.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use original built-in template, rather than an overridden one.
        /// </summary>
        public bool BuiltInTemplate { get; set; }

        /// <summary>
        /// Gets or sets the version of the package to be created.
        /// </summary>
        public string PackageVersion { get; set; }

        /// <summary>
        /// Gets or sets the owner of the package to be created.
        /// </summary>
        public string MaintainerName { get; set; }

        /// <summary>
        /// Gets or sets the repository of the package source.
        /// </summary>
        public string MaintainerRepo { get; set; }

        /// <summary>
        /// Gets or sets the type of the installer.
        /// </summary>
        public string InstallerType { get; set; }

        /// <summary>
        /// Gets or sets the URL where the software to be installed can be downloaded from.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the URL where the 64-Bit version of the software to be installed can be downloaded from.
        /// </summary>
        public string Url64 { get; set; }

        /// <summary>
        /// Gets or sets the arguments for running the installer silently.
        /// </summary>
        public string SilentArgs { get; set; }

        /// <summary>
        /// Gets the list of additional property values which should be passed to the template.
        /// </summary>
        public Dictionary<string, string> AdditionalPropertyValues
        {
            get
            {
                return _additionalPropertyValues;
            }
        }

        /// <summary>
        /// Gets or sets the file or URL to binary used for auto-detection and generation of package.
        /// </summary>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the file or URL to 64-bit binary used for auto-detection and generation of package.
        /// </summary>
        public string File64 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use original location of binary in packaging.
        /// </summary>
        public bool UseOriginalFilesLocation { get; set; }

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
        /// Gets or sets a value indicating whether to pause when there is an error creating a package.
        /// </summary>
        public bool PauseOnError { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to attempt to compile the package after creating it.
        /// </summary>
        public bool BuildPackage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to generate packages from the currenty installed software on a system.
        /// </summary>
        public bool GeneratePackagesFromInstalledSoftware { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remove x86, x64, etc. from generated package id.
        /// </summary>
        public bool RemoveArchitectureFromName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to leave x86, x64, etc. as part of the generated package id.
        /// </summary>
        public bool IncludeArchitectureInPackageName { get; set; }
    }
}