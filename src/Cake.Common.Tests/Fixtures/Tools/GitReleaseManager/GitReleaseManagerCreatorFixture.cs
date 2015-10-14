using Cake.Common.Tools.GitReleaseManager.Create;

namespace Cake.Common.Tests.Fixtures.Tools.GitReleaseManager
{
    public sealed class GitReleaseManagerCreatorFixture : GitReleaseManagerFixture
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Owner { get; set; }
        public string Repository { get; set; }
        public GitReleaseManagerCreateSettings Settings { get; set; }

        public GitReleaseManagerCreatorFixture()
        {
            UserName = "bob";
            Password = "password";
            Owner = "repoOwner";
            Repository = "repo";
            Settings = new GitReleaseManagerCreateSettings();
        }

        public void Create()
        {
            var tool = new GitReleaseManagerCreator(FileSystem, Environment, ProcessRunner, Globber, GitReleaseManagerToolResolver);
            tool.Create(UserName, Password, Owner, Repository, Settings);
        }
    }
}