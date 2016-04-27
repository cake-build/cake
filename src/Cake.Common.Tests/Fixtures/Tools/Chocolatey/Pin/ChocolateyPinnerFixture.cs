using Cake.Common.Tools.Chocolatey.Pin;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Pin
{
    internal sealed class ChocolateyPinnerFixture : ChocolateyFixture<ChocolateyPinSettings>
    {
        public string Name { get; set; }

        public ChocolateyPinnerFixture()
        {
            Name = "Cake";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyPinner(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Pin(Name, Settings);
        }
    }
}