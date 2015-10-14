using Cake.Common.Tools.GitReleaseManager.Publish;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    public sealed class GitReleaseManagerPublisherFixture : GitReleaseManagerFixture
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public string TagName { get; set; }
        public GitReleaseManagerPublishSettings Settings { get; set; }

        public GitReleaseManagerPublisherFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            TagName = "0.1.0";
            Settings = new GitReleaseManagerPublishSettings();
        }

        public void Publish()
        {
            var tool = new GitReleaseManagerPublisher(FileSystem, Environment, ProcessRunner, Globber, GitReleaseManagerToolResolver);
            tool.Publish(UserName, Password, Owner, Repository, TagName, Settings);
        }
    }
}