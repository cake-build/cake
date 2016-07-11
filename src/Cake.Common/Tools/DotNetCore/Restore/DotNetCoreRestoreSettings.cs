// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Restore
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreRestoreSettings" />.
    /// </summary>
    public sealed class DotNetCoreRestoreSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets the specified NuGet package sources to use during the restore.
        /// </summary>
        public ICollection<string> Sources { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file to use.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets the directory to install packages in.
        /// </summary>
        public DirectoryPath PackagesDirectory { get; set; }

        /// <summary>
        /// Gets or sets a temporary option to allow NuGet to infer RIDs for legacy repositories.
        /// </summary>
        public ICollection<string> InferRuntimes { get; set; }

        /// <summary>
        /// Gets or sets the list of packages sources to use as a fallback.
        /// </summary>
        public ICollection<string> FallbackSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display any output.
        /// </summary>
        public bool Quiet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to do not cache packages and http requests.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force the application to run using an invariant, English-based culture.
        /// </summary>
        public bool ForceEnglishOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to only warning failed sources if there are packages meeting version requirement.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets the verbosity of logging to use.
        /// </summary>
        public DotNetCoreRestoreVerbosity? Verbosity { get; set; }
    }
}
