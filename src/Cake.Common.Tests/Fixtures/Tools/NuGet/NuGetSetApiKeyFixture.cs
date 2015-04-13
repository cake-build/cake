using Cake.Common.Tools.NuGet.SetApiKey;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public class NuGetSetApiKeyFixture : NuGetFixture
    {
        public string ApiKey { get; set; }
        public string Source { get; set; }
        public NuGetSetApiKeySettings Settings { get; set; }

        public NuGetSetApiKeyFixture()
        {
            ApiKey = "SECRET";
            Source = "http://a.com";
            Settings = new NuGetSetApiKeySettings();

            // Set the standard output.
            Process.GetStandardOutput()
                .Returns(new[] { string.Concat("The API Key '", ApiKey, "' was saved for '", Source, "'.") });
        }

        public void GivenUnexpectedOutput()
        {
            Process.GetStandardOutput()
                .Returns(new string[] { });
        }

        public void SetApiKey()
        {
            var tool = new NuGetSetApiKey(Log, FileSystem, Environment, ProcessRunner, NuGetToolResolver);
            tool.SetApiKey(ApiKey, Source, Settings);
        }
    }
}
