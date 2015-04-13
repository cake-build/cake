using Cake.Common.Tools.NuGet.Update;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetUpdateFixture : NuGetFixture
    {
        public FilePath TargetFile { get; set; }
        public NuGetUpdateSettings Settings { get; set; }

        public NuGetUpdateFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            TargetFile = "./packages.config";
            Settings = new NuGetUpdateSettings();
        }

        public void Update()
        {
            var tool = new NuGetUpdater(FileSystem, Environment, ProcessRunner, NuGetToolResolver);
            tool.Update(TargetFile, Settings);
        }
    }
}