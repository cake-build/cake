using Cake.Common.Tools.NuGet.SetProxy;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public class NuGetSetProxyFixture : NuGetFixture
    {
        public string Url { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public NuGetSetProxySettings Settings { get; set; }

        public NuGetSetProxyFixture()
        {
            Url = "http://a.com";

            Settings = new NuGetSetProxySettings();

            Process.GetStandardOutput()
                .Returns(new string[] { });
        }

        public void GivenUnexpectedOutput()
        {
            Process.GetStandardOutput()
                .Returns(new string[] { "Unknown Command" });
        }

        public void SetProxy()
        {
            var tool = new NuGetSetProxy(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.SetProxy(Url, Username, Password, Settings);
        }
    }
}
