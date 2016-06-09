// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.Chocolatey.Upgrade;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Upgrade
{
    internal sealed class ChocolateyUpgraderFixture : ChocolateyFixture<ChocolateyUpgradeSettings>
    {
        public string PackageId { get; set; }

        public ChocolateyUpgraderFixture()
        {
            PackageId = "Cake";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyUpgrader(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Upgrade(PackageId, Settings);
        }
    }
}
