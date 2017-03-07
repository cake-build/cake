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

public class TwitterCredentials
{
    public string ConsumerKey { get; private set; }
    public string ConsumerSecret { get; private set; }
    public string AccessToken { get; private set; }
    public string AccessTokenSecret { get; private set; }

    public TwitterCredentials(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
    {
        ConsumerKey = consumerKey;
        ConsumerSecret = consumerSecret;
        AccessToken = accessToken;
        AccessTokenSecret = accessTokenSecret;
    }

    public static TwitterCredentials GetTwitterCredentials(ICakeContext context)
    {
        return new TwitterCredentials(
            context.EnvironmentVariable("TWITTER_CONSUMER_KEY"),
            context.EnvironmentVariable("TWITTER_CONSUMER_SECRET"),
            context.EnvironmentVariable("TWITTER_ACCESS_TOKEN"),
            context.EnvironmentVariable("TWITTER_ACCESS_TOKEN_SECRET"));
    }
}

public class GitterCredentials
{
    public string Token { get; private set; }
    public string RoomId { get; private set; }

    public GitterCredentials(string token, string roomId)
    {
        Token = token;
        RoomId = roomId;
    }

    public static GitterCredentials GetGitterCredentials(ICakeContext context)
    {
        return new GitterCredentials(
            context.EnvironmentVariable("GITTER_TOKEN"),
            context.EnvironmentVariable("GITTER_ROOM_ID")
        );
    }
}
