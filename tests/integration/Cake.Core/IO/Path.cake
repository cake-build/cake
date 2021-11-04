#load "./../../utilities/xunit.cake"

Task("Cake.Core.IO.Path.DirectoryPath.Relative")
    .Does(() =>
{
    // Given
    var relativeDirectoryPath1 = DirectoryPath.FromString("./MyProject");
    var relativeDirectoryPath2 = DirectoryPath.FromString(".\\MyProject");

    // When / Then
    Assert.Equal(relativeDirectoryPath1, relativeDirectoryPath2);
});

Task("Cake.Core.IO.Path.DirectoryPath.Absolute")
    .Does(() =>
{
    // Given
    var absoluteDirectoryPath1 = MakeAbsolute(DirectoryPath.FromString("./MyProject"));
    var absoluteDirectoryPath2 = MakeAbsolute(DirectoryPath.FromString(".\\MyProject"));

    // When / Then
    Assert.Equal(absoluteDirectoryPath1, absoluteDirectoryPath2);
});


Task("Cake.Core.IO.Path.FilePath.Relative")
    .Does(() =>
{
    // Given
    var relativeFilePath1 = FilePath.FromString("./MyProject/MyApp.csproj");
    var relativeFilePath2 = FilePath.FromString(".\\MyProject\\MyApp.csproj");

    // When / Then
    Assert.Equal(relativeFilePath1, relativeFilePath2);
});

Task("Cake.Core.IO.Path.FilePath.Absolute")
    .Does(() =>
{
    // Given
    var absoluteFilePath1 = MakeAbsolute(FilePath.FromString("./MyProject/MyApp.csproj"));
    var absoluteFilePath2 = MakeAbsolute(FilePath.FromString(".\\MyProject\\MyApp.csproj"));

    // When / Then
    Assert.Equal(absoluteFilePath1, absoluteFilePath1);
});



Task("Cake.Core.IO.Path")
    .IsDependentOn("Cake.Core.IO.Path.DirectoryPath.Relative")
    .IsDependentOn("Cake.Core.IO.Path.DirectoryPath.Absolute")
    .IsDependentOn("Cake.Core.IO.Path.FilePath.Relative")
    .IsDependentOn("Cake.Core.IO.Path.FilePath.Absolute");
