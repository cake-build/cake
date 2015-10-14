using Cake.Common.Tools.GitReleaseManager.Export;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    public sealed class GitReleaseManagerExporterFixture : GitReleaseManagerFixture
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public FilePath FileOutputPath { get; set; }
        public GitReleaseManagerExportSettings Settings { get; set; }

        public GitReleaseManagerExporterFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            FileOutputPath = "c:/temp";
            Settings = new GitReleaseManagerExportSettings();
        }

        public void Export()
        {
            var tool = new GitReleaseManagerExporter(FileSystem, Environment, ProcessRunner, Globber, GitReleaseManagerToolResolver);
            tool.Export(UserName, Password, Owner, Repository, FileOutputPath, Settings);
        }
    }
}