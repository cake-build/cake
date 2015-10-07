using Cake.Common.Tools.Chocolatey.ApiKey;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyApiKeySetterFixture : ChocolateyFixture
    {
        public string ApiKey { get; set; }
        public string Source { get; set; }
        public ChocolateyApiKeySettings Settings { get; set; }

        public ChocolateyApiKeySetterFixture()
        {
            Settings = new ChocolateyApiKeySettings();
            Source = "source1";
            ApiKey = "apikey1";
        }

        public void Set()
        {
            var tool = new ChocolateyApiKeySetter(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.Set(ApiKey, Source, Settings);
        }
    }
}