using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.XUnit.Tests.Fixtures
{
    public sealed class XUnitRunnerFixture
    {
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public ICakeContext Context { get; set; }

        public XUnitRunnerFixture()
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            FileSystem = Substitute.For<IFileSystem>();

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/xunit.console.clr4.exe").Returns(new[] { (FilePath)"/Working/tools/xunit.console.clr4.exe" });

            Context = Substitute.For<ICakeContext>();
            Context.FileSystem.Returns(FileSystem);
            Context.Environment.Returns(Environment);
            Context.Globber.Returns(Globber);            
        }

        public XUnitRunner CreateRunner()
        {
            return new XUnitRunner(ProcessRunner);
        }
    }
}
