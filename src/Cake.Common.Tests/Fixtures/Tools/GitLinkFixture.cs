using Cake.Common.Tools.GitLink;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class GitLinkFixture : ToolFixture<GitLinkSettings>
    {
        private readonly ICakeLog Log;

        public DirectoryPath RepositoryRootPath { get; set; }

        public GitLinkFixture()
            : base("gitlink.exe")
        {
            RepositoryRootPath = new DirectoryPath("c:/temp");

            Log = Substitute.For<ICakeLog>();
        }

        protected override void RunTool()
        {
            var tool = new GitLinkRunner(FileSystem, Environment, ProcessRunner, Globber, Log);
            tool.Run(RepositoryRootPath, Settings);
        }
    }
}