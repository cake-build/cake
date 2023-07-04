public record CodeSigningCredentials(
    string SignTenantId,
    string SignClientId,
    string SignClientSecret,
    string SignKeyVaultCertificate,
    string SignKeyVaultUrl
)
{
    public bool HasCredentials
    {
        get
        {
            return
                !string.IsNullOrEmpty(SignTenantId) &&
                !string.IsNullOrEmpty(SignClientId) &&
                !string.IsNullOrEmpty(SignClientSecret) &&
                !string.IsNullOrEmpty(SignKeyVaultCertificate) &&
                !string.IsNullOrEmpty(SignKeyVaultUrl);
        }
    }

    public static CodeSigningCredentials GetCodeSigningCredentials(ICakeContext context)
    {
        return new CodeSigningCredentials(
            SignTenantId: context.EnvironmentVariable("SIGN_TENANT_ID"),
            SignClientId: context.EnvironmentVariable("SIGN_CLIENT_ID"),
            SignClientSecret: context.EnvironmentVariable("SIGN_CLIENT_SECRET"),
            SignKeyVaultCertificate: context.EnvironmentVariable("SIGN_KEYVAULT_CERTIFICATE"),
            SignKeyVaultUrl: context.EnvironmentVariable("SIGN_KEYVAULT_URL"));
    }
}

public record BuildCredentials(string Token)
{
    public static BuildCredentials GetGitHubCredentials(ICakeContext context)
    {
        return new BuildCredentials(
            context.EnvironmentVariable("CAKE_GITHUB_TOKEN"));
    }
}

public record TwitterCredentials(
    string ConsumerKey,
    string ConsumerSecret,
    string AccessToken,
    string AccessTokenSecret
)
{
    public static TwitterCredentials GetTwitterCredentials(ICakeContext context)
    {
        return new TwitterCredentials(
            context.EnvironmentVariable("TWITTER_CONSUMER_KEY"),
            context.EnvironmentVariable("TWITTER_CONSUMER_SECRET"),
            context.EnvironmentVariable("TWITTER_ACCESS_TOKEN"),
            context.EnvironmentVariable("TWITTER_ACCESS_TOKEN_SECRET"));
    }
}
