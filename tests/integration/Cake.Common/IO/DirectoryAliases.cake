#load "./../../scripts/directory.cake"
#load "./../../scripts/xunit.cake"
#load "./../../scripts/paths.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.DirectoryAliases.CreateDirectory")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./hello");
    EnsureDirectoryDoNotExist(path);

    // When
    CreateDirectory(path);

    // Then
    Assert.True(System.IO.Directory.Exists(path.FullPath));
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.DirectoryAliases")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.CreateDirectory");
