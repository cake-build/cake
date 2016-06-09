// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.GitReleaseManager.Create;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerCreatorFixture : GitReleaseManagerFixture<GitReleaseManagerCreateSettings>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }

        public GitReleaseManagerCreatorFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerCreator(FileSystem, Environment, ProcessRunner, Tools);
            tool.Create(UserName, Password, Owner, Repository, Settings);
        }
    }
}
