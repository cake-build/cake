// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.Install;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Installer
{
    internal sealed class NuGetInstallerFromConfigFixture : NuGetFixture<NuGetInstallSettings>
    {
        public FilePath PackageConfigPath { get; set; }

        public NuGetInstallerFromConfigFixture()
        {
            PackageConfigPath = new FilePath("./packages.config");
        }

        protected override void RunTool()
        {
            var tool = new NuGetInstaller(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.InstallFromConfig(PackageConfigPath, Settings);
        }
    }
}
