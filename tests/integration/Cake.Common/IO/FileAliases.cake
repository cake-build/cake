#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

//////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.FileAliases.CopyFileToDirectory")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFileToDirectory/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFileToDirectory/target");
    var sourceFile = sourcePath.CombineWithFilePath("test.file");
    var targetFile = targetPath.CombineWithFilePath("test.file");
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFileExist(sourceFile);

    // When
    CopyFileToDirectory(sourceFile, targetPath);

    // Then
    Assert.True(System.IO.File.Exists(targetFile.FullPath));
});

Task("Cake.Common.IO.FileAliases.CopyFile")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFile");
    var sourceFile = path.CombineWithFilePath("source.txt");
    var targetFile = path.CombineWithFilePath("target.txt");
    EnsureDirectoryExist(path);
    EnsureFileExist(sourceFile);

    // When
    CopyFile(sourceFile, targetFile);

    // Then
    Assert.True(System.IO.File.Exists(targetFile.FullPath));
});

Task("Cake.Common.IO.FileAliases.CopyFiles.Pattern")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/Pattern/CopyFiles/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/Pattern/CopyFiles/target");
    var sourceFiles = new FilePath[] {
        sourcePath.CombineWithFilePath("Cake.txt"),
        sourcePath.CombineWithFilePath("Cake.Common.txt"),
        sourcePath.CombineWithFilePath("Cake.Core.txt"),
        sourcePath.CombineWithFilePath("Cake.NuGet.txt"),
        sourcePath.CombineWithFilePath("Autofac.txt"),
        sourcePath.CombineWithFilePath("Mono.CSharp.txt"),
        sourcePath.CombineWithFilePath("NuGet.Core.txt")
    };
    var targetCopyFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Cake.txt"),
        targetPath.CombineWithFilePath("Cake.Common.txt"),
        targetPath.CombineWithFilePath("Cake.Core.txt"),
        targetPath.CombineWithFilePath("Cake.NuGet.txt")
    };
    var targetDontCopyFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Autofac.txt"),
        targetPath.CombineWithFilePath("Mono.CSharp.txt"),
        targetPath.CombineWithFilePath("NuGet.Core.txt")
    };
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFilesExist(sourceFiles);

    // When
    CopyFiles(sourcePath.FullPath + "/Cake.*", targetPath);

    // Then
    Assert.True(AllExist(targetCopyFiles));
    Assert.False(AnyExist(targetDontCopyFiles));
});

Task("Cake.Common.IO.FileAliases.CopyFiles.FilePaths")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFiles/FilePaths/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFiles/FilePaths/target");
    var sourceFiles = new FilePath[] {
        sourcePath.CombineWithFilePath("Cake.txt"),
        sourcePath.CombineWithFilePath("Cake.Common.txt"),
        sourcePath.CombineWithFilePath("Cake.Core.txt"),
        sourcePath.CombineWithFilePath("Cake.NuGet.txt")
    };
    var targetFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Cake.txt"),
        targetPath.CombineWithFilePath("Cake.Common.txt"),
        targetPath.CombineWithFilePath("Cake.Core.txt"),
        targetPath.CombineWithFilePath("Cake.NuGet.txt")
    };
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFilesExist(sourceFiles);

    // When
    CopyFiles(sourceFiles, targetPath);

    // Then
    Assert.True(AllExist(targetFiles));
});

Task("Cake.Common.IO.FileAliases.CopyFiles.Strings")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFiles/Strings/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/CopyFiles/Strings/target");
    var sourceFiles = new FilePath[] {
        sourcePath.CombineWithFilePath("Cake.txt"),
        sourcePath.CombineWithFilePath("Cake.Common.txt"),
        sourcePath.CombineWithFilePath("Cake.Core.txt"),
        sourcePath.CombineWithFilePath("Cake.NuGet.txt")
    };
    var sourceStrings = sourceFiles.Select(file=>file.FullPath).ToArray();
    var targetFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Cake.txt"),
        targetPath.CombineWithFilePath("Cake.Common.txt"),
        targetPath.CombineWithFilePath("Cake.Core.txt"),
        targetPath.CombineWithFilePath("Cake.NuGet.txt")
    };
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFilesExist(sourceFiles);

    // When
    CopyFiles(sourceStrings, targetPath);

    // Then
    Assert.True(AllExist(targetFiles));
});

Task("Cake.Common.IO.FileAliases.DeleteFiles.Pattern")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/DeleteFiles/Pattern");
    var deleteFiles = new FilePath[] {
        path.CombineWithFilePath("Cake.txt"),
        path.CombineWithFilePath("Cake.Common.txt"),
        path.CombineWithFilePath("Cake.Core.txt"),
        path.CombineWithFilePath("Cake.NuGet.txt")
    };
    var keepFiles = new FilePath[] {
        path.CombineWithFilePath("Autofac.txt"),
        path.CombineWithFilePath("Mono.CSharp.txt"),
        path.CombineWithFilePath("NuGet.Core.txt")
    };
    EnsureDirectoryExist(path);
    EnsureFilesExist(deleteFiles.Concat(keepFiles));

    // When
    DeleteFiles(path.FullPath + "/Cake.*");

    // Then
    Assert.False(AnyExist(deleteFiles));
    Assert.True(AllExist(keepFiles));
});

Task("Cake.Common.IO.FileAliases.DeleteFiles.FilePaths")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/DeleteFiles/FilePaths");
    var deleteFiles = new FilePath[] {
        path.CombineWithFilePath("Cake.txt"),
        path.CombineWithFilePath("Cake.Common.txt"),
        path.CombineWithFilePath("Cake.Core.txt"),
        path.CombineWithFilePath("Cake.NuGet.txt")
    };
    var keepFiles = new FilePath[] {
        path.CombineWithFilePath("Autofac.txt"),
        path.CombineWithFilePath("Mono.CSharp.txt"),
        path.CombineWithFilePath("NuGet.Core.txt")
    };
    EnsureDirectoryExist(path);
    EnsureFilesExist(deleteFiles.Concat(keepFiles));

    // When
    DeleteFiles(deleteFiles);

    // Then
    Assert.False(AnyExist(deleteFiles));
    Assert.True(AllExist(keepFiles));
});

Task("Cake.Common.IO.FileAliases.DeleteFile")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/DeleteFile");
    var file = path.CombineWithFilePath("file.txt");
    EnsureDirectoryExist(path);
    EnsureFileExist(file);

    // When
    DeleteFile(file);

    // Then
    Assert.False(System.IO.File.Exists(file.FullPath));
});

Task("Cake.Common.IO.FileAliases.FileExists")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/FileExists");
    var file = path.CombineWithFilePath("file.txt");
    EnsureDirectoryExist(path);
    EnsureFileExist(file);

    // When
    var result = FileExists(file);

    // Then
    Assert.True(result);
});

Task("Cake.Common.IO.FileAliases.FileSize")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/FileSize");
    var file = path.CombineWithFilePath("file.txt");
    var message = "FileSizeTestData";
    var expect = Encoding.UTF8.GetByteCount(message + Environment.NewLine);
    EnsureDirectoryExist(path);
    EnsureFileExist(file, message);

    // When
    var result = FileSize(file);

    // Then
    Assert.Equal(expect, result);
});

Task("Cake.Common.IO.FileAliases.MoveFileToDirectory")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/MoveFileToDirectory/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/MoveFileToDirectory/target");
    var sourceFile = sourcePath.CombineWithFilePath("test.file");
    var targetFile = targetPath.CombineWithFilePath("test.file");
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFileExist(sourceFile);

    // When
    MoveFileToDirectory(sourceFile, targetPath);

    // Then
    Assert.True(System.IO.File.Exists(targetFile.FullPath));
    Assert.False(System.IO.File.Exists(sourceFile.FullPath));
});

Task("Cake.Common.IO.FileAliases.MoveFiles.Pattern")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/Pattern/MoveFiles/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/Pattern/MoveFiles/target");
    var sourceMoveFiles = new FilePath[] {
        sourcePath.CombineWithFilePath("Cake.txt"),
        sourcePath.CombineWithFilePath("Cake.Common.txt"),
        sourcePath.CombineWithFilePath("Cake.Core.txt"),
        sourcePath.CombineWithFilePath("Cake.NuGet.txt")
    };
    var sourceDontMoveFiles = new FilePath[] {
        sourcePath.CombineWithFilePath("Autofac.txt"),
        sourcePath.CombineWithFilePath("Mono.CSharp.txt"),
        sourcePath.CombineWithFilePath("NuGet.Core.txt")
    };
    var targetMoveFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Cake.txt"),
        targetPath.CombineWithFilePath("Cake.Common.txt"),
        targetPath.CombineWithFilePath("Cake.Core.txt"),
        targetPath.CombineWithFilePath("Cake.NuGet.txt")
    };
    var targetDontMoveFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Autofac.txt"),
        targetPath.CombineWithFilePath("Mono.CSharp.txt"),
        targetPath.CombineWithFilePath("NuGet.Core.txt")
    };
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFilesExist(sourceMoveFiles.Union(sourceDontMoveFiles));

    // When
    MoveFiles(sourcePath.FullPath + "/Cake.*", targetPath);

    // Then
    Assert.False(AnyExist(sourceMoveFiles));
    Assert.True(AllExist(sourceDontMoveFiles));
    Assert.True(AllExist(targetMoveFiles));
    Assert.False(AnyExist(targetDontMoveFiles));
});

Task("Cake.Common.IO.FileAliases.MoveFiles.FilePaths")
    .Does(() =>
{
    // Given
    var sourcePath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/MoveFiles/FilePaths/source");
    var targetPath = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/MoveFiles/FilePaths/target");
    var sourceFiles = new FilePath[] {
        sourcePath.CombineWithFilePath("Cake.txt"),
        sourcePath.CombineWithFilePath("Cake.Common.txt"),
        sourcePath.CombineWithFilePath("Cake.Core.txt"),
        sourcePath.CombineWithFilePath("Cake.NuGet.txt")
    };
    var targetFiles = new FilePath[] {
        targetPath.CombineWithFilePath("Cake.txt"),
        targetPath.CombineWithFilePath("Cake.Common.txt"),
        targetPath.CombineWithFilePath("Cake.Core.txt"),
        targetPath.CombineWithFilePath("Cake.NuGet.txt")
    };
    EnsureDirectoriesExist(new DirectoryPath[] { sourcePath, targetPath });
    EnsureFilesExist(sourceFiles);

    // When
    MoveFiles(sourceFiles, targetPath);

    // Then
    Assert.False(AnyExist(sourceFiles));
    Assert.True(AllExist(targetFiles));
});

Task("Cake.Common.IO.FileAliases.MoveFile")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./Cake.Common.IO.FileAliases/MoveFile");
    var sourceFile = path.CombineWithFilePath("source.txt");
    var targetFile = path.CombineWithFilePath("target.txt");
    EnsureDirectoryExist(path);
    EnsureFileExist(sourceFile);

    // When
    MoveFile(sourceFile, targetFile);

    // Then
    Assert.False(System.IO.File.Exists(sourceFile.FullPath));
    Assert.True(System.IO.File.Exists(targetFile.FullPath));
});


//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.FileAliases")
    .IsDependentOn("Cake.Common.IO.FileAliases.CopyFileToDirectory")
    .IsDependentOn("Cake.Common.IO.FileAliases.CopyFile")
    .IsDependentOn("Cake.Common.IO.FileAliases.CopyFiles.Pattern")
    .IsDependentOn("Cake.Common.IO.FileAliases.CopyFiles.FilePaths")
    .IsDependentOn("Cake.Common.IO.FileAliases.CopyFiles.Strings")
    .IsDependentOn("Cake.Common.IO.FileAliases.DeleteFiles.Pattern")
    .IsDependentOn("Cake.Common.IO.FileAliases.DeleteFiles.FilePaths")
    .IsDependentOn("Cake.Common.IO.FileAliases.DeleteFile")
    .IsDependentOn("Cake.Common.IO.FileAliases.FileExists")
    .IsDependentOn("Cake.Common.IO.FileAliases.FileSize")
    .IsDependentOn("Cake.Common.IO.FileAliases.MoveFileToDirectory")
    .IsDependentOn("Cake.Common.IO.FileAliases.MoveFiles.Pattern")
    .IsDependentOn("Cake.Common.IO.FileAliases.MoveFiles.FilePaths")
    .IsDependentOn("Cake.Common.IO.FileAliases.MoveFile");