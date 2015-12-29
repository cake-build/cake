using Cake.Common.Tools.NuGet.Pack;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Packer
{
    internal abstract class NuGetPackerFixture : NuGetFixture<NuGetPackSettings, NuGetPackerFixtureResult>
    {
        public void WithNuSpecXml(string xml)
        {
            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(xml);
        }

        public void GivenTemporaryNuSpecAlreadyExist()
        {
            FileSystem.CreateFile("/Working/existing.temp.nuspec");
        }

        protected override NuGetPackerFixtureResult CreateResult(FilePath path, ProcessSettings process)
        {
            return new NuGetPackerFixtureResult(FileSystem, path, process);
        }
    }
}