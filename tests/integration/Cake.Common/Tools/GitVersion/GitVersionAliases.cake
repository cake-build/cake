#tool "dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=6.6.0"
#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

//////////////////////////////////////////////////////////////////////////////
// Integration tests for GitVersion aliases.
// Verifies GitVersion() runs with GitVersion.Tool 6+ and that legacy
// properties (LegacySemVerPadded, NuGetVersion, etc.) are populated.
// Skipped when CAKE_SKIP_GITVERSION is "True" (case-insensitive).
//////////////////////////////////////////////////////////////////////////////

public record GitVersionBuildData(bool SkipGitVersion);

Setup<GitVersionBuildData>(setupContext =>
{
    var skipGitVersion = StringComparer.OrdinalIgnoreCase.Equals("True", EnvironmentVariable("CAKE_SKIP_GITVERSION"));
    return new GitVersionBuildData(skipGitVersion);
});

Task("Cake.Common.Tools.GitVersion.GitVersionAliases.GitVersion.Default")
    .WithCriteria<GitVersionBuildData>((context, data) => !data.SkipGitVersion)
    .Does(() =>
{
    // When - run GitVersion with default settings (JSON output) in current repo
    var result = GitVersion(new GitVersionSettings { OutputType = GitVersionOutput.Json });

    // Then - core properties are set
    Assert.NotNull(result);
    Assert.True(result.Major >= 0, "Major should be non-negative");
    Assert.True(result.Minor >= 0, "Minor should be non-negative");
    Assert.True(result.Patch >= 0, "Patch should be non-negative");
    Assert.NotNull(result.MajorMinorPatch);
    Assert.NotNull(result.SemVer);
});

Task("Cake.Common.Tools.GitVersion.GitVersionAliases.GitVersion.LegacyPropertiesPopulated")
    .WithCriteria<GitVersionBuildData>((context, data) => !data.SkipGitVersion)
    .Does(() =>
{
    // When - run GitVersion (GitVersion 6+ omits legacy vars; Cake populates them)
    var result = GitVersion(new GitVersionSettings { OutputType = GitVersionOutput.Json });

    // Then - legacy properties are populated (best-effort when null from tool)
    Assert.NotNull(result);
    Assert.NotNull(result.LegacySemVer);
    Assert.NotNull(result.LegacySemVerPadded);
    Assert.NotNull(result.NuGetVersion);
    Assert.NotNull(result.NuGetVersionV2);
    Assert.NotNull(result.CommitsSinceVersionSourcePadded);
    Assert.NotNull(result.BuildMetaDataPadded);
    // NuGet pre-release can be empty for release versions
    Assert.NotNull(result.NuGetPreReleaseTag);
    Assert.NotNull(result.NuGetPreReleaseTagV2);
});

Task("Cake.Common.Tools.GitVersion.GitVersionAliases.GitVersion.LegacySemVerPadded.Format")
    .WithCriteria<GitVersionBuildData>((context, data) => !data.SkipGitVersion)
    .Does(() =>
{
    var result = GitVersion(new GitVersionSettings { OutputType = GitVersionOutput.Json });

    // LegacySemVerPadded should equal SemVer when no pre-release, or match known format
    Assert.NotNull(result.LegacySemVerPadded);
    if (string.IsNullOrEmpty(result.PreReleaseLabel))
    {
        Assert.Equal(result.MajorMinorPatch, result.LegacySemVerPadded);
    }
    else
    {
        Assert.Contains(result.MajorMinorPatch, result.LegacySemVerPadded);
    }
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.Tools.GitVersion.GitVersionAliases")
    .WithCriteria<GitVersionBuildData>((context, data) => !data.SkipGitVersion)
    .IsDependentOn("Cake.Common.Tools.GitVersion.GitVersionAliases.GitVersion.Default")
    .IsDependentOn("Cake.Common.Tools.GitVersion.GitVersionAliases.GitVersion.LegacyPropertiesPopulated")
    .IsDependentOn("Cake.Common.Tools.GitVersion.GitVersionAliases.GitVersion.LegacySemVerPadded.Format");
