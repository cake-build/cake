using Cake.Common.Tests.Properties;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetPackerFixture : NuGetFixture
    {
        public FilePath NuSpecFilePath { get; set; }
        public NuGetPackSettings Settings { get; set; }

        public NuGetPackerFixture()
        {
            NuSpecFilePath = "./existing.nuspec";
            Settings = new NuGetPackSettings();

            FileSystem.GetCreatedFile("/Working/existing.nuspec", Resources.Nuspec_NoMetadataValues);
        }

        public void WithNuSpecXml(string xml)
        {
            FileSystem.GetCreatedFile("/Working/existing.nuspec", xml);
        }

        public void GivenTemporaryNuSpecAlreadyExist()
        {
            FileSystem.GetCreatedFile("/Working/existing.temp.nuspec");
        }

        public void Pack()
        {
            var tool = new NuGetPacker(FileSystem, Environment, ProcessRunner, Log, NuGetToolResolver);
            tool.Pack(NuSpecFilePath, Settings);
        }
    }
}
