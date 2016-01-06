using Cake.Common.Tools.NuGet.SetApiKey;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.SetApiKey
{
    internal class NuGetSetApiKeyFixture : NuGetFixture<NuGetSetApiKeySettings>
    {
        public string ApiKey { get; set; }
        public string Source { get; set; }

        public NuGetSetApiKeyFixture()
        {
            ApiKey = "SECRET";
            Source = "http://a.com";

            // Set the standard output.
            ProcessRunner.Process.SetStandardOutput(new[] {
                string.Concat("The API Key '", ApiKey,
                    "' was saved for '", Source, "'.")});
        }

        public void GivenUnexpectedOutput()
        {
            ProcessRunner.Process.SetStandardOutput(new string[] { });
        }

        protected override void RunTool()
        {
            var tool = new NuGetSetApiKey(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.SetApiKey(ApiKey, Source, Settings);
        }
    }
}