using System.Diagnostics;
using Cake.Common.Tools.XUnit;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class XUnitRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }

        public XUnitRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/xunit.console.clr4.exe").Returns(new[] { (FilePath)"/Working/tools/xunit.console.clr4.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/xunit.console.clr4.exe")).Returns(defaultToolExist);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }
        }

        public XUnitRunner CreateRunner()
        {
            return new XUnitRunner(FileSystem, Environment, Globber, ProcessRunner);
        }
    }
}
