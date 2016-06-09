// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.NuGet.SetProxy;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.SetProxy
{
    internal class NuGetSetProxyFixture : NuGetFixture<NuGetSetProxySettings>
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public NuGetSetProxyFixture()
        {
            Url = "http://a.com";
        }

        public void GivenUnexpectedOutput()
        {
            ProcessRunner.Process.SetStandardOutput(new[] { "Unknown Command" });
        }

        protected override void RunTool()
        {
            var tool = new NuGetSetProxy(FileSystem, Environment, ProcessRunner, Tools, Resolver);
            tool.SetProxy(Url, Username, Password, Settings);
        }
    }
}
