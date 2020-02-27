// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.GitReleaseManager.Discard;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerDiscarderFixture : GitReleaseManagerFixture<GitReleaseManagerDiscardSettings>
    {
        public string Token { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }

        public string Milestone { get; set; }

        public GitReleaseManagerDiscarderFixture()
        {
            Token = "token";
            Owner = "repoOwner";
            Repository = "repo";
            Milestone = "0.1.0";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerDiscarder(FileSystem, Environment, ProcessRunner, Tools);

            tool.Discard(Token, Owner, Repository, Milestone, Settings);
        }
    }
}
