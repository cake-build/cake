using Cake.Common.Tests.Properties;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet.Restore;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tests.Fakes;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public class NuGetFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }
        public ICakeLog Log { get; set; }

        public NuGetFixture(FilePath toolPath = null, bool defaultToolExist = true, string xml = null)
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/NuGet.exe").Returns(new[] { (FilePath)"/Working/tools/NuGet.exe" });

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Log = Substitute.For<ICakeLog>();

            FileSystem = new FakeFileSystem(true);
            FileSystem.GetCreatedFile("/Working/existing.nuspec", xml ?? Resources.Nuspec_NoMetadataValues);

            if (defaultToolExist)
            {
                FileSystem.GetCreatedFile("/Working/tools/NuGet.exe");
            }
            if (toolPath != null)
            {
                FileSystem.GetCreatedFile(toolPath);
            }
        }

        public NuGetPacker CreatePacker()
        {
            return new NuGetPacker(FileSystem, Environment, Globber, ProcessRunner, Log);
        }

        public NuGetPusher CreatePusher()
        {
            return new NuGetPusher(FileSystem, Environment, Globber, ProcessRunner);
        }

        public NuGetRestorer CreateRestorer()
        {
            return new NuGetRestorer(FileSystem, Environment, Globber, ProcessRunner);
        }
    }
}