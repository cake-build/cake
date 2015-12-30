using Cake.Common.Tools.Chocolatey.Features;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Features
{
    internal sealed class ChocolateyDisableFeatureFixture : ChocolateyFeatureTogglerFixture
    {
        protected override void RunTool()
        {
            var tool = new ChocolateyFeatureToggler(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.DisableFeature(Name, Settings);
        }
    }
}