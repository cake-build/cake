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
    Assert.NotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
    Assert.NotNull(gitVersion.BranchName, "BranchName");
    Assert.NotNull(gitVersion.BuildMetaData, "BuildMetaData");
    Assert.NotNull(gitVersion.BuildMetaDataPadded, "BuildMetaDataPadded");
    Assert.NotNull(gitVersion.CommitDate, "CommitDate");
    Assert.NotNull(gitVersion.CommitsSinceVersionSource, "CommitsSinceVersionSource");
    Assert.NotNull(gitVersion.CommitsSinceVersionSourcePadded, "CommitsSinceVersionSourcePadded");
    Assert.NotNull(gitVersion.FullBuildMetaData, "FullBuildMetaData");
    Assert.NotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
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
    Assert.NotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
    Assert.NotNull(gitVersion.BranchName, "BranchName");
    Assert.NotNull(gitVersion.BuildMetaData, "BuildMetaData");
    Assert.NotNull(gitVersion.BuildMetaDataPadded, "BuildMetaDataPadded");
    Assert.NotNull(gitVersion.CommitDate, "CommitDate");
    Assert.NotNull(gitVersion.CommitsSinceVersionSource, "CommitsSinceVersionSource");
    Assert.NotNull(gitVersion.CommitsSinceVersionSourcePadded, "CommitsSinceVersionSourcePadded");
    Assert.NotNull(gitVersion.FullBuildMetaData, "FullBuildMetaData");
    Assert.NotNull(gitVersion.AssemblySemVer, "AssemblySemVer");
});

Task("Cake.Common.Tools.GitVersionAliases")
  .IsDependentOn("Cake.Common.Tools.GitVersionAliases.OutputTypeIsJson")
  .IsDependentOn("Cake.Common.Tools.GitVersionAliases.OutputTypeIsBuildServer");