// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetPackageLister" />.
    /// </summary>
    public sealed class DotNetPackageListSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets the NuGet configuration file (nuget.config) to use.
        /// Requires the <see cref="Outdated"/> option.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display packages that have been deprecated.
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to list packages that have newer versions available.
        /// </summary>
        public bool Outdated { get; set; }

        /// <summary>
        /// Gets or sets a specific framework to compile.
        /// </summary>
        public string Framework { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display only the packages with a matching major version number when searching for newer packages.
        /// Requires the <see cref="Outdated"/> or <see cref="Deprecated"/> option.
        /// </summary>
        public bool HighestMinor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display only the packages with a matching major and minor version numbers when searching for newer packages.
        /// Requires the <see cref="Outdated"/> or <see cref="Deprecated"/> option.
        /// </summary>
        public bool HighestPatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to list packages with prerelease versions when searching for newer packages.
        /// Requires the <see cref="Outdated"/> or <see cref="Deprecated"/> option.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to list transitive packages, in addition to the top-level packages.
        /// When specifying this option, you get a list of packages that the top-level packages depend on.
        /// </summary>
        public bool Transitive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// For example, to complete authentication. Available since .NET Core 3.0 SDK.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets the URI of the NuGet package source to use.
        /// Requires the <see cref="Outdated"/> or <see cref="Deprecated"/> option.
        /// </summary>
        public ICollection<string> Source { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether to list packages that have known vulnerabilities.
        /// Cannot be combined with <see cref="Outdated"/> or <see cref="Deprecated"/> options.
        /// Nuget.org is the source of information about vulnerabilities.
        /// </summary>
        public bool Vulnerable { get; set; }
    }
}
