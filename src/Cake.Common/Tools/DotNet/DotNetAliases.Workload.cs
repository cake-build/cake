// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotNet.Workload.Install;
using Cake.Common.Tools.DotNet.Workload.List;
using Cake.Common.Tools.DotNet.Workload.Repair;
using Cake.Common.Tools.DotNet.Workload.Restore;
using Cake.Common.Tools.DotNet.Workload.Search;
using Cake.Common.Tools.DotNet.Workload.Uninstall;
using Cake.Common.Tools.DotNet.Workload.Update;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.DotNet
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/dotnet/cli">.NET CLI</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, the .NET CLI tools will need to be installed on the machine where
    /// the Cake script is being executed.  See this <see href="https://www.microsoft.com/net/core">page</see> for information
    /// on how to install.
    /// </para>
    /// </summary>
    public static partial class DotNetAliases
    {
        /// <summary>
        /// Lists available workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of available workloads.</returns>
        /// <example>
        /// <code>
        /// var workloads = DotNetWorkloadSearch();
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Id: {workload.Id}, Description: {workload.Description}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Search")]
        public static IEnumerable<DotNetWorkload> DotNetWorkloadSearch(this ICakeContext context)
        {
            return context.DotNetWorkloadSearch(null);
        }

        /// <summary>
        /// Lists available workloads by specifying all or part of the workload ID.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="searchString">The workload ID to search for, or part of it.</param>
        /// <returns>The list of available workloads.</returns>
        /// <example>
        /// <code>
        /// var workloads = DotNetWorkloadSearch("maui");
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Id: {workload.Id}, Description: {workload.Description}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Search")]
        public static IEnumerable<DotNetWorkload> DotNetWorkloadSearch(this ICakeContext context, string searchString)
        {
            return context.DotNetWorkloadSearch(searchString, null);
        }

        /// <summary>
        /// Lists available workloads by specifying all or part of the workload ID.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="searchString">The workload ID to search for, or part of it.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of available workloads.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadSearchSettings
        /// {
        ///     DotNetVerbosity.Detailed
        /// };
        ///
        /// var workloads = DotNetWorkloadSearch("maui", settings);
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Id: {workload.Id}, Description: {workload.Description}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Search")]
        public static IEnumerable<DotNetWorkload> DotNetWorkloadSearch(this ICakeContext context, string searchString, DotNetWorkloadSearchSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadSearchSettings();
            }

            var searcher = new DotNetWorkloadSearcher(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return searcher.Search(searchString, settings);
        }

        /// <summary>
        /// Uninstalls a specified workload.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadId">The workload ID to uninstall.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadUninstall("maui");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Uninstall")]
        public static void DotNetWorkloadUninstall(this ICakeContext context, string workloadId)
        {
            context.DotNetWorkloadUninstall(new string[] { workloadId });
        }

        /// <summary>
        /// Uninstalls one or more workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadIds">The workload ID or multiple IDs to uninstall.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadUninstall(new string[] { "maui", "maui-desktop", "maui-mobile" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Uninstall")]
        public static void DotNetWorkloadUninstall(this ICakeContext context, IEnumerable<string> workloadIds)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var uninstaller = new DotNetWorkloadUninstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            uninstaller.Uninstall(workloadIds);
        }

        /// <summary>
        /// Installs a specified optional workload.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadId">The workload ID to install.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadInstall("maui");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, string workloadId)
        {
            context.DotNetWorkloadInstall(workloadId, null);
        }

        /// <summary>
        /// Installs a specified optional workload.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadId">The workload ID to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadInstallSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadInstall("maui", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, string workloadId, DotNetWorkloadInstallSettings settings)
        {
            context.DotNetWorkloadInstall(new string[] { workloadId }, settings);
        }

        /// <summary>
        /// Installs one or more optional workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadIds">The workload ID or multiple IDs to install.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadInstall(new string[] { "maui", "maui-desktop", "maui-mobile" });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, IEnumerable<string> workloadIds)
        {
            context.DotNetWorkloadInstall(workloadIds, null);
        }

        /// <summary>
        /// Installs one or more optional workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="workloadIds">The workload ID or multiple IDs to install.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadInstallSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadInstall(new string[] { "maui", "maui-desktop", "maui-mobile" }, settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Install")]
        public static void DotNetWorkloadInstall(this ICakeContext context, IEnumerable<string> workloadIds, DotNetWorkloadInstallSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadInstallSettings();
            }

            var installer = new DotNetWorkloadInstaller(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            installer.Install(workloadIds, settings);
        }

        /// <summary>
        /// Lists all installed workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The list of installed workloads.</returns>
        /// <example>
        /// <code>
        /// var workloadIds = DotNetWorkloadList();
        ///
        /// foreach (var workloadId in workloadIds)
        /// {
        ///      Information($"Installed Workload Id: {workloadId}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.List")]
        public static IEnumerable<DotNetWorkloadListItem> DotNetWorkloadList(this ICakeContext context)
        {
            return context.DotNetWorkloadList(null);
        }

        /// <summary>
        /// Lists all installed workloads.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>The list of installed workloads.</returns>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadListSettings
        /// {
        ///     Verbosity = DotNetVerbosity.Detailed
        /// };
        ///
        /// var workloads = DotNetWorkloadList(settings);
        ///
        /// foreach (var workload in workloads)
        /// {
        ///      Information($"Installed Workload Id: {workload.Id}\t Manifest Version: {workload.ManifestVersion}\t Installation Source: {workload.InstallationSource}");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.List")]
        public static IEnumerable<DotNetWorkloadListItem> DotNetWorkloadList(this ICakeContext context, DotNetWorkloadListSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadListSettings();
            }

            var lister = new DotNetWorkloadLister(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return lister.List(settings);
        }

        /// <summary>
        /// Repairs all workloads installations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadRepair();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Repair")]
        public static void DotNetWorkloadRepair(this ICakeContext context)
        {
            context.DotNetWorkloadRepair(null);
        }

        /// <summary>
        /// Repairs all workloads installations.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadRepairSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadRepair(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Repair")]
        public static void DotNetWorkloadRepair(this ICakeContext context, DotNetWorkloadRepairSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadRepairSettings();
            }

            var repairer = new DotNetWorkloadRepairer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            repairer.Repair(settings);
        }

        /// <summary>
        /// Updates all installed workloads to the newest available version.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadUpdate();
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Update")]
        public static void DotNetWorkloadUpdate(this ICakeContext context)
        {
            context.DotNetWorkloadUpdate(null);
        }

        /// <summary>
        /// Updates all installed workloads to the newest available version.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadUpdateSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadUpdate(settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Update")]
        public static void DotNetWorkloadUpdate(this ICakeContext context, DotNetWorkloadUpdateSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotNetWorkloadUpdateSettings();
            }

            var updater = new DotNetWorkloadUpdater(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            updater.Update(settings);
        }

        /// <summary>
        /// Installs workloads needed for a project or a solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution file to install workloads for.</param>
        /// <example>
        /// <code>
        /// DotNetWorkloadRestore("./src/project");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Restore")]
        public static void DotNetWorkloadRestore(this ICakeContext context, string project)
        {
            context.DotNetWorkloadRestore(project, null);
        }

        /// <summary>
        /// Installs workloads needed for a project or a solution.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="project">The project or solution file to install workloads for.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var settings = new DotNetWorkloadRestoreSettings
        /// {
        ///     IncludePreviews = true,
        ///     NoCache = true
        /// };
        ///
        /// DotNetWorkloadRestore("./src/project", settings);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Workload")]
        [CakeNamespaceImport("Cake.Common.Tools.DotNet.Workload.Restore")]
        public static void DotNetWorkloadRestore(this ICakeContext context, string project, DotNetWorkloadRestoreSettings settings)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings is null)
            {
                settings = new DotNetWorkloadRestoreSettings();
            }

            var restorer = new DotNetWorkloadRestorer(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            restorer.Restore(project, settings);
        }
    }
}
