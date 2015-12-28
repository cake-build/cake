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
            var tool = new ChocolateyUpgrader(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.Upgrade(PackageId, Settings);
        }
    }
}