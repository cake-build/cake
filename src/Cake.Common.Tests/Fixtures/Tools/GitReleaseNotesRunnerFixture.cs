using Cake.Common.Tools.GitReleaseNotes;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitReleaseNotesRunnerFixture : ToolFixture<GitReleaseNotesSettings>
    {
        public FilePath OutputFile;

        public GitReleaseNotesRunnerFixture()
            : base("GitReleaseNotes.exe")
        {
            this.OutputFile = "c:/temp/releasenotes.md";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseNotesRunner(FileSystem, Environment, ProcessRunner, Globber);
            tool.Run(OutputFile, Settings);
        }
    }
}