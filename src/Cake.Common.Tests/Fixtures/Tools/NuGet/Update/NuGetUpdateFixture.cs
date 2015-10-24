using Cake.Common.Tools.NuGet.Update;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet.Update
{
    internal sealed class NuGetUpdateFixture : NuGetFixture<NuGetUpdateSettings>
    {
        public FilePath TargetFile { get; set; }

        public NuGetUpdateFixture()
        {
            TargetFile = "./packages.config";
        }

        protected override void RunTool()
        {
            var tool = new NuGetUpdater(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.Update(TargetFile, Settings);
        }
    }
}