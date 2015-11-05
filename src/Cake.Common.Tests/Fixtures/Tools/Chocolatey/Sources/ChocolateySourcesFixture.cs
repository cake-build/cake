using Cake.Common.Tools.Chocolatey.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Sources
{
    internal abstract class ChocolateySourcesFixture : ChocolateyFixture<ChocolateySourcesSettings>
    {
        public string Name { get; set; }

        protected ChocolateySourcesFixture()
        {
            Name = "name";
        }
    }
}