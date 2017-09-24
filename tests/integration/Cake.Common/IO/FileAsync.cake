#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

Task("Cake.Common.IO.FileAsync.CopyFileAsync")
    .Does(async () =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFileToDirectory/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFileToDirectory/target");
    var sourceFile = sourcePath.CombineWithFilePath("test.file");
    var targetFile = targetPath.CombineWithFilePath("test.file");
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFileExist(sourceFile);

    // When
    using(var sourceStream = System.IO.File.OpenRead(sourceFile.ToString()))
    using(var targetStream = System.IO.File.OpenWrite(targetFile.ToString()))
    {
        await sourceStream.CopyToAsync(targetStream);
    }

    // Then
    Assert.True(System.IO.File.Exists(targetFile.FullPath));
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.FileAsync")
    .IsDependentOn("Cake.Common.IO.FileAsync.CopyFileAsync");