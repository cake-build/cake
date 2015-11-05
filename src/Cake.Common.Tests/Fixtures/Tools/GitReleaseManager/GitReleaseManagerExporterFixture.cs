using Cake.Common.Tools.GitReleaseManager.Export;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerExporterFixture : GitReleaseManagerFixture<GitReleaseManagerExportSettings>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public FilePath FileOutputPath { get; set; }

        public GitReleaseManagerExporterFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            FileOutputPath = "c:/temp";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerExporter(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.Export(UserName, Password, Owner, Repository, FileOutputPath, Settings);
        }
    }
}