using System.Diagnostics;
using Cake.Common.NUnit;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class NUnitRunnerFixture
    {
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }

        public NUnitRunnerFixture()
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/nunit-console.exe").Returns(new[] { (FilePath)"/Working/tools/nunit-console.exe" });
        }

        public NUnitRunner CreateRunner()
        {
            return new NUnitRunner(Environment, Globber, ProcessRunner);
        }
    }
}
