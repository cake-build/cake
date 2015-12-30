﻿using Cake.Common.Tools.GitReleaseManager.Close;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerMilestoneCloserFixture : GitReleaseManagerFixture<GitReleaseManagerCloseMilestoneSettings>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string Milestone { get; set; }

        public GitReleaseManagerMilestoneCloserFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            Milestone = "0.1.0";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerMilestoneCloser(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.Close(UserName, Password, Owner, Repository, Milestone, Settings);
        }
    }
}