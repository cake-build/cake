// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.Chocolatey.Uninstall;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Uninstaller
{
    internal sealed class ChocolateyUninstallerFixture : ChocolateyFixture<ChocolateyUninstallSettings>
    {
        public IEnumerable<string> PackageIds { get; set; }

        public ChocolateyUninstallerFixture()
        {
            PackageIds = new[] { "Cake" };
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyUninstaller(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Uninstall(PackageIds, Settings);
        }
    }
}
