// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains settings used by <see cref="OctopusDeployReleaseCreator.CreateRelease"/>.
    /// </summary>
    public sealed class CreateReleaseSettings : OctopusDeploySettings
    {
        /// <summary>
        /// Gets or sets the release number to use for the new release.
        /// </summary>
        public string ReleaseNumber { get; set; }

        /// <summary>
        /// Gets or sets the default version number of all packages to use the new release.
        /// </summary>
        public string DefaultPackageVersion { get; set; }

        /// <summary>
        /// Gets or sets the version number to use for a package in the release.
        /// </summary>
        public Dictionary<string, string> Packages { get; set; }

        /// <summary>
        /// Gets or sets the folder containing NuGet packages.
        /// </summary>
        public FilePath PackagesFolder { get; set; }

        /// <summary>
        /// Gets or sets the release notes for the new release.
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the path to a file that contains Release Notes for the new release.
        /// </summary>
        public FilePath ReleaseNotesFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Ignore Existing flag.
        /// </summary>
        public bool IgnoreExisting { get; set; }
    }
}
