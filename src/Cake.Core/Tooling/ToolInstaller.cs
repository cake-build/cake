// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Installs tools using the most suitable <see cref="IPackageInstaller"/>
    /// and registers the installed files with the <see cref="IToolLocator"/>.
    /// </summary>
    public sealed class ToolInstaller : IToolInstaller
    {
        private readonly IToolLocator _locator;
        private readonly ICakeLog _log;
        private readonly DirectoryPath _toolPath;
        private readonly IPackageInstaller[] _installers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolInstaller"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="locator">The tool locator.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="log">The log.</param>
        /// <param name="installers">The available package installers.</param>
        public ToolInstaller(
            ICakeEnvironment environment,
            IToolLocator locator,
            ICakeConfiguration configuration,
            ICakeLog log,
            IEnumerable<IPackageInstaller> installers)
        {
            ArgumentNullException.ThrowIfNull(environment);
            ArgumentNullException.ThrowIfNull(locator);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(log);

            _locator = locator;
            _log = log;
            _toolPath = configuration.GetToolPath(environment.WorkingDirectory, environment);
            _installers = (installers ?? Enumerable.Empty<IPackageInstaller>()).ToArray();
        }

        /// <inheritdoc/>
        public IEnumerable<FilePath> Install(PackageReference tool)
        {
            ArgumentNullException.ThrowIfNull(tool);

            var installer = _installers.FirstOrDefault(i => i.CanInstall(tool, PackageType.Tool))
                ?? throw new CakeException($"Could not find an installer for the '{tool.Scheme}' scheme.");

            _log.Debug("Installing tool '{0}'...", tool.Package);
            var result = installer.Install(tool, PackageType.Tool, _toolPath);
            if (result.Count == 0)
            {
                throw new CakeException($"Failed to install tool '{tool.Package}'.");
            }

            foreach (var item in result)
            {
                _locator.RegisterFile(item.Path);
                yield return item.Path;
            }
        }
    }
}
