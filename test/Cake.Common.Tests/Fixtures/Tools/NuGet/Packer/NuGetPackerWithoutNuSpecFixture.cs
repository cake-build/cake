using Cake.Common.Tools.NuGet.Pack;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Packer
{
    internal sealed class NuGetPackerWithoutNuSpecFixture : NuGetPackerFixture
    {
        protected override void RunTool()
        {
            var tool = new NuGetPacker(FileSystem, Environment, ProcessRunner, Log, Globber, Resolver);
            tool.Pack(Settings);
        }
    }
}