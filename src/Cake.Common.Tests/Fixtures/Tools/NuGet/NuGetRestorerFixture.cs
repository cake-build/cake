using Cake.Common.Tools.NuGet.Restore;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.NuGet
{
    public sealed class NuGetRestorerFixture : NuGetFixture
    {
        public FilePath TargetFilePath { get; set; }
        public NuGetRestoreSettings Settings { get; set; }

        public NuGetRestorerFixture()
        {
            TargetFilePath = "./project.sln";
            Settings = new NuGetRestoreSettings();
        }

        public void Restore()
        {
            var tool = new NuGetRestorer(FileSystem, Environment, ProcessRunner, Globber, NuGetToolResolver);
            tool.Restore(TargetFilePath, Settings);
        }
    }
}