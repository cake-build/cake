using Cake.Core.IO;
using Cake.Core.Tooling;
using NSubstitute;

namespace Cake.Testing.Shared
{
    public abstract class ToolFixture<TToolSettings, TFixtureResult>
        where TToolSettings : ToolSettings, new()
        where TFixtureResult : ToolFixtureResult
    {
        private readonly string _toolFilename;

        public FakeFileSystem FileSystem { get; set; }
        public IProcess Process { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public FakeEnvironment Environment { get; set; }
        public IGlobber Globber { get; set; }
        public TToolSettings Settings { get; set; }

        protected ToolFixture(string toolFilename)
        {
            _toolFilename = toolFilename;

            Settings = new TToolSettings();

            Process = Substitute.For<IProcess>();
            Process.GetExitCode().Returns(0);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(Process);

            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Globber = new Globber(FileSystem, Environment);

            // Create the default tool path.
            FileSystem.CreateFile(GetDefaultToolPath());
        }

        protected virtual FilePath GetDefaultToolPath()
        {
            return new FilePath("./tools/" + _toolFilename).MakeAbsolute(Environment);
        }

        public void GivenDefaultToolDoNotExist()
        {
            var path = GetDefaultToolPath();
            if (FileSystem.Exist(path))
            {
                FileSystem.GetFile(path).Delete();
            }
        }

        public void GivenSettingsToolPathExist()
        {
            if (Settings.ToolPath != null)
            {
                var path = Settings.ToolPath.MakeAbsolute(Environment);
                FileSystem.CreateFile(path);
            }
        }

        public void GivenProcessCannotStart()
        {
            ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
        }

        public void GivenProcessExitsWithCode(int exitCode)
        {
            Process.GetExitCode().Returns(exitCode);
        }

        public TFixtureResult Run()
        {
            TFixtureResult result = null;
            if (ProcessRunner != null)
            {
                // Intercept the arguments.
                ProcessRunner
                    .When(s => s.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()))
                    .Do(info =>
                    {
                        result = CreateResult(
                            info.Arg<FilePath>(),
                            info.Arg<ProcessSettings>());
                    });
            }

            // Run the tool.
            RunTool();

            // Returned the captured result.
            return result;
        }

        protected abstract TFixtureResult CreateResult(FilePath toolPath, ProcessSettings process);
        protected abstract void RunTool();
    }
}