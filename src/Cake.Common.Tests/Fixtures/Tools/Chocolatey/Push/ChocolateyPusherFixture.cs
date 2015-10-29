using Cake.Common.Tools.Chocolatey.Push;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey.Push
{
    internal sealed class ChocolateyPusherFixture : ChocolateyFixture<ChocolateyPushSettings>
    {
        public FilePath PackageFilePath { get; set; }

        public ChocolateyPusherFixture()
        {
            PackageFilePath = "./existing.nupkg";
        }

        protected override void RunTool()
        {
            var tool = new ChocolateyPusher(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.Push(PackageFilePath, Settings);
        }
    }
}