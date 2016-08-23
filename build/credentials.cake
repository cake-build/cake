public class BuildCredentials
{
    public string UserName { get; private set; }
    public string Password { get; private set; }

    public BuildCredentials(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public static BuildCredentials GetGitHubCredentials(ICakeContext context)
    {
        return new BuildCredentials(
            context.EnvironmentVariable("CAKE_GITHUB_USERNAME"),
            context.EnvironmentVariable("CAKE_GITHUB_PASSWORD"));
    }
}

public class CoverallsCredentials
{
    public string RepoToken { get; private set; }

    public CoverallsCredentials(string repoToken)
    {
        RepoToken = repoToken;
    }

    public static CoverallsCredentials GetCoverallsCredentials(ICakeContext context)
    {
        return new CoverallsCredentials(
            context.EnvironmentVariable("COVERALLS_REPO_TOKEN"));
    }
}