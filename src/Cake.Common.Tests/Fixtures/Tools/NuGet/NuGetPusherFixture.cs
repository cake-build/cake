using Cake.Common.Tools.NuGet.Push;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetPusherFixture : NuGetFixture
    {
        public FilePath PackageFilePath { get; set; }
        public NuGetPushSettings Settings { get; set; }

        public NuGetPusherFixture()
        {
            PackageFilePath = "./existing.nupkg";
            Settings = new NuGetPushSettings();
        }

        public void Push()
        {
            var tool = new NuGetPusher(FileSystem, Environment, ProcessRunner, NuGetToolResolver);
            tool.Push(PackageFilePath, Settings);
        }
    }
}
