using Cake.Common.Tools.NuGet.Update;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing.Fakes;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures
{
    internal sealed class NuGetUpdateFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }
        public IToolResolver NuGetToolResolver { get; set; }

        public FilePath TargetFile { get; set; }
        public NuGetUpdateSettings Settings { get; set; }

        public NuGetUpdateFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            TargetFile = "./packages.config";
            Settings = new NuGetUpdateSettings();

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            NuGetToolResolver = Substitute.For<IToolResolver>();
            NuGetToolResolver.Name.Returns("NuGet");
            NuGetToolResolver.ResolveToolPath().Returns(defaultToolExist
                ? "/Working/tools/NuGet.exe" : "/NonWorking/tools/NuGet.exe");

            FileSystem = new FakeFileSystem(true);

            // Got a default tool?
            if (defaultToolExist)
            {
                FileSystem.GetCreatedFile("/Working/tools/NuGet.exe");
            }

            // Custom tool path?
            if (toolPath != null)
            {
                FileSystem.GetCreatedFile(toolPath);
            }
        }

        public void RunUpdater()
        {
            var updater = new NuGetUpdater(FileSystem, Environment, ProcessRunner, NuGetToolResolver);
            updater.Update(TargetFile, Settings);
        }
    }
}