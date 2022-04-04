// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Workload.Uninstall
{
    /// <summary>
    /// .NET workloads uninstaller.
    /// </summary>
    public sealed class DotNetWorkloadUninstaller : DotNetTool<DotNetWorkloadUninstallSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetWorkloadUninstaller" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetWorkloadUninstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Uninstalls one or more workloads.
        /// </summary>
        /// <param name="workloadIds">The workload ID or multiple IDs to uninstall.</param>
        public void Uninstall(IEnumerable<string> workloadIds)
        {
            if (workloadIds == null || !workloadIds.Any())
            {
                throw new ArgumentNullException(nameof(workloadIds));
            }

            var settings = new DotNetWorkloadUninstallSettings();
            RunCommand(settings, GetArguments(workloadIds, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<string> workloadIds, DotNetWorkloadUninstallSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("workload uninstall");

            if (workloadIds != null && workloadIds.Any())
            {
                builder.Append(string.Join(" ", workloadIds));
            }

            return builder;
        }
    }
}
