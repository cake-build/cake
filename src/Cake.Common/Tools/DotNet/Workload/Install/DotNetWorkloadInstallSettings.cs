// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Workload.Install
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetWorkloadInstaller" />.
    /// </summary>
    public sealed class DotNetWorkloadInstallSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets the NuGet configuration file (nuget.config) to use.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to prevent restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to treat package source failures as warnings.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow prerelease workload manifests.
        /// </summary>
        public bool IncludePreviews { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow the command to stop and wait for user input or action.
        /// For example, to complete authentication.
        /// </summary>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to do not cache packages and http requests.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to skip updating the workload manifests.
        /// The workload manifests define what assets and versions need to be installed for each workload.
        /// </summary>
        public bool SkipManifestUpdate { get; set; }

        /// <summary>
        /// Gets or sets the URI of the NuGet package source to use.
        /// This setting overrides all of the sources specified in the nuget.config files.
        /// </summary>
        public ICollection<string> Source { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the temporary directory used to download and extract NuGet packages (must be secure).
        /// </summary>
        public DirectoryPath TempDir { get; set; }
    }
}
