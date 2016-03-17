#load "./paths.cake"
#load "./packages.cake"
#load "./version.cake"

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
    public bool IsPublishBuild { get; private set; }
    public bool IsReleaseBuild { get; private set; }
    public BuildCredentials GitHub { get; private set; }
    public ReleaseNotes ReleaseNotes { get; private set; }
    public BuildVersion Version { get; private set; }
    public BuildPaths Paths { get; private set; }
    public BuildPackages Packages { get; private set; }

    public void SetVersion(BuildVersion version)
    {
        Version  = version;
    }

    public void SetPaths(BuildPaths paths)
    {
        Paths  = paths;
    }

    public void SetPackages(BuildPackages packages)
    {
        Packages  = packages;
    }

    public static BuildParameters GetParameters(
        ICakeContext context,
        BuildSystem buildSystem
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var target = context.Argument("target", "Default");

        return new BuildParameters {
            Target = target,
            Configuration = context.Argument("configuration", "Release"),
            IsLocalBuild = buildSystem.IsLocalBuild,
            IsRunningOnUnix = context.IsRunningOnUnix(),
            IsRunningOnWindows = context.IsRunningOnWindows(),
            IsRunningOnAppVeyor = context.IsRunningOnWindows(),
            IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest,
            IsMainCakeRepo =  StringComparer.OrdinalIgnoreCase.Equals("cake-build/cake", buildSystem.AppVeyor.Environment.Repository.Name),
            GitHub = new BuildCredentials (
                userName: context.EnvironmentVariable("CAKE_GITHUB_USERNAME"),
                password: context.EnvironmentVariable("CAKE_GITHUB_PASSWORD")
            ),
            ReleaseNotes = context.ParseReleaseNotes("./ReleaseNotes.md"),
            IsPublishBuild = new [] {
                "ReleaseNotes",
                "Create-Release-Notes"
            }.Any(
                releaseTarget => StringComparer.OrdinalIgnoreCase.Equals(releaseTarget, target)
            ),
            IsReleaseBuild = new [] {
                "Publish",
                "Publish-NuGet",
                "Publish-Chocolatey",
                "Publish-HomeBrew",
                "Publish-GitHub-Release"
            }.Any(
                publishTarget => StringComparer.OrdinalIgnoreCase.Equals(publishTarget, target)
            )
        };
    }
}

public class BuildCredentials
{
    public string UserName { get; private set; }
    public string Password { get; private set; }

    public BuildCredentials(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}

