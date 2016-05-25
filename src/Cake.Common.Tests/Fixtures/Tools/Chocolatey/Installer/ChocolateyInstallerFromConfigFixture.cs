using Cake.Common.Tools.Chocolatey.Install;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Installer
{
    internal sealed class ChocolateyInstallFromConfigFixture : ChocolateyFixture<ChocolateyInstallSettings>
    {
        public FilePath PackageConfigPath { get; set; }

        public ChocolateyInstallFromConfigFixture()
        {
            PackageConfigPath = "./packages.config";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyInstaller(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.InstallFromConfig(PackageConfigPath, Settings);
        }
    }
}