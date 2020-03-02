// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.GitReleaseManager.Open;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerMilestoneOpenerFixture : GitReleaseManagerFixture<GitReleaseManagerOpenMilestoneSettings>
    {
        public string Token { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string Milestone { get; set; }

        public GitReleaseManagerMilestoneOpenerFixture()
        {
            Token = "token";
            Owner = "repoOwner";
            Repository = "repo";
            Milestone = "0.1.0";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerMilestoneOpener(FileSystem, Environment, ProcessRunner, Tools);

            tool.Open(Token, Owner, Repository, Milestone, Settings);
        }
    }
}
