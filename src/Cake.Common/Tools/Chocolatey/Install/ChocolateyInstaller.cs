// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Install
{
    /// <summary>
    /// The Chocolatey package installer used to install Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyInstaller : ChocolateyTool<ChocolateyInstallSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyInstaller(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package configuration file and settings.
        /// </summary>
        /// <param name="packageConfigPath">Path to package configuration to use for install.</param>
        /// <param name="settings">The settings.</param>
        public void InstallFromConfig(FilePath packageConfigPath, ChocolateyInstallSettings settings)
        {
            if (packageConfigPath == null)
            {
                throw new ArgumentNullException(nameof(packageConfigPath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var packageId = packageConfigPath.MakeAbsolute(_environment).FullPath;

            Run(settings, GetArguments(packageId, settings));
        }

        /// <summary>
        /// Installs Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="packageId">The source package id.</param>
        /// <param name="settings">The settings.</param>
        public void Install(string packageId, ChocolateyInstallSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ArgumentNullException(nameof(packageId));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(packageId, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageId, ChocolateyInstallSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("install");
            builder.AppendQuoted(packageId);

            AddCommonArguments(settings, builder);

            // Prerelease
            if (settings.Prerelease)
            {
                builder.Append("--pre");
            }

            // Forcex86
            if (settings.Forcex86)
            {
                builder.Append("--x86");
            }

            // Install Arguments
            if (!string.IsNullOrWhiteSpace(settings.InstallArguments))
            {
                builder.Append("--ia");
                builder.AppendQuoted(settings.InstallArguments);
            }

            // Allow Downgrade
            if (settings.AllowDowngrade)
            {
                builder.Append("--allowdowngrade");
            }

            // Ignore Dependencies
            if (settings.IgnoreDependencies)
            {
                builder.Append("-i");
            }

            // Force Dependencies
            if (settings.ForceDependencies)
            {
                builder.Append("-x");
            }

            // User
            if (!string.IsNullOrWhiteSpace(settings.User))
            {
                builder.Append("-u");
                builder.AppendQuoted(settings.User);
            }

            // Password
            if (!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append("-p");
                builder.AppendQuoted(settings.Password);
            }

            // Ignore Checksums
            if (settings.IgnoreChecksums)
            {
                builder.Append("--ignorechecksums");
            }

            return builder;
        }
    }
}