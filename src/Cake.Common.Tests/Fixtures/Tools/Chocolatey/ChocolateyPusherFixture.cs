using Cake.Common.Tools.Chocolatey.Push;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.Chocolatey
{
    public sealed class ChocolateyPusherFixture : ChocolateyFixture
    {
        public FilePath PackageFilePath { get; set; }
        public ChocolateyPushSettings Settings { get; set; }

        public ChocolateyPusherFixture()
        {
            PackageFilePath = "./existing.nupkg";
            Settings = new ChocolateyPushSettings();
        }

        public void Push()
        {
            var tool = new ChocolateyPusher(FileSystem, Environment, ProcessRunner, Globber, ChocolateyToolResolver);
            tool.Push(PackageFilePath, Settings);
        }
    }
}