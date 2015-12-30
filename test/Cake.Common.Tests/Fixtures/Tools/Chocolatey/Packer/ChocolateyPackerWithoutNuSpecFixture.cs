using Cake.Common.Tools.Chocolatey.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer
{
    internal sealed class ChocolateyPackerWithoutNuSpecFixture : ChocolateyPackerFixture
    {
        protected override void RunTool()
        {
            var tool = new ChocolateyPacker(FileSystem, Environment, ProcessRunner, Log, Globber, Resolver);
            tool.Pack(Settings);
        }
    }
}