#load "./paths.cake"
#load "./packages.cake"
#load "./version.cake"
#load "./credentials.cake"

public class BuildParameters
{
    public string Target { get; private set; }
    public string Configuration { get; private set; }
    public bool IsLocalBuild { get; private set; }
    public bool IsRunningOnUnix { get; private set; }
    public bool IsRunningOnWindows { get; private set; }
    public bool IsRunningOnAppVeyor { get; private set; }
    public bool IsPullRequest { get; private set; }
    public bool IsMainCakeRepo { get; private set; }
    public bool IsMainCakeBranch { get; private set; }
    public bool IsDevelopCakeBranch { get; private set; }
    public bool IsTagged { get; private set; }
    public bool IsPublishBuild { get; private set; }
    public bool IsReleaseBuild { get; private set; }
    public bool SkipGitVersion { get; private set; }
    public bool SkipOpenCover { get; private set; }
    public bool SkipSigning { get; private set; }
    public BuildCredentials GitHub { get; private set; }
    public CoverallsCredentials Coveralls { get; private set; }
    public TwitterCredentials Twitter { get; private set; }
    public GitterCredentials Gitter { get; private set; }
    public ReleaseNotes ReleaseNotes { get; private set; }
    public BuildVersion Version { get; private set; }
    public BuildPaths Paths { get; private set; }
    public BuildPackages Packages { get; private set; }

    public bool ShouldPublish
    {
        get
        {
            return !IsLocalBuild && !IsPullRequest && IsMainCakeRepo
                && IsMainCakeBranch && IsTagged;
        }
    }

    public bool ShouldPublishToMyGet
    {
        get
        {
            return !IsLocalBuild && !IsPullRequest && IsMainCakeRepo
                && (IsTagged || IsDevelopCakeBranch);
        }
    }

    public bool CanPostToTwitter
    {
        get
        {
            return !string.IsNullOrEmpty(Twitter.ConsumerKey) &&
                !string.IsNullOrEmpty(Twitter.ConsumerSecret) &&
                !string.IsNullOrEmpty(Twitter.AccessToken) &&
                !string.IsNullOrEmpty(Twitter.AccessTokenSecret);
        }
    }

    public bool CanPostToGitter
    {
        get
        {
            return !string.IsNullOrEmpty(Gitter.Token) && !string.IsNullOrEmpty(Gitter.RoomId);
        }
    }

    public void Initialize(ICakeContext context)
    {
        Version = BuildVersion.Calculate(context, this);

        Paths = BuildPaths.GetPaths(context, Configuration, Version.SemVersion);

        Packages = BuildPackages.GetPackages(
            Paths.Directories.NugetRoot,
            Version.SemVersion,
            new [] { "Cake", "Cake.Core", "Cake.Common", "Cake.Testing", "Cake.CoreCLR", "Cake.NuGet" },
            new [] { "cake.portable" });
    }

    public static BuildParameters GetParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var target = context.Argument("target", "Default");
        var buildSystem = context.BuildSystem();

        return new BuildParameters {
            Target = target,
            Configuration = context.Argument("configuration", "Release"),
            IsLocalBuild = buildSystem.IsLocalBuild,
            IsRunningOnUnix = context.IsRunningOnUnix(),
            IsRunningOnWindows = context.IsRunningOnWindows(),
            IsRunningOnAppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor,
            IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest,
            IsMainCakeRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/cake", buildSystem.AppVeyor.Environment.Repository.Name),
            IsMainCakeBranch = StringComparer.OrdinalIgnoreCase.Equals("main", buildSystem.AppVeyor.Environment.Repository.Branch),
            IsDevelopCakeBranch = StringComparer.OrdinalIgnoreCase.Equals("develop", buildSystem.AppVeyor.Environment.Repository.Branch),
            IsTagged = IsBuildTagged(buildSystem),
            GitHub = BuildCredentials.GetGitHubCredentials(context),
            Coveralls = CoverallsCredentials.GetCoverallsCredentials(context),
            Twitter = TwitterCredentials.GetTwitterCredentials(context),
            Gitter = GitterCredentials.GetGitterCredentials(context),
            ReleaseNotes = context.ParseReleaseNotes("./ReleaseNotes.md"),
            IsPublishBuild = IsPublishing(target),
            IsReleaseBuild = IsReleasing(target),
            SkipSigning = StringComparer.OrdinalIgnoreCase.Equals("True", context.Argument("skipsigning", "False")),
            SkipGitVersion = StringComparer.OrdinalIgnoreCase.Equals("True", context.EnvironmentVariable("CAKE_SKIP_GITVERSION")),
            SkipOpenCover = true//StringComparer.OrdinalIgnoreCase.Equals("True", context.EnvironmentVariable("CAKE_SKIP_OPENCOVER"))
        };
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }

    private static bool IsReleasing(string target)
    {
        var targets = new [] { "Publish", "Publish-NuGet", "Publish-Chocolatey", "Publish-HomeBrew", "Publish-GitHub-Release" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }

    private static bool IsPublishing(string target)
    {
        var targets = new [] { "ReleaseNotes", "Create-Release-Notes" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }
}

