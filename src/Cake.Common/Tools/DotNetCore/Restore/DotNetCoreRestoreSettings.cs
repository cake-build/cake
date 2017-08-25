// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNetCore.MSBuild;
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
        /// Gets or sets a value indicating whether to do not cache packages and http requests.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable restoring multiple projects in parallel.
        /// </summary>
        public bool DisableParallel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to only warning failed sources if there are packages meeting version requirement.
        /// </summary>
        public bool IgnoreFailedSources { get; set; }

        /// <summary>
        /// Gets or sets the target runtime to restore packages for.
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore project to project references and restore only the root project.
        /// </summary>
        public bool NoDependencies { get; set; }

        /// <summary>
        /// Gets or sets additional arguments to be passed to MSBuild.
        /// </summary>
        public DotNetCoreMSBuildSettings MSBuildSettings { get; set; }
    }
}
