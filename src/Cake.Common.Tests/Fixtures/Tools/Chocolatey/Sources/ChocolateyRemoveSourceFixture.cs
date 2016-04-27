using Cake.Common.Tools.Chocolatey.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Sources
{
    internal sealed class ChocolateyRemoveSourceFixture : ChocolateySourcesFixture
    {
        protected override void RunTool()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.RemoveSource(Name, Settings);
        }
    }
}