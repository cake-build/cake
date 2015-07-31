using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class InspectCodeRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }

        public IProcess Process { get; set; }

        public IProcessRunner ProcessRunner { get; set; }

        public ICakeEnvironment Environment { get; set; }

        public IGlobber Globber { get; set; }

        public string ProcessArguments { get; set; }

        public InspectCodeRunnerFixture()
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            ProcessRunner.Start(
               Arg.Any<FilePath>(),
               Arg.Do<ProcessSettings>(p => ProcessArguments = p.Arguments.Render())
            );

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/inspectcode.exe").Returns(new[] { (FilePath)"/Working/tools/inspectcode.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/inspectcode.exe")).Returns(true);
        }

        public InspectCodeRunner CreateRunner()
        {
            return new InspectCodeRunner(FileSystem, Environment, ProcessRunner, Globber);
        }
    }
}