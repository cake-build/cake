using Cake.Common.Tools.Chocolatey.Install;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyInstallerFixture : ChocolateyFixture
    {
        public string PackageId { get; set; }
        public FilePath PackageConfigPath { get; set; }
        public ChocolateyInstallSettings Settings { get; set; }

        public ChocolateyInstallerFixture()
        {
            PackageId = "Cake";
            PackageConfigPath = "./packages.config";
            Settings = new ChocolateyInstallSettings();
        }

        public void Install()
        {
            var tool = new ChocolateyInstaller(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.Install(PackageId, Settings);
        }

        public void InstallFromConfig()
        {
            var tool = new ChocolateyInstaller(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.InstallFromConfig(PackageConfigPath, Settings);
        }
    }
}