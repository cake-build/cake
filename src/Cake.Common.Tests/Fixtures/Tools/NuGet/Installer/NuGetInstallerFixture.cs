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