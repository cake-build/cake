using Cake.Common.Tools.Fixie;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class FixieRunnerFixture
    {
        public IProcess Process { get; set; }
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IGlobber Globber { get; set; }

        public FixieRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/Fixie.Console.exe").Returns(new[] { (FilePath)"/Working/tools/Fixie.Console.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/Fixie.Console.exe")).Returns(defaultToolExist);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }
        }

        public FixieRunner CreateRunner()
        {
            return new FixieRunner(FileSystem, Environment, ProcessRunner, Globber);
        }
    }
}