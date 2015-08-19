using Cake.Common.Tools.DupFinder;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class DupFinderRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }

        public IProcess Process { get; set; }

        public IProcessRunner ProcessRunner { get; set; }

        public ICakeEnvironment Environment { get; set; }

        public IGlobber Globber { get; set; }

        public string ProcessArguments { get; set; }

        public DupFinderRunnerFixture()
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
            Globber.Match("./tools/**/dupfinder.exe").Returns(new[] { (FilePath)"/Working/tools/dupfinder.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/dupfinder.exe")).Returns(true);
        }

        public DupFinderRunner CreateRunner()
        {
            return new DupFinderRunner(FileSystem, Environment, ProcessRunner, Globber);
        }
    }
}