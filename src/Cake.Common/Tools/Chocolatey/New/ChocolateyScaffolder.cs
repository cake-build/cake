// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
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
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(packageId, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageId, ChocolateyNewSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("new");
            builder.AppendQuoted(packageId);

            AddCommonArguments(settings, builder);

            // Package version
            if (!string.IsNullOrWhiteSpace(settings.PackageVersion))
            {
                builder.AppendSwitch("packageversion", "=", settings.PackageVersion.Quote());
            }

            // Owner
            if (!string.IsNullOrWhiteSpace(settings.MaintainerName))
            {
                builder.AppendSwitch("maintainername", "=", settings.MaintainerName.Quote());
            }

            // Package source repository
            if (!string.IsNullOrWhiteSpace(settings.MaintainerRepo))
            {
                builder.AppendSwitch("maintainerrepo", "=", settings.MaintainerRepo.Quote());
            }

            // Installer type
            if (!string.IsNullOrWhiteSpace(settings.InstallerType))
            {
                builder.AppendSwitch("installertype", "=", settings.InstallerType.Quote());
            }

            // URL
            if (!string.IsNullOrWhiteSpace(settings.Url))
            {
                builder.AppendSwitch("url", "=", settings.Url.Quote());
            }

            // URL64
            if (!string.IsNullOrWhiteSpace(settings.Url64))
            {
                builder.AppendSwitch("url64", "=", settings.Url64.Quote());
            }

            // Silent arguments
            if (!string.IsNullOrWhiteSpace(settings.SilentArgs))
            {
                builder.AppendSwitch("silentargs", "=", settings.SilentArgs.Quote());
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--outputdirectory");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Additional properties
            foreach (var propertyValue in settings.AdditionalPropertyValues)
            {
                builder.AppendSwitch(propertyValue.Key, "=", propertyValue.Value.Quote());
            }

            return builder;
        }
    }
}