using Cake.Common.Tools.NuGet.Install;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetInstallerFixture : NuGetFixture
    {
        public string PackageId { get; set; }
        public FilePath PackageConfigPath { get; set; }
        public NuGetInstallSettings Settings { get; set; }

        public NuGetInstallerFixture()
        {
            PackageId = "Cake";
            PackageConfigPath = "./packages.config";
            Settings = new NuGetInstallSettings();
        }

        public void Install()
        {
            var tool = new NuGetInstaller(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.Install(PackageId, Settings);
        }

        public void InstallFromConfig()
        {
            var tool = new NuGetInstaller(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.InstallFromConfig(PackageConfigPath, Settings);
        }
    }
}