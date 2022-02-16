#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.DirectoryAliases.DirectoryExists")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./hello");
    EnsureDirectoryExist(path);

    // When
    var result = DirectoryExists(path);

    // Then
    Assert.True(result);
});

Task("Cake.Common.IO.DirectoryAliases.CleanDirectory")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./foo");
    var file = path.CombineWithFilePath("bar.baz");
    EnsureDirectoryExist(path);
    EnsureFileExist(file);

    // When
    CleanDirectory(path);

    // Then
    Assert.True(System.IO.Directory.Exists(path.FullPath));
    Assert.False(System.IO.File.Exists(file.FullPath));
});

Task("Cake.Common.IO.DirectoryAliases.CleanDirectory.Filter")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./foo");
    var file1 = path.CombineWithFilePath("bar.baz");
    var file2 = path.CombineWithFilePath("bar.qux");
    EnsureDirectoryExist(path);
    EnsureFilesExist(new FilePath[] { file1, file2 });

    // When
    CleanDirectory(path, f => f.Path.FullPath.EndsWith("baz"));

    // Then
    Assert.True(System.IO.Directory.Exists(path.FullPath));
    Assert.False(System.IO.File.Exists(file1.FullPath));
    Assert.True(System.IO.File.Exists(file2.FullPath));
});

Task("Cake.Common.IO.DirectoryAliases.CleanDirectories")
    .Does(() =>
{
    // Given
    var path1 = Paths.Temp.Combine("./foo");
    var file1 = path1.CombineWithFilePath("bar.baz");
    var path2 = Paths.Temp.Combine("./bar");
    var file2 = path2.CombineWithFilePath("qux.foo");
    EnsureDirectoriesExist(new DirectoryPath[] { path1, path2 });
    EnsureFilesExist(new FilePath[] { file1, file2 });

    // When
    CleanDirectories(new DirectoryPath[] { path1, path2 });

    // Then
    Assert.True(System.IO.Directory.Exists(path1.FullPath));
    Assert.False(System.IO.File.Exists(file1.FullPath));
    Assert.True(System.IO.Directory.Exists(path2.FullPath));
    Assert.False(System.IO.File.Exists(file2.FullPath));
});

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

Task("Cake.Common.IO.DirectoryAliases.DeleteDirectory")
    .Does(() =>
{
    // Given
    var path = Paths.Temp.Combine("./hello");
    EnsureDirectoryExist(path);

    // When
    DeleteDirectory(path,
        new DeleteDirectorySettings {
            Force = true
        });

    // Then
    Assert.False(System.IO.Directory.Exists(path.FullPath));
});

Task("Cake.Common.IO.DirectoryAliases.DeleteDirectory.Recurse")
    .Does(() =>
{
    // Given
    var root = Paths.Temp.Combine("./hello");
    var path = root.Combine("world");
    EnsureDirectoryExist(path);

    // When
    DeleteDirectory(root,
        new DeleteDirectorySettings {
            Force = true,
            Recursive = true
        });

    // Then
    Assert.False(System.IO.Directory.Exists(path.FullPath));
});

Task("Cake.Common.IO.DirectoryAliases.DeleteDirectories")
    .Does(() =>
{
    // Given
    var path1 = Paths.Temp.Combine("./hello");
    var path2 = Paths.Temp.Combine("./world");
    EnsureDirectoriesExist(new DirectoryPath[] { path1, path2 });

    // When
    DeleteDirectories(new DirectoryPath[] { path1, path2 },
        new DeleteDirectorySettings {
            Force = true
        });

    // Then
    Assert.False(System.IO.Directory.Exists(path1.FullPath));
    Assert.False(System.IO.Directory.Exists(path2.FullPath));
});

Task("Cake.Common.IO.DirectoryAliases.DeleteDirectories.Recurse")
    .Does(() =>
{
    // Given
    var root1 = Paths.Temp.Combine("./foo");
    var path1 = root1.Combine("bar");
    var root2 = Paths.Temp.Combine("./baz");
    var path2 = root2.Combine("qux");
    EnsureDirectoriesExist(new DirectoryPath[] { root1, path1, root2, path2 });

    // When
    DeleteDirectories(new DirectoryPath[] { root1, root2 },
        new DeleteDirectorySettings {
            Force = true,
            Recursive = true
        });

    // Then
    Assert.False(System.IO.Directory.Exists(path1.FullPath));
    Assert.False(System.IO.Directory.Exists(path2.FullPath));
});

Task("Cake.Common.IO.DirectoryAliases.MakeRelative.DefinedRootPath")
    .Does(() =>
{
    // Given
    var directoryPath = Paths.Temp.Combine("./hello/world");
    var filePath = Paths.Temp.Combine("./hello/world/test.cake");
    var rootPath1 = Paths.Temp;
    var rootPath2 = Paths.Temp.Combine("./cake/world");

    // When
    var relativeDirectoryPath1 = MakeRelative(directoryPath, rootPath1);
    var relativeDirectoryPath2 = MakeRelative(directoryPath, rootPath2);
    var relativeFilePath = MakeRelative(filePath, rootPath2);
    
    // Then
    Assert.Equal("hello/world", relativeDirectoryPath1.ToString());
    Assert.Equal("../../hello/world", relativeDirectoryPath2.ToString());
    Assert.Equal("../../hello/world/test.cake", relativeFilePath.ToString());
});

Task("Cake.Common.IO.DirectoryAliases.MakeRelative.WorkingDirectory")
    .Does(() =>
{
    // Given
    var directoryPath = Paths.Temp.Combine("./hello/world");
    var filePath = Paths.Temp.Combine("./hello/world/test.cake");

    // When
    var relativeDirectoryPath = MakeRelative(directoryPath);
    var relativeFilePath = MakeRelative(filePath);

    // Then
    Assert.Equal("temp/hello/world", relativeDirectoryPath.ToString());
    Assert.Equal("temp/hello/world/test.cake", relativeFilePath.ToString());
});

Task("Cake.Common.IO.Paths.ConvertableDirectoryPath.DirectoryPathPlusConvertableDirectoryPath")
    .Does(context =>
{
    // Given
    var directoryPath = new DirectoryPath("./root");

    // When
    var result = directoryPath + context.Directory("other");

    // Then
    Assert.Equal("root/other", result.ToString());
});

Task("Cake.Common.IO.Paths.ConvertableDirectoryPath.ConvertableDirectoryPathPlusDirectoryPath")
    .Does(context =>
{
    // Given
    var convertableDirectoryPath = context.Directory("./root");

    // When
    var result = convertableDirectoryPath + new DirectoryPath("other");

    // Then
    Assert.Equal("root/other", result.ToString());
});

Task("Cake.Common.IO.Paths.ConvertableDirectoryPath.ConvertableDirectoryPathPlusConvertableDirectoryPath")
    .Does(context =>
{
    // Given
    var convertableDirectoryPath = context.Directory("./root");

    // When
    var result = convertableDirectoryPath + context.Directory("other");

    // Then
    Assert.Equal("root/other", result.ToString());
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.DirectoryAliases")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.DirectoryExists")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.CleanDirectory")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.CleanDirectory.Filter")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.CleanDirectories")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.CreateDirectory")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.DeleteDirectory")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.DeleteDirectory.Recurse")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.DeleteDirectories")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.DeleteDirectories.Recurse")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.MakeRelative.DefinedRootPath")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases.MakeRelative.WorkingDirectory")
    .IsDependentOn("Cake.Common.IO.Paths.ConvertableDirectoryPath.DirectoryPathPlusConvertableDirectoryPath")
    .IsDependentOn("Cake.Common.IO.Paths.ConvertableDirectoryPath.ConvertableDirectoryPathPlusDirectoryPath")
    .IsDependentOn("Cake.Common.IO.Paths.ConvertableDirectoryPath.ConvertableDirectoryPathPlusConvertableDirectoryPath")
;
