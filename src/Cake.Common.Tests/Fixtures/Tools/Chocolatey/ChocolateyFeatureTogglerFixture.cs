using Cake.Common.Tools.Chocolatey.Features;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyFeatureTogglerFixture : ChocolateyFixture
    {
        public string Name { get; set; }
        public ChocolateyFeatureSettings Settings { get; set; }

        public ChocolateyFeatureTogglerFixture()
        {
            Name = "checkSumFiles";
            Settings = new ChocolateyFeatureSettings();
        }

        public void EnableFeature()
        {
            var tool = new ChocolateyFeatureToggler(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.EnableFeature(Name, Settings);
        }

        public void DisableFeature()
        {
            var tool = new ChocolateyFeatureToggler(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.DisableFeature(Name, Settings);
        }
    }
}