using Cake.Common.Tools.MSTest;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class MSTestRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }

        public FilePath ToolPath { get; set; }

        public MSTestRunnerFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            ToolPath = "/ProgramFilesX86/Microsoft Visual Studio 12.0/Common7/IDE/mstest.exe";

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory = "/Working";
            Environment.GetSpecialPath(SpecialPath.ProgramFilesX86).Returns("/ProgramFilesX86");

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(p => p.FullPath.Equals(ToolPath.FullPath))).Returns(defaultToolExist);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }
        }

        public MSTestRunner CreateRunner()
        {
            return new MSTestRunner(FileSystem, Environment, ProcessRunner);
        }
    }
}
