// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains the settings used by OctoPack.
    /// </summary>
    public sealed class OctopusPackSettings : OctopusDeployToolSettings
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the package format.
        /// </summary>
        public OctopusPackFormat Format { get; set; }

        /// <summary>
        /// Gets or sets the folder into which the package will be written. Defaults to the current folder.
        /// </summary>
        public DirectoryPath OutFolder { get; set; }

        /// <summary>
        /// Gets or sets the root folder containing files and folders to pack. Defaults to the current folder.
        /// </summary>
        public DirectoryPath BasePath { get; set; }

        /// <summary>
        /// Gets or sets the author. Only applies to NuGet packages.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the title. Only applies to NuGet packages.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description. Only applies to NuGet packages.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the release notes. Only applies to NuGet packages.
        /// </summary>
        public string ReleaseNotes { get; set; }

        /// <summary>
        /// Gets or sets the release notes file. Only applies to NuGet packages.
        /// </summary>
        public FilePath ReleaseNotesFile { get; set; }

        /// <summary>
        /// Gets or sets the file patterns to include. If none are specified, defaults to **.
        /// </summary>
        public ICollection<string> Include { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether to allow an existing package with the same ID/version to be overwritten.
        /// </summary>
        public bool Overwrite { get; set; }
    }
}