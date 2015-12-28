using Cake.Common.Tools.GitReleaseManager.Publish;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    internal sealed class GitReleaseManagerPublisherFixture : GitReleaseManagerFixture<GitReleaseManagerPublishSettings>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string TagName { get; set; }

        public GitReleaseManagerPublisherFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            TagName = "0.1.0";
        }

        protected override void RunTool()
        {
            var tool = new GitReleaseManagerPublisher(FileSystem, Environment, ProcessRunner, Globber, Resolver);
            tool.Publish(UserName, Password, Owner, Repository, TagName, Settings);
        }
    }
}