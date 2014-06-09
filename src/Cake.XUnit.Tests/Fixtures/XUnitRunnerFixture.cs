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
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }

        public XUnitRunnerFixture()
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/xunit.console.clr4.exe").Returns(new[] { (FilePath)"/Working/tools/xunit.console.clr4.exe" });     
        }

        public XUnitRunner CreateRunner()
        {
            return new XUnitRunner(Environment, Globber, ProcessRunner);
        }
    }
}
