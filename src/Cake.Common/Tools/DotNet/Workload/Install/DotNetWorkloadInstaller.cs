// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Workload.Install
{
    /// <summary>
    /// .NET workloads installer.
    /// </summary>
    public sealed class DotNetWorkloadInstaller : DotNetTool<DotNetWorkloadInstallSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetWorkloadInstaller" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetWorkloadInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Lists the latest available version of the .NET SDK and .NET Runtime, for each feature band.
        /// </summary>
        /// <param name="workloadIds">The workload ID or multiple IDs to uninstall.</param>
        /// <param name="settings">The settings.</param>
        public void Install(IEnumerable<string> workloadIds, DotNetWorkloadInstallSettings settings)
        {
            if (workloadIds == null || !workloadIds.Any())
            {
                throw new ArgumentNullException(nameof(workloadIds));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(workloadIds, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<string> workloadIds, DotNetWorkloadInstallSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("workload install");

            if (workloadIds != null && workloadIds.Any())
            {
                builder.Append(string.Join(" ", workloadIds));
            }

            // Config File
            if (settings.ConfigFile != null)
            {
                builder.AppendSwitchQuoted("--configfile", settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            // Disable Parallel
            if (settings.DisableParallel)
            {
                builder.Append("--disable-parallel");
            }

            // Ignore Failed Sources
            if (settings.IgnoreFailedSources)
            {
                builder.Append("--ignore-failed-sources");
            }

            // Include Previews
            if (settings.IncludePreviews)
            {
                builder.Append("--include-previews");
            }

            // Interactive
            if (settings.Interactive)
            {
                builder.Append("--interactive");
            }

            // No Cache
            if (settings.NoCache)
            {
                builder.Append("--no-cache");
            }

            // Skip Manifest Update
            if (settings.SkipManifestUpdate)
            {
                builder.Append("--skip-manifest-update");
            }

            // Source
            if (settings.Source != null && settings.Source.Any())
            {
                foreach (var source in settings.Source)
                {
                    builder.AppendSwitchQuoted("--source", source);
                }
            }

            // Temp Dir
            if (settings.TempDir != null)
            {
                builder.AppendSwitchQuoted("--temp-dir", settings.TempDir.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}
