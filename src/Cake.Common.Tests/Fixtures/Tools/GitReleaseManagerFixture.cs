using Cake.Common.Tools.GitReleaseManager;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    public abstract class GitReleaseManagerFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }
        public ICakeLog Log { get; set; }
        public IGitReleaseManagerToolResolver GitReleaseManagerToolResolver { get; set; }
        public IGlobber Globber { get; set; }

        protected GitReleaseManagerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/gitreleasemanager.exe").Returns(new[] { (FilePath)"/Working/tools/GitReleaseManager.exe" });
            Globber.Match("./tools/**/GitReleaseManager.exe").Returns(new[] { (FilePath)"/Working/tools/GitReleaseManager.exe" });

            GitReleaseManagerToolResolver = Substitute.For<IGitReleaseManagerToolResolver>();

            Log = Substitute.For<ICakeLog>();
            FileSystem = new FakeFileSystem(Environment);

            // By default, there is a default tool.
            GitReleaseManagerToolResolver.ResolvePath().Returns("/Working/tools/GitReleaseManager.exe");
            FileSystem.CreateFile("/Working/tools/GitReleaseManager.exe");

            // Set standard output.
            Process.GetStandardOutput().Returns(new string[0]);
        }

        public void GivenCustomToolPathExist(FilePath toolPath)
        {
            FileSystem.CreateFile(toolPath);
        }

        public void GivenDefaultToolDoNotExist()
        {
            var toolPath = new FilePath("/Working/tools/GitReleaseManager.exe");
            FileSystem.EnsureFileDoNotExist(toolPath);
            GitReleaseManagerToolResolver.ResolvePath().Returns("/NonWorking/tools/GitReleaseManager.exe");
        }

        public void GivenProcessCannotStart()
        {
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
        }

        public void GivenProcessReturnError()
        {
            Process.GetExitCode().Returns(1);
        }
    }
}