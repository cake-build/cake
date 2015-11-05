using Cake.Common.Tests.Properties;
using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer
{
    internal sealed class ChocolateyPackerWithNuSpecFixture : ChocolateyPackerFixture
    {
        public FilePath NuSpecFilePath { get; set; }

        public ChocolateyPackerWithNuSpecFixture()
        {
            NuSpecFilePath = "./existing.nuspec";
            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(Resources.ChocolateyNuspec_NoMetadataValues);
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyPacker(FileSystem, Environment, ProcessRunner, Log, Globber, Resolver);
            tool.Pack(NuSpecFilePath, Settings);
        }
    }
}