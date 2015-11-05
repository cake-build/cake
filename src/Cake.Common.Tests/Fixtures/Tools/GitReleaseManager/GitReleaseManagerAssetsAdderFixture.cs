using Cake.Common.Tools.GitReleaseManager.AddAssets;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerAssetsAdderFixture : GitReleaseManagerFixture<GitReleaseManagerAddAssetsSettings>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string TagName { get; set; }
        public string Assets { get; set; }

        public GitReleaseManagerAssetsAdderFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            TagName = "0.1.0";
            Assets = @"c:/temp/asset1.txt";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerAssetsAdder(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.AddAssets(UserName, Password, Owner, Repository, TagName, Assets, Settings);
        }
    }
}