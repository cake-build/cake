// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.Packaging;
using Cake.Core.Tooling;

namespace Cake.Frosting.Internal
{
    internal sealed class ToolInstaller : IToolInstaller
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolLocator _locator;
        private readonly List<IPackageInstaller> _installers;

        public ToolInstaller(ICakeEnvironment environment, IToolLocator locator, IEnumerable<IPackageInstaller> installers)
        {
            _environment = environment;
            _locator = locator;
            _installers = new List<IPackageInstaller>(installers ?? Enumerable.Empty<IPackageInstaller>());
        }

        public void Install(PackageReference tool)
        {
            // Get the tool path.
            var root = _environment.WorkingDirectory.Combine("tools").MakeAbsolute(_environment);

            // Get the installer.
            var installer = _installers.FirstOrDefault(i => i.CanInstall(tool, PackageType.Tool));
            if (installer == null)
            {
                const string format = "Could not find an installer for the '{0}' scheme.";
                var message = string.Format(CultureInfo.InvariantCulture, format, tool.Scheme);
                throw new FrostingException(message);
            }

            // Install the tool.
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
