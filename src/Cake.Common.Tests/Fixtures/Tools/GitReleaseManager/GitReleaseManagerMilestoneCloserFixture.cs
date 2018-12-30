// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.GitReleaseManager.Close;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerMilestoneCloserFixture : GitReleaseManagerFixture<GitReleaseManagerCloseMilestoneSettings>
    {
        private bool _useToken = false;

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string Milestone { get; set; }

        public GitReleaseManagerMilestoneCloserFixture()
        {
            UserName = "bob";
            Password = "password";
            Token = "token";
            Owner = "repoOwner";
            Repository = "repo";
            Milestone = "0.1.0";
        }

        public void UseToken()
        {
            _useToken = true;
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerMilestoneCloser(FileSystem, Environment, ProcessRunner, Tools);

            if (_useToken)
            {
                tool.Close(Token, Owner, Repository, Milestone, Settings);
            }
            else
            {
                tool.Close(UserName, Password, Owner, Repository, Milestone, Settings);
            }
        }
    }
}
