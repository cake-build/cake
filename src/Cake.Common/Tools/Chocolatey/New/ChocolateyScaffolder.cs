// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.New
{
    /// <summary>
    /// The Chocolatey project scaffolder used to generate package specification files for a new package.
    /// </summary>
    public sealed class ChocolateyScaffolder : ChocolateyTool<ChocolateyNewSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyScaffolder"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyScaffolder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Generate package specification files for a new package.
        /// </summary>
        /// <param name="packageId">Id of the new package.</param>
        /// <param name="settings">The settings.</param>
        public void CreatePackage(string packageId, ChocolateyNewSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetArguments(packageId, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageId, ChocolateyNewSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("new");
            builder.AppendQuoted(packageId);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Automatic Packages
            if (settings.AutomaticPackage)
            {
                builder.Append("--automaticpackage");
            }

            // Template Name
            if (!string.IsNullOrEmpty(settings.TemplateName))
            {
                builder.AppendSwitchQuoted("--template-name", separator, settings.TemplateName);
            }

            // Package version
            if (!string.IsNullOrWhiteSpace(settings.PackageVersion))
            {
                builder.AppendSwitchQuoted("packageversion", separator, settings.PackageVersion);
            }

            // Owner
            if (!string.IsNullOrWhiteSpace(settings.MaintainerName))
            {
                builder.AppendSwitchQuoted("maintainername", separator, settings.MaintainerName);
            }

            // Package source repository
            if (!string.IsNullOrWhiteSpace(settings.MaintainerRepo))
            {
                builder.AppendSwitchQuoted("maintainerrepo", separator, settings.MaintainerRepo);
            }

            // Installer type
            if (!string.IsNullOrWhiteSpace(settings.InstallerType))
            {
                builder.AppendSwitchQuoted("installertype", separator, settings.InstallerType);
            }

            // URL
            if (!string.IsNullOrWhiteSpace(settings.Url))
            {
                builder.AppendSwitchQuoted("url", separator, settings.Url);
            }

            // URL64
            if (!string.IsNullOrWhiteSpace(settings.Url64))
            {
                builder.AppendSwitchQuoted("url64", separator, settings.Url64);
            }

            // Silent arguments
            if (!string.IsNullOrWhiteSpace(settings.SilentArgs))
            {
                builder.AppendSwitchQuoted("silentargs", separator, settings.SilentArgs);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.AppendSwitchQuoted("--output-directory", separator, settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Built In Template
            if (settings.BuiltInTemplate)
            {
                builder.Append("--built-in-template");
            }

            // Additional properties
            foreach (var propertyValue in settings.AdditionalPropertyValues)
            {
                builder.AppendSwitchQuoted(propertyValue.Key, separator, propertyValue.Value);
            }

            // File
            if (!string.IsNullOrEmpty(settings.File))
            {
                builder.AppendSwitchQuoted("--file", separator, settings.File);
            }

            // File 64
            if (!string.IsNullOrEmpty(settings.File64))
            {
                builder.AppendSwitchQuoted("--file64", separator, settings.File64);
            }

            // Use Original Files Location
            if (settings.UseOriginalFilesLocation)
            {
                builder.Append("--use-original-files-location");
            }

            // Checksum
            if (!string.IsNullOrEmpty(settings.Checksum))
            {
                builder.AppendSwitchQuoted("--download-checksum", separator, settings.Checksum);
            }

            // Checksum 64
            if (!string.IsNullOrEmpty(settings.Checksum64))
            {
                builder.AppendSwitchQuoted("--download-checksum-x64", separator, settings.Checksum64);
            }

            // Checksum Type
            if (!string.IsNullOrEmpty(settings.ChecksumType))
            {
                builder.AppendSwitchQuoted("--download-checksum-type", separator, settings.ChecksumType);
            }

            // Pause On Error
            if (settings.PauseOnError)
            {
                builder.Append("--pause-on-error");
            }

            // Build Package
            if (settings.BuildPackage)
            {
                builder.Append("--build-package");
            }

            // Generate Packages From Installed Software
            if (settings.GeneratePackagesFromInstalledSoftware)
            {
                builder.Append("--from-programs-and-features");
            }

            // Remove Architecture From Name
            if (settings.RemoveArchitectureFromName)
            {
                builder.Append("--remove-architecture-from-name");
            }

            // Include Architecture In Package Name
            if (settings.IncludeArchitectureInPackageName)
            {
                builder.Append("--include-architecture-in-name");
            }

            return builder;
        }
    }
}