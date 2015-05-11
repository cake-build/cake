using Cake.Common.Tools.OctopusDeploy;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    public sealed class OctopusDeployReleaseCreatorFixture
    {
        public string ProjectName { get; set; }
        public string Server { get; set; }
        public string ApiKey { get; set; }
        public CreateReleaseSettings Settings { get; set; }

        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public IProcess Process { get; set; }

        public OctopusDeployReleaseCreatorFixture(FilePath toolPath = null, bool defaultToolExist = true)
        {
            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("/Working");

            Globber = Substitute.For<IGlobber>();
            Globber.Match("./tools/**/Octo.exe").Returns(new[] { (FilePath)"/Working/tools/Octo.exe" });

            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == "/Working/tools/Octo.exe")).Returns(defaultToolExist);

            if (toolPath != null)
            {
                FileSystem.Exist(Arg.Is<FilePath>(a => a.FullPath == toolPath.FullPath)).Returns(true);
            }

            ProjectName = "testProject";
            Server = "http://octopus";
            ApiKey = "API-12345";
            Settings = new CreateReleaseSettings();
        }

        public void CreateRelease()
        {
            var tool = new OctopusDeployReleaseCreator(FileSystem, Environment, Globber, ProcessRunner);
            tool.CreateRelease(ProjectName, Server, ApiKey, Settings);
        }

        public void GivenProcessCannotStart()
        {
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
        }

        public void GivenProcessReturnError()
        {
            Process.GetExitCode().Returns(1);
        }

        public string GetDefaultArguments()
        {
            return string.Format("create-release --project \"{0}\" --server {1} --apiKey {2}", ProjectName, Server, ApiKey);
        }
    }
}

