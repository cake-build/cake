// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.SetApiKey;

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
            var tool = new NuGetSetApiKey(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.SetApiKey(ApiKey, Source, Settings);
        }
    }
}
