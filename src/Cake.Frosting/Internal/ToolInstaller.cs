// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Packaging;
using Cake.Core.Tooling;

namespace Cake.Frosting.Internal
{
    internal sealed class ToolInstaller : IToolInstaller
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolLocator _locator;
        private readonly ICakeConfiguration _configuration;
        private readonly ICakeLog _log;
        private readonly List<IPackageInstaller> _installers;

        public ToolInstaller(
            ICakeEnvironment environment,
            IToolLocator locator,
            ICakeConfiguration configuration,
            ICakeLog log,
            IEnumerable<IPackageInstaller> installers)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _locator = locator ?? throw new ArgumentNullException(nameof(locator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _installers = new List<IPackageInstaller>(installers ?? Enumerable.Empty<IPackageInstaller>());
        }

        public void Install(PackageReference tool)
        {
            // Get the tool path.
            var root = _configuration.GetToolPath(".", _environment);

            // Get the installer.
            var installer = _installers.FirstOrDefault(i => i.CanInstall(tool, PackageType.Tool));
            if (installer == null)
            {
                const string format = "Could not find an installer for the '{0}' scheme.";
                var message = string.Format(CultureInfo.InvariantCulture, format, tool.Scheme);
                throw new FrostingException(message);
            }

            // Install the tool.
            _log.Debug("Installing tool '{0}'...", tool.Package);
            var result = installer.Install(tool, PackageType.Tool, root);
            if (result.Count == 0)
            {
                const string format = "Failed to install tool '{0}'.";
                var message = string.Format(CultureInfo.InvariantCulture, format, tool.Package);
                throw new FrostingException(message);
            }

            // Register the tools.
            foreach (var item in result)
            {
                _locator.RegisterFile(item.Path);
            }
        }
    }
}
