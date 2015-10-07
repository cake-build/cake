using Cake.Common.Tools.Chocolatey.Pin;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyPinnerFixture : ChocolateyFixture
    {
        public string Name { get; set; }
        public ChocolateyPinSettings Settings { get; set; }

        public ChocolateyPinnerFixture()
        {
            Settings = new ChocolateyPinSettings();
            Name = "Cake";
        }

        public void Pin()
        {
            var tool = new ChocolateyPinner(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.Pin(Name, Settings);
        }
    }
}