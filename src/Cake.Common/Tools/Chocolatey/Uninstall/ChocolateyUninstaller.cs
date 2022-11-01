// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Uninstall
{
    /// <summary>
    /// The Chocolatey package uninstall used to uninstall Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyUninstaller : ChocolateyTool<ChocolateyUninstallSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyUninstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyUninstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Uninstalls Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="packageIds">List of package ids to uninstall.</param>
        /// <param name="settings">The settings.</param>
        public void Uninstall(IEnumerable<string> packageIds, ChocolateyUninstallSettings settings)
        {
            if (packageIds == null)
            {
                throw new ArgumentNullException(nameof(packageIds));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(packageIds, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<string> packageIds, ChocolateyUninstallSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("uninstall");
            foreach (var packageId in packageIds)
            {
                builder.AppendQuoted(packageId);
            }

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Add shared arguments using the inherited method
            AddSharedArguments(settings, builder);

            // All Versions
            if (settings.AllVersions)
            {
                builder.Append("--all-versions");
            }

            // Uninstall Arguments
            if (!string.IsNullOrEmpty(settings.UninstallArguments))
            {
                builder.AppendSwitchQuoted("--uninstall-arguments", separator, settings.UninstallArguments);
            }

            // Force Dependencies
            if (settings.ForceDependencies)
            {
                builder.Append("--force-dependencies");
            }

            // Use Auto Uninstaller
            if (settings.UseAutoUninstaller)
            {
                builder.Append("--use-autouninstaller");
            }

            // Skip Auto Uninstaller
            if (settings.SkipAutoUninstaller)
            {
                builder.Append("--skip-autouninstaller");
            }

            // Fail on Auto Uninstaller
            if (settings.FailOnAutoUninstaller)
            {
                builder.Append("--fail-on-autouninstaller");
            }

            // Ignore Auto Uninstaller failure
            if (settings.IgnoreAutoUninstallerFailure)
            {
                builder.Append("--ignore-autouninstaller-failure");
            }

            if (settings.FromProgramsAndFeatures)
            {
                builder.Append("--from-programs-and-features");
            }

            return builder;
        }
    }
}
