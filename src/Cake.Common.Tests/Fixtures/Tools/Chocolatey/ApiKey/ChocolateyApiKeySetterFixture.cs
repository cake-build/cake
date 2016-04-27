using Cake.Common.Tools.Chocolatey.ApiKey;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.ApiKey
{
    internal sealed class ChocolateyApiKeySetterFixture : ChocolateyFixture<ChocolateyApiKeySettings>
    {
        public string ApiKey { get; set; }
        public string Source { get; set; }

        public ChocolateyApiKeySetterFixture()
        {
            Source = "source1";
            ApiKey = "apikey1";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyApiKeySetter(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.Set(ApiKey, Source, Settings);
        }
    }
}