// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tools.GitReleaseManager.Export;
using Cake.Core.IO;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerExporterFixture : GitReleaseManagerFixture<GitReleaseManagerExportSettings>
    {
        private bool _useToken = false;

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public FilePath FileOutputPath { get; set; }

        public GitReleaseManagerExporterFixture()
        {
            UserName = "bob";
            Password = "password";
            Token = "token";
            Owner = "repoOwner";
            Repository = "repo";
            FileOutputPath = "/temp";
        }

        public void UseToken()
        {
            _useToken = true;
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerExporter(FileSystem, Environment, ProcessRunner, Tools);

            if (_useToken)
            {
                tool.Export(Token, Owner, Repository, FileOutputPath, Settings);
            }
            else
            {
                tool.Export(UserName, Password, Owner, Repository, FileOutputPath, Settings);
            }
        }
    }
}
