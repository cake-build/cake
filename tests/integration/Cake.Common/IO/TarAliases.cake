#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

Task("Cake.Common.IO.TarAliases.Untar")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");
    var sourceFile = sourcePath.CombineWithFilePath("./testfile.txt");

    var archivePath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/Untar");
    var archiveFile = archivePath.CombineWithFilePath("testfile.tar");
    var targetPath = archivePath.Combine("Out");
    var targetFile = targetPath.CombineWithFilePath("testfile.txt");
    EnsureDirectoryExist(archivePath);
    EnsureDirectoryExist(targetPath);

    // When
    Tar(sourcePath, archiveFile, new[] { sourceFile });
    Untar(archiveFile, targetPath);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
    Assert.True(FileHashEquals(sourceFile, targetFile), "Hash: " + targetFile.FullPath);
});

Task("Cake.Common.IO.TarAliases.Tar.Directory")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("Cake.Common/IO/Root");
    EnsureDirectoryExist(sourcePath.Combine("Dir0"));

    var targetPath = Paths.Temp.Combine("Cake.Common.IO.TarAliases/Directory");
    var targetFile = targetPath.CombineWithFilePath("testfile.tar");
    EnsureDirectoryExist(targetPath);

    // When
    Tar(sourcePath, targetFile);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.TarAliases.Tar.FilePaths")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/FilePaths");
    var targetFile = targetPath.CombineWithFilePath("testfile.tar");
    EnsureDirectoryExist(targetPath);

    // When
    var filePaths = GetFiles(sourcePath.FullPath + "/**/testfile.*");
    Tar(sourcePath, targetFile, filePaths);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.TarAliases.Tar.Strings")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/Strings");
    var targetFile = targetPath.CombineWithFilePath("testfile.tar");
    EnsureDirectoryExist(targetPath);

    // When
    var fileStrings = GetFiles(sourcePath.FullPath + "/**/testfile.*")
                        .Select(filePath => filePath.FullPath);
    Tar(sourcePath, targetFile, fileStrings);

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.TarAliases.Tar.Pattern")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Resources.Combine("./Cake.Common/IO");

    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/Pattern");
    var targetFile = targetPath.CombineWithFilePath("testfile.tar");
    EnsureDirectoryExist(targetPath);

    // When
    Tar(sourcePath, targetFile, sourcePath.FullPath + "/**/testfile.*");

    // Then
    Assert.True(PathExists(targetFile), "Exists: " + targetFile.FullPath);
});

Task("Cake.Common.IO.TarAliases.TarUntar.GZip")
    .Does(() =>
{
    // Given
    var expectedDate = new DateTime(2001, 2, 3, 4, 5, 6, DateTimeKind.Utc);
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/GZip");
    var targetFile = targetPath.CombineWithFilePath("testfile.tar.gz");
    var sourceFile = targetPath.CombineWithFilePath("text.txt");
    var outPath = targetPath.Combine("Out");
    var outFile = outPath.CombineWithFilePath("text.txt");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(sourceFile);
    System.IO.File.SetLastWriteTimeUtc(sourceFile.FullPath, expectedDate);

    // When
    Tar(targetPath, targetFile, new[] { sourceFile }, new TarSettings
    {
        Compression = TarCompression.GZip
    });
    Untar(targetFile, outPath, new UntarSettings { SkipUnchangedFiles = false });

    // Then
    Assert.True(PathExists(outFile), "Exists: " + outFile.FullPath);
    var result = System.IO.File.GetLastWriteTimeUtc(outFile.FullPath);
    var duration = expectedDate - result;
    Assert.True(Math.Abs(duration.TotalSeconds) < 1, $"Expected: {expectedDate}, Actual: {result}, Delta: {duration}");
});

Task("Cake.Common.IO.TarAliases.Untar.Multiple")
    .Does(() =>
{
    // Given
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/Multiple");
    var firstSource = targetPath.CombineWithFilePath("first.txt");
    var secondSource = targetPath.CombineWithFilePath("second.txt");
    var firstArchive = targetPath.CombineWithFilePath("first.tar");
    var secondArchive = targetPath.CombineWithFilePath("second.tar.gz");
    var outPath = targetPath.Combine("Out");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(firstSource, "first");
    EnsureFileExist(secondSource, "second");
    Tar(targetPath, firstArchive, new[] { firstSource });
    Tar(targetPath, secondArchive, new[] { secondSource }, new TarSettings
    {
        Compression = TarCompression.GZip
    });

    // When
    Untar(new[] { firstArchive, secondArchive }, outPath);

    // Then
    Assert.True(PathExists(outPath.CombineWithFilePath("first.txt")), "Exists: first.txt");
    Assert.True(PathExists(outPath.CombineWithFilePath("second.txt")), "Exists: second.txt");
});

Task("Cake.Common.IO.TarAliases.Untar.SkipUnchangedFiles")
    .Does(() =>
{
    // Given
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/SkipUnchangedFiles");
    var sourceFile = targetPath.CombineWithFilePath("text.txt");
    var archiveFile = targetPath.CombineWithFilePath("text.tar");
    var outPath = targetPath.Combine("Out");
    var outFile = outPath.CombineWithFilePath("text.txt");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(sourceFile, "old");
    Tar(targetPath, archiveFile, new[] { sourceFile });
    Untar(archiveFile, outPath, new UntarSettings { SkipUnchangedFiles = false });
    System.IO.File.WriteAllText(outFile.FullPath, "new");
    System.IO.File.SetLastWriteTimeUtc(outFile.FullPath, DateTime.UtcNow.AddHours(1));

    // When
    Untar(archiveFile, outPath, new UntarSettings { SkipUnchangedFiles = true });

    // Then
    Assert.Equal("new", System.IO.File.ReadAllText(outFile.FullPath));
});

Task("Cake.Common.IO.TarAliases.Untar.OverwriteReadOnlyFiles")
    .Does(() =>
{
    // Given
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.TarAliases/OverwriteReadOnlyFiles");
    var sourceFile = targetPath.CombineWithFilePath("text.txt");
    var archiveFile = targetPath.CombineWithFilePath("text.tar");
    var outPath = targetPath.Combine("Out");
    var outFile = outPath.CombineWithFilePath("text.txt");
    EnsureDirectoryExist(outPath);
    EnsureFileExist(sourceFile, "updated");
    Tar(targetPath, archiveFile, new[] { sourceFile });
    EnsureFileExist(outFile, "old");
    System.IO.File.SetLastWriteTimeUtc(outFile.FullPath, new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc));
    System.IO.File.SetAttributes(outFile.FullPath, System.IO.FileAttributes.ReadOnly);

    // When
    Untar(archiveFile, outPath, new UntarSettings
    {
        SkipUnchangedFiles = false,
        OverwriteReadOnlyFiles = true
    });

    // Then
    Assert.Equal("updated" + Environment.NewLine, System.IO.File.ReadAllText(outFile.FullPath));
});

Task("Cake.Common.IO.TarAliases")
    .IsDependentOn("Cake.Common.IO.TarAliases.Untar")
    .IsDependentOn("Cake.Common.IO.TarAliases.Tar.Directory")
    .IsDependentOn("Cake.Common.IO.TarAliases.Tar.FilePaths")
    .IsDependentOn("Cake.Common.IO.TarAliases.Tar.Strings")
    .IsDependentOn("Cake.Common.IO.TarAliases.Tar.Pattern")
    .IsDependentOn("Cake.Common.IO.TarAliases.TarUntar.GZip")
    .IsDependentOn("Cake.Common.IO.TarAliases.Untar.Multiple")
    .IsDependentOn("Cake.Common.IO.TarAliases.Untar.SkipUnchangedFiles")
    .IsDependentOn("Cake.Common.IO.TarAliases.Untar.OverwriteReadOnlyFiles");
