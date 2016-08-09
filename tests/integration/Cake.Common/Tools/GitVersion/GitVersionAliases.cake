#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/gitversion.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.Tools.GitVersionAliases.OutputTypeIsBuildServer")
    .Does(() =>
{
    // Given, When
    var gitVersion = GitVersion(new GitVersionSettings
    {
        OutputType = GitVersionOutput.BuildServer,
    });

    // Then
    AssertNotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
    AssertNotNull(gitVersion.BranchName, "BranchName");
    AssertNotNull(gitVersion.BuildMetaData, "BuildMetaData");
    AssertNotNull(gitVersion.BuildMetaDataPadded, "BuildMetaDataPadded");
    AssertNotNull(gitVersion.CommitDate, "CommitDate");
    AssertNotNull(gitVersion.CommitsSinceVersionSource, "CommitsSinceVersionSource");
    AssertNotNull(gitVersion.CommitsSinceVersionSourcePadded, "CommitsSinceVersionSourcePadded");
    AssertNotNull(gitVersion.FullBuildMetaData, "FullBuildMetaData");
    AssertNotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
});

Task("Cake.Common.Tools.GitVersionAliases.OutputTypeIsJson")
    .Does(() =>
{
    // Given, When
    var gitVersion = GitVersion(new GitVersionSettings
    {
        OutputType = GitVersionOutput.Json,
    });

    // Then
    AssertNotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
    AssertNotNull(gitVersion.BranchName, "BranchName");
    AssertNotNull(gitVersion.BuildMetaData, "BuildMetaData");
    AssertNotNull(gitVersion.BuildMetaDataPadded, "BuildMetaDataPadded");
    AssertNotNull(gitVersion.CommitDate, "CommitDate");
    AssertNotNull(gitVersion.CommitsSinceVersionSource, "CommitsSinceVersionSource");
    AssertNotNull(gitVersion.CommitsSinceVersionSourcePadded, "CommitsSinceVersionSourcePadded");
    AssertNotNull(gitVersion.FullBuildMetaData, "FullBuildMetaData");
    AssertNotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
});

Task("Cake.Common.Tools.GitVersionAliases")
  .IsDependentOn("Cake.Common.Tools.GitVersionAliases.OutputTypeIsJson")
  .IsDependentOn("Cake.Common.Tools.GitVersionAliases.OutputTypeIsBuildServer");