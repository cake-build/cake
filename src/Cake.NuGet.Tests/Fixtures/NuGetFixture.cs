using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.NuGet.Tests.Fixtures
{
    public class NuGetFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }

        public NuGetFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/NuGet.exe").Returns(new[] { (FilePath)"/Working/tools/NuGet.exe" });
            
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);
        }

        public NuGetPacker CreatePacker()
        {
            return new NuGetPacker(Environment, Globber, ProcessRunner);
        }
    }
}
