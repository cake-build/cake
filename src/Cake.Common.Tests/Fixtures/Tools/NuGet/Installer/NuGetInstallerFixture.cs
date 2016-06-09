// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.Install;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Installer
{
    internal sealed class NuGetInstallerFixture : NuGetFixture<NuGetInstallSettings>
    {
        public string PackageId { get; set; }

        public NuGetInstallerFixture()
        {
            PackageId = "Cake";
        }

        protected override void RunTool()
        {
            var tool = new NuGetInstaller(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Install(PackageId, Settings);
        }
    }
}
