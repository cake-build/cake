// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.GitReleaseManager.Label;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerLabellerFixture : GitReleaseManagerFixture<GitReleaseManagerLabelSettings>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }

        public GitReleaseManagerLabellerFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerLabeller(FileSystem, Environment, ProcessRunner, Tools);
            tool.Label(UserName, Password, Owner, Repository, Settings);
        }
    }
}
