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
    Assert.Equal(absoluteFilePath1, absoluteFilePath2);
});

Task("Cake.Core.IO.Path.ConvertableFilePath.DirectoryPathPlusFilePath")
    .Does(() =>
{
    // Given - DirectoryPath + ConvertableFilePath operator (Cake.Common) returns combined path with separator
    var dirPath = DirectoryPath.FromString("X");
    var filePath = File("file.txt");

    // When
    var result = dirPath + filePath;

    // Then
    Assert.Equal("X/file.txt", result);
});

Task("Cake.Core.IO.Path.ConvertableFilePath.DirectoryPathPlusFilePath.NullDirectoryThrows")
    .Does(() =>
{
    // Given
    DirectoryPath dirPath = null;
    var filePath = File("file.txt");

    // When / Then
    var ex = Assert.Throws<ArgumentNullException>(() => dirPath + filePath);
    Assert.Equal("dir", ex.ParamName);
});

Task("Cake.Core.IO.Path.ConvertableFilePath.DirectoryPathPlusFilePath.NullFileThrows")
    .Does(() =>
{
    // Given
    var dirPath = DirectoryPath.FromString("X");
    Cake.Common.IO.Paths.ConvertableFilePath filePath = null;

    // When / Then
    var ex = Assert.Throws<ArgumentNullException>(() => dirPath + filePath);
    Assert.Equal("file", ex.ParamName);
});

Task("Cake.Core.IO.Path.ConvertableFilePath.ConvertableDirectoryPathChain")
    .Does(() =>
{
    // Given - root + dir + file using ConvertableDirectoryPath chain (Directory + Directory + File)
    var rootDir = Directory("..");
    var dirPath = Directory("X");
    var filePath = File("file.txt");

    // When
    var result = rootDir + dirPath + filePath;

    // Then
    Assert.Equal("../X/file.txt", result);
});

Task("Cake.Core.IO.Path")
    .IsDependentOn("Cake.Core.IO.Path.DirectoryPath.Relative")
    .IsDependentOn("Cake.Core.IO.Path.DirectoryPath.Absolute")
    .IsDependentOn("Cake.Core.IO.Path.FilePath.Relative")
    .IsDependentOn("Cake.Core.IO.Path.FilePath.Absolute")
    .IsDependentOn("Cake.Core.IO.Path.ConvertableFilePath.DirectoryPathPlusFilePath")
    .IsDependentOn("Cake.Core.IO.Path.ConvertableFilePath.DirectoryPathPlusFilePath.NullDirectoryThrows")
    .IsDependentOn("Cake.Core.IO.Path.ConvertableFilePath.DirectoryPathPlusFilePath.NullFileThrows")
    .IsDependentOn("Cake.Core.IO.Path.ConvertableFilePath.ConvertableDirectoryPathChain");
