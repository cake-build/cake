using Cake.Common.Tools.Chocolatey.Install;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Installer
{
    internal sealed class ChocolateyInstallFixture : ChocolateyFixture<ChocolateyInstallSettings>
    {
        public string PackageId { get; set; }

        public ChocolateyInstallFixture()
        {
            PackageId = "Cake";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyInstaller(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Install(PackageId, Settings);
        }
    }
}