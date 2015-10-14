using Cake.Common.Tools.GitReleaseManager.Close;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    public sealed class GitReleaseManagerMilestoneCloserFixture : GitReleaseManagerFixture
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string Milestone { get; set; }
        public GitReleaseManagerCloseMilestoneSettings Settings { get; set; }

        public GitReleaseManagerMilestoneCloserFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            Milestone = "0.1.0";
            Settings = new GitReleaseManagerCloseMilestoneSettings();
        }

        public void Close()
        {
            var tool = new GitReleaseManagerMilestoneCloser(FileSystem, Environment, ProcessRunner, Globber, GitReleaseManagerToolResolver);
            tool.Close(UserName, Password, Owner, Repository, Milestone, Settings);
        }
    }
}