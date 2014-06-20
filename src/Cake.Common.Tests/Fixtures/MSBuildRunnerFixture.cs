using System.Diagnostics;
using Cake.Common.Tools.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    public sealed class MSBuildRunnerFixture
    {
        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }

        public MSBuildRunnerFixture(bool is64BitOperativeSystem = false,
            bool msBuildFileExist = true)
        {
            Process = Substitute.For<IProcess>();

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.Is64BitOperativeSystem().Returns(is64BitOperativeSystem);
            Environment.GetSpecialPath(SpecialPath.ProgramFilesX86).Returns("/Program86");
            Environment.GetSpecialPath(SpecialPath.Windows).Returns("/Windows");
            Environment.WorkingDirectory.Returns("/Working");

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.GetFile(Arg.Is<FilePath>(p => p.FullPath.EndsWith("MSBuild.exe")))
                .Returns(c => {
                    // All requested files exist.
                    var file = Substitute.For<IFile>();
                    file.Exists.Returns(msBuildFileExist);
                    file.Path.Returns(c.Arg<FilePath>());
                    return file;
                });
        }

        public MSBuildRunner CreateRunner()
        {
            return new MSBuildRunner(FileSystem, Environment, ProcessRunner);
        }
    }
}
