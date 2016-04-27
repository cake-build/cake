using Cake.Common.Tools.GitLink;
using Cake.Core.IO;
using Cake.Testing.Fixtures;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitLinkFixture : ToolFixture<GitLinkSettings>
    {
        public DirectoryPath RepositoryRootPath { get; set; }

        public GitLinkFixture()
            : base("gitlink.exe")
        {
            RepositoryRootPath = new DirectoryPath("c:/temp");
        }

        protected override void RunTool()
        {
            var tool = new GitLinkRunner(FileSystem, Environment, ProcessRunner, Tools);
            tool.Run(RepositoryRootPath, Settings);
        }
    }
}