using Cake.Common.Tools.Cake;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class CakeRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public FilePath ScriptPath { get; set; }
        public CakeSettings Settings { get; set; }
        public CakeRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true, bool scriptExist = true)
        {
            ScriptPath = "/Working/build.cake";

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/Cake.exe").Returns(new[] { (FilePath)"/Working/tools/Cake.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/Cake.exe")).Returns(defaultToolExist);
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/build.cake")).Returns(scriptExist);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }

            Settings = new CakeSettings();
        }

        public void Run()
        {
            var runner = new CakeRunner(FileSystem, Environment, Globber, ProcessRunner);
            runner.ExecuteScript(ScriptPath, Settings);
        }
    }
}
