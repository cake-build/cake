using Cake.Common.Tools.Chocolatey.Upgrade;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyUpgraderFixture : ChocolateyFixture
    {
        public string PackageId { get; set; }
        public ChocolateyUpgradeSettings Settings { get; set; }

        public ChocolateyUpgraderFixture()
        {
            PackageId = "Cake";
            Settings = new ChocolateyUpgradeSettings();
        }

        public void Upgrade()
        {
            var tool = new ChocolateyUpgrader(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.Upgrade(PackageId, Settings);
        }
    }
}