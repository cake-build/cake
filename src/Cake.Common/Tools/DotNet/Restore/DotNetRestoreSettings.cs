// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNet.Restore
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreRestoreSettings" />.
    /// </summary>
    public class DotNetRestoreSettings : DotNetSettings
    {
        /// <summary>
        /// Gets or sets the specified NuGet package sources to use during the restore.
        /// </summary>
        public ICollection<string> Sources { get; set; } = new List<string>();

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
        /// Gets or sets a value indicating whether to force all dependencies to be resolved even if the last restore was successful.
        /// This is equivalent to deleting the project.assets.json file.
        ///
        /// Note: This flag was introduced with the .NET Core 2.x release.
        /// </summary>
        public bool Force { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to stop and wait for user input or action (for example to complete authentication).
        /// </summary>
        /// <remarks>
        /// Supported by .NET SDK version 2.1.400 and above.
        /// </remarks>
        public bool Interactive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to enable project lock file to be generated and used with restore.
        /// </summary>
        /// <remarks>
        /// Supported by .NET SDK version 2.1.500 and above.
        /// </remarks>
        public bool UseLockFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not allow updating project lock file.
        /// </summary>
        /// <remarks>
        /// When set to true, restore will fail if the lock file is out of sync.
        /// Useful for CI builds when you do not want the build to continue if the package closure has changed than what is present in the lock file.
        /// <para>
        /// Supported by .NET SDK version 2.1.500 and above.
        /// </para>
        /// </remarks>
        public bool LockedMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating output location where project lock file is written.
        /// </summary>
        /// <remarks>
        /// If not set, 'dotnet restore' defaults to 'PROJECT_ROOT\packages.lock.json'.
        /// <para>
        /// Supported by .NET SDK version 2.1.500 and above.
        /// </para>
        /// </remarks>
        public FilePath LockFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to force restore to reevaluate all dependencies even if a lock file already exists.
        /// </summary>
        /// <remarks>
        /// Supported by .NET SDK version 2.1.500 and above.
        /// </remarks>
        public bool ForceEvaluate { get; set; }

        /// <summary>
        /// Gets or sets additional arguments to be passed to MSBuild.
        /// </summary>
        public DotNetMSBuildSettings MSBuildSettings { get; set; }
    }
}
