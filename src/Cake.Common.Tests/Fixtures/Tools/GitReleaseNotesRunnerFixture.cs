using Cake.Common.Tools.GitReleaseNotes;
using Cake.Core.IO;
using Cake.Testing.Shared;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitReleaseNotesRunnerFixture : ToolFixture<GitReleaseNotesSettings>
    {
        public FilePath OutputFile;

        public GitReleaseNotesRunnerFixture()
            : base("GitReleaseNotes.exe")
        {
            OutputFile = "/temp/releasenotes.md";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseNotesRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(OutputFile, Settings);
        }
    }
}