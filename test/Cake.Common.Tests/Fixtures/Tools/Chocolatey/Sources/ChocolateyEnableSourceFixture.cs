using Cake.Common.Tools.Chocolatey.Sources;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Sources
{
    internal sealed class ChocolateyEnableSourceFixture : ChocolateySourcesFixture
    {
        protected override void RunTool()
        {
            var tool = new ChocolateySources(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.EnableSource(Name, Settings);
        }
    }
}