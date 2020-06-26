#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

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

Task("Cake.Common.IO.ZipAliases.ZipUnzip.ValidLastWriteTime")
    .Does(() =>
{
    // Given
    var expectedDate = new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc);
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/ValidLastWriteTime");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    var sourceFile = targetPath.CombineWithFilePath("text.txt");
    var outPath = targetPath.Combine("Out");
    var outFile = outPath.CombineWithFilePath("text.txt");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(sourceFile);
    System.IO.File.SetLastWriteTimeUtc(sourceFile.FullPath, expectedDate);

    // When
    Zip(targetPath, targetFile, new []{ sourceFile });
    Unzip(targetFile, outPath);

    // Then
    var result = System.IO.File.GetLastWriteTimeUtc(outFile.FullPath);
    var duration  = expectedDate - result;
    Assert.True(duration.TotalMinutes==0, $"Expected: {expectedDate}, Actual: {result}, Delta: {duration}");
});

Task("Cake.Common.IO.ZipAliases.ZipUnzip.InValidMinLastWriteTime")
    .Does(() =>
{
    // Given
    var givenDate = new DateTime(1979, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var expectedDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/InValidMinLastWriteTime");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    var sourceFile = targetPath.CombineWithFilePath("text.txt");
    var outPath = targetPath.Combine("Out");
    var outFile = outPath.CombineWithFilePath("text.txt");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(sourceFile);
    System.IO.File.SetLastWriteTimeUtc(sourceFile.FullPath, givenDate);

    // When
    Zip(targetPath, targetFile, new []{ sourceFile });
    Unzip(targetFile, outPath);

    // Then
    var result = System.IO.File.GetLastWriteTime(outFile.FullPath);
    var duration  = expectedDate - result;
    Assert.True(duration.TotalMinutes==0, $"Expected: {expectedDate}, Actual: {result}, Delta: {duration}");
});

Task("Cake.Common.IO.ZipAliases.ZipUnzip.InValidMaxLastWriteTime")
    .Does(() =>
{
    // Given
    var givenDate = new DateTime(2108, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var expectedDate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.ZipAliases/InValidMaxLastWriteTime");
    var targetFile = targetPath.CombineWithFilePath("testfile.zip");
    var sourceFile = targetPath.CombineWithFilePath("text.txt");
    var outPath = targetPath.Combine("Out");
    var outFile = outPath.CombineWithFilePath("text.txt");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(sourceFile);
    System.IO.File.SetLastWriteTimeUtc(sourceFile.FullPath, givenDate);

    // When
    Zip(targetPath, targetFile, new []{ sourceFile });
    Unzip(targetFile, outPath);

    // Then
    var result = System.IO.File.GetLastWriteTime(outFile.FullPath);
    var duration  = expectedDate - result;
    Assert.True(duration.TotalMinutes==0, $"Expected: {expectedDate}, Actual: {result}, Delta: {duration}");
});


Task("Cake.Common.IO.ZipAliases")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Unzip")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.Directory")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.FilePaths")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.Strings")
    .IsDependentOn("Cake.Common.IO.ZipAliases.Zip.Pattern")
    .IsDependentOn("Cake.Common.IO.ZipAliases.ZipUnzip.ValidLastWriteTime")
    .IsDependentOn("Cake.Common.IO.ZipAliases.ZipUnzip.InValidMinLastWriteTime")
    .IsDependentOn("Cake.Common.IO.ZipAliases.ZipUnzip.InValidMaxLastWriteTime");
