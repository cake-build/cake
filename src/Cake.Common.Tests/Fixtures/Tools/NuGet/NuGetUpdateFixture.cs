using Cake.Common.Tools.NuGet.Update;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetUpdateFixture : NuGetFixture
    {
        public FilePath TargetFile { get; set; }
        public NuGetUpdateSettings Settings { get; set; }

        public NuGetUpdateFixture()
        {
            TargetFile = "./packages.config";
            Settings = new NuGetUpdateSettings();
        }

        public void Update()
        {
            var tool = new NuGetUpdater(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.Update(TargetFile, Settings);
        }
    }
}