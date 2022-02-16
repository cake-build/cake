#load "./paths.cake"
#load "./packages.cake"
#load "./version.cake"
#load "./credentials.cake"

public class BuildParameters
{
    public string Target { get; }
    public string Configuration { get; }
    public bool IsLocalBuild { get; }
    public bool IsRunningOnUnix { get; }
    public bool IsRunningOnWindows { get; }
    public bool IsRunningOnAppVeyor { get; }
    public bool IsPullRequest { get; }
    public bool IsMainCakeRepo { get; }
    public bool IsMainCakeBranch { get; }
    public bool IsDevelopCakeBranch { get; }
    public bool IsTagged { get; }
    public bool IsPublishBuild { get; }
    public bool IsReleaseBuild { get; }
    public bool SkipGitVersion { get; }
    public bool SkipSigning { get; }
    public BuildCredentials GitHub { get; }
    public TwitterCredentials Twitter { get; }
    public GitterCredentials Gitter { get; }
    public ReleaseNotes ReleaseNotes { get; }
    public BuildVersion Version { get; set; }
    public BuildPaths Paths { get; }
    public BuildPackages Packages { get; }
    public bool PublishingError { get; set; }
    public DotNetMSBuildSettings MSBuildSettings { get; }

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
            return false; /* !IsLocalBuild && !IsPullRequest && IsMainCakeRepo
                && (IsTagged || IsDevelopCakeBranch);*/
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

    public BuildParameters (ISetupContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var buildSystem = context.BuildSystem();

        Target = context.TargetTask.Name;
        Configuration = context.Argument("configuration", "Release");
        IsLocalBuild = buildSystem.IsLocalBuild;
        IsRunningOnUnix = context.IsRunningOnUnix();
        IsRunningOnWindows = context.IsRunningOnWindows();
        IsRunningOnAppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        IsMainCakeRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/cake", buildSystem.AppVeyor.Environment.Repository.Name);
        IsMainCakeBranch = StringComparer.OrdinalIgnoreCase.Equals("main", buildSystem.AppVeyor.Environment.Repository.Branch);
        IsDevelopCakeBranch = StringComparer.OrdinalIgnoreCase.Equals("develop", buildSystem.AppVeyor.Environment.Repository.Branch);
        IsTagged = IsBuildTagged(buildSystem);
        GitHub = BuildCredentials.GetGitHubCredentials(context);
        Twitter = TwitterCredentials.GetTwitterCredentials(context);
        Gitter = GitterCredentials.GetGitterCredentials(context);
        ReleaseNotes = context.ParseReleaseNotes("./ReleaseNotes.md");
        IsPublishBuild = IsPublishing(context.TargetTask.Name);
        IsReleaseBuild = IsReleasing(context.TargetTask.Name);
        SkipSigning = StringComparer.OrdinalIgnoreCase.Equals("True", context.Argument("skipsigning", "False"));
        SkipGitVersion = StringComparer.OrdinalIgnoreCase.Equals("True", context.EnvironmentVariable("CAKE_SKIP_GITVERSION"));
        Version = BuildVersion.Calculate(context, this);
        Paths = BuildPaths.GetPaths(context, Configuration, Version.SemVersion);
        Packages = BuildPackages.GetPackages(
            Paths.Directories.NuGetRoot,
            Version.SemVersion,
            new [] {
                "Cake.Core",
                "Cake.Common",
                "Cake.Testing",
                "Cake.Testing.Xunit",
                "Cake.NuGet",
                "Cake.Tool",
                "Cake.Frosting",
                "Cake.Frosting.Template",
                "Cake.Cli",
                "Cake.DotNetTool.Module"
                });

        var releaseNotes = string.Join(
                                System.Environment.NewLine,
                                ReleaseNotes.Notes.ToArray()
                            );

        MSBuildSettings = new DotNetMSBuildSettings {
                                Version = Version.SemVersion,
                                AssemblyVersion = Version.Version,
                                FileVersion = Version.Version,
                                PackageReleaseNotes =  releaseNotes
                            };

        if (!IsLocalBuild)
        {
            MSBuildSettings.WithProperty("TemplateVersion", Version.SemVersion);
        }
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }

    private static bool IsReleasing(string target)
    {
        var targets = new [] { "Publish", "Publish-NuGet", "Publish-GitHub-Release" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }

    private static bool IsPublishing(string target)
    {
        var targets = new [] { "ReleaseNotes", "Create-Release-Notes" };
        return targets.Any(t => StringComparer.OrdinalIgnoreCase.Equals(t, target));
    }
}

