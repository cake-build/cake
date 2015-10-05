using Cake.Common.Tools.Chocolatey;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    public abstract class ChocolateyFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }
        public ICakeLog Log { get; set; }
        public IChocolateyToolResolver ChocolateyToolResolver { get; set; }
        public IGlobber Globber { get; set; }

        protected ChocolateyFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Globber = Substitute.For<IGlobber>();

            ChocolateyToolResolver = Substitute.For<IChocolateyToolResolver>();

            Log = Substitute.For<ICakeLog>();
            FileSystem = new FakeFileSystem(Environment);

            // By default, there is a default tool.
            ChocolateyToolResolver.ResolvePath().Returns("/Working/tools/choco.exe");
            FileSystem.CreateFile("/Working/tools/choco.exe");

            // Set standard output.
            Process.GetStandardOutput().Returns(new string[0]);
        }

        public void GivenCustomToolPathExist(FilePath toolPath)
        {
            FileSystem.CreateFile(toolPath);
        }

        public void GivenDefaultToolDoNotExist()
        {
            var toolPath = new FilePath("/Working/tools/choco.exe");
            FileSystem.EnsureFileDoNotExist(toolPath);
            ChocolateyToolResolver.ResolvePath().Returns("/NonWorking/tools/choco.exe");
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