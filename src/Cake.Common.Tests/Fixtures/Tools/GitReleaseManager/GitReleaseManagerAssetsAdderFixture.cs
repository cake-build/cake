using Cake.Common.Tools.GitReleaseManager.AddAssets;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    public sealed class GitReleaseManagerAssetsAdderFixture : GitReleaseManagerFixture
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string TagName { get; set; }
        public string Assets { get; set; }
        public GitReleaseManagerAddAssetsSettings Settings { get; set; }

        public GitReleaseManagerAssetsAdderFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            TagName = "0.1.0";
            Assets = @"c:/temp/asset1.txt";
            Settings = new GitReleaseManagerAddAssetsSettings();
        }

        public void AddAssets()
        {
            var tool = new GitReleaseManagerAssetsAdder(FileSystem, Environment, ProcessRunner, Globber, GitReleaseManagerToolResolver);
            tool.AddAssets(UserName, Password, Owner, Repository, TagName, Assets, Settings);
        }
    }
}