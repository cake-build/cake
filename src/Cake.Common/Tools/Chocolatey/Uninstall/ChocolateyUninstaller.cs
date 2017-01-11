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
            var builder = new ProcessArgumentBuilder();

            builder.Append("uninstall");
            foreach (var packageId in packageIds)
            {
                builder.AppendQuoted(packageId);
            }

            // Add common arguments using the inherited method
            AddCommonArguments(settings, builder);

            // Fail On Standard Error
            if (settings.FailOnStandardError)
            {
                builder.Append("--failstderr");
            }

            // Use System Powershell
            if (settings.UseSystemPowershell)
            {
                builder.Append("--use-system-powershell");
            }

            // All Versions
            if (settings.AllVersions)
            {
                builder.Append("--allversions");
            }

            // Global Arguments
            if (settings.GlobalArguments)
            {
                builder.Append("--argsglobal");
            }

            // Global Package Parameters
            if (settings.GlobalPackageParameters)
            {
                builder.Append("--paramsglobal");
            }

            // Force Dependencies
            if (settings.ForceDependencies)
            {
                builder.Append("-x");
            }

            // Ignore Package Codes
            if (settings.IgnorePackageExitCodes)
            {
                builder.Append("--ignorepackageexitcodes");
            }

            // Use Package Exit Codes
            if (settings.UsePackageExitCodes)
            {
                builder.Append("--usepackageexitcodes");
            }

            // Use Auto Uninstaller
            if (settings.UseAutoUninstaller)
            {
                builder.Append("--use-autouninstaller");
            }

            // Skip Auto Uninstaller
            if (settings.SkipAutoUninstaller)
            {
                builder.Append("--skipautouninstaller");
            }

            // Fail on Auto Uninstaller
            if (settings.FailOnAutoUninstaller)
            {
                builder.Append("--failonautouninstaller");
            }

            // Ignore Auto Uninstaller failure
            if (settings.IgnoreAutoUninstaller)
            {
                builder.Append("--ignoreautouninstallerfailure");
            }

            return builder;
        }
    }
}
