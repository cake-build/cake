using Cake.Common.Tools.Chocolatey.Pack;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Packer
{
    internal abstract class ChocolateyPackerFixture : ChocolateyFixture<ChocolateyPackSettings, ChocolateyPackerFixtureResult>
    {
        public void WithNuSpecXml(string xml)
        {
            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(xml);
        }

        public void GivenTemporaryNuSpecAlreadyExist()
        {
            FileSystem.CreateFile("/Working/existing.temp.nuspec");
        }

        protected override ChocolateyPackerFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new ChocolateyPackerFixtureResult(FileSystem, path, process);
        }
    }
}