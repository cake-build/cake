using Cake.Common.Tools.XUnit;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class XUnit2RunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }

        public XUnit2RunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/xunit.console.exe").Returns(new[] { (FilePath)"/Working/tools/xunit.console.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/xunit.console.exe")).Returns(defaultToolExist);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }
        }

        public XUnit2Runner CreateRunner()
        {
            return new XUnit2Runner(FileSystem, Environment, Globber, ProcessRunner);
        }
    }
}
