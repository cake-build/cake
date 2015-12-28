using Cake.Common.Tools.Chocolatey.Features;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Features
{
    internal abstract class ChocolateyFeatureTogglerFixture : ChocolateyFixture<ChocolateyFeatureSettings>
    {
        public string Name { get; set; }

        protected ChocolateyFeatureTogglerFixture()
        {
            Name = "checkSumFiles";
        }
    }
}