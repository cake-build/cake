#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"

Task("Cake.Common.IO.ZipAliases.Unzip")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");
    var sourceFile = sourcePath.CombineWithFilePath("./testfile.zip");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/Unzip");
    var targetFile = targetPath.CombineWithFilePath("testfile.txt");
    EnsureDirectoryExist(targetPath);

    // When
    Unzip(sourceFile, targetPath);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.ZipAliases.Zip.Directory")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("Cake.Common/IO/Root");
    EnsureDirectoryExist(sourcePath.Combine("Dir0")); // empty directory

    var targetPath = Paths.Temp.Combine("Cake.Common.IO.ZipAliases/Directory");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    EnsureDirectoryExist(targetPath);

    // When
    Zip(sourcePath, targetFile);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.ZipAliases.Zip.FilePaths")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/FilePaths");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    EnsureDirectoryExist(targetPath);

    // When
    var filePaths = GetFiles(sourcePath.FullPath + "/**/testfile.*");
    Zip(sourcePath, targetFile, filePaths);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.ZipAliases.Zip.Strings")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/Strings");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    EnsureDirectoryExist(targetPath);

    // When
    var fileStrings = GetFiles(sourcePath.FullPath + "/**/testfile.*")
                        .Select(filePath=>filePath.FullPath);
    Zip(sourcePath, targetFile, fileStrings);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.ZipAliases.Zip.Pattern")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/Pattern");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    EnsureDirectoryExist(targetPath);

    // When
    Zip(sourcePath, targetFile, sourcePath.FullPath + "/**/testfile.*");

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});


Task("Cake.Common.IO.ZipAliases")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Unzip")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.Directory")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.FilePaths")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.Strings")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.Pattern");
