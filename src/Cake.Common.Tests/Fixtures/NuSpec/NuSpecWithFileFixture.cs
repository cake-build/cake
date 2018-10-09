using Cake.Common.NuSpec;
using Cake.Common.Tests.Properties;
using Cake.Core.IO;
using Cake.Testing;

namespace Cake.Common.Tests.Fixtures.NuSpec
{
    internal sealed class NuSpecWithFileFixture : NuSpecFixtureBase
    {
        private FilePath NuSpecFilePath { get; set; }

        public NuSpecWithFileFixture()
        {
            NuSpecFilePath = "/Working/existing.nuspec";
            WithNuSpecXml(Resources.Nuspec_NoMetadataValues);
        }

        public void WithNuSpecXml(string xml)
        {
            FileSystem.CreateFile("/Working/existing.nuspec").SetContent(xml);
        }

        public NuSpecFixtureResult Run()
        {
            var nuSpecProcessor = new NuSpecProcessor(FileSystem, Environment, Log);
            var tmpNuSpecFile = nuSpecProcessor.Process(NuSpecFilePath, Settings);
            return new NuSpecFixtureResult(FileSystem, tmpNuSpecFile);
        }
    }
}