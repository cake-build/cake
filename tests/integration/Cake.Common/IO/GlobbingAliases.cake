#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"
#load "./../../utilities/io.cake"

//////////////////////////////////////////////////////////////////////////////

public static void AssertFiles(this FilePathCollection collection, params FilePath[] files)
{
    Assert.NotNull(collection);
    Assert.Equal(files.Length, collection.Count);
    foreach (var file in files)
    {
        Assert.True(collection.Contains(file, PathComparer.Default), $"Expected '{file}' to be found by globber.");
    }
}

public static void AssertDirectories(this DirectoryPathCollection collection, params DirectoryPath[] directories)
{
    Assert.NotNull(collection);
    Assert.Equal(directories.Length, collection.Count);
    foreach (var directory in directories)
    {
        Assert.True(collection.Contains(directory, PathComparer.Default), $"Expected '{directory}' to be found by globber.");
    }
}

public static void AssertPaths(this PathCollection collection, params Cake.Core.IO.Path[] paths)
{
    Assert.NotNull(collection);
    Assert.Equal(paths.Length, collection.Count);
    foreach (var path in paths)
    {
        Assert.True(collection.Contains(path, PathComparer.Default), $"Expected '{path}' to be found by globber.");
    }
}

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.GlobbingAliases.GetFiles.Wildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/wildcard");
    var foobaa = EnsureFileExist(root.CombineWithFilePath("foobaa.txt"));
    var foobao = EnsureFileExist(root.CombineWithFilePath("foobao.txt"));
    var foobau = EnsureFileExist(root.CombineWithFilePath("foobau.txt"));

    // When
    var files = GetFiles($"{root}/*");

    // Then
    files.AssertFiles(foobaa, foobao, foobau);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.Wildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/wildcard");
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var directories = GetDirectories($"{root}/*");

    // Then
    directories.AssertDirectories(foobar, foobaz, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.Wildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/wildcard");
    var foobaa = EnsureFileExist(root.CombineWithFilePath("foobaa.txt"));
    var foobao = EnsureFileExist(root.CombineWithFilePath("foobao.txt"));
    var foobau = EnsureFileExist(root.CombineWithFilePath("foobau.txt"));
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var paths = GetPaths($"{root}/*");

    // Then
    paths.AssertPaths(foobaa, foobao, foobau, foobar, foobaz, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetFiles.RecursiveWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/recursivewildcard");
    var first = EnsureFileExist(root.CombineWithFilePath("foo/bar/qux.txt"));
    var second = EnsureFileExist(root.CombineWithFilePath("bar/qux.txt"));
    var third = EnsureFileExist(root.CombineWithFilePath("bar/foo/baz.txt"));

    // When
    var files = GetFiles($"{root}/**/qux.txt");

    // Then
    files.AssertFiles(first, second);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.RecursiveWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/recursivewildcard");
    var first = EnsureDirectoryExist(root.Combine("foo/bar/qux"));
    var second = EnsureDirectoryExist(root.Combine("bar/qux"));
    var third = EnsureDirectoryExist(root.Combine("bar/foo/baz"));

    // When
    var files = GetDirectories($"{root}/**/qux");

    // Then
    files.AssertDirectories(first, second);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.RecursiveWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/recursivewildcard");
    var firstf = EnsureFileExist(root.CombineWithFilePath("foo/boo/qux"));
    var secondf = EnsureFileExist(root.CombineWithFilePath("boo/qux"));
    var thirdf = EnsureFileExist(root.CombineWithFilePath("boo/foo/baz"));
    var firstd = EnsureDirectoryExist(root.Combine("foo/bar/qux"));
    var secondd = EnsureDirectoryExist(root.Combine("bar/qux"));
    var thirdd = EnsureDirectoryExist(root.Combine("bar/foo/baz"));

    // When
    var paths = GetPaths($"{root}/**/qux");

    // Then
    paths.AssertPaths(firstf, secondf, firstd, secondd);
});

Task("Cake.Common.IO.GlobbingAliases.GetFiles.CharacterWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/characterwildcard");
    var foobar = EnsureFileExist(root.CombineWithFilePath("foobar.txt"));
    var foobaz = EnsureFileExist(root.CombineWithFilePath("foobaz.txt"));
    var foobax = EnsureFileExist(root.CombineWithFilePath("foobax.txt"));

    // When
    var files = GetFiles($"{root}/fooba?.txt");

    // Then
    files.AssertFiles(foobar, foobaz, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.CharacterWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/characterwildcard");
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var files = GetDirectories($"{root}/fooba?");

    // Then
    files.AssertDirectories(foobar, foobaz, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.CharacterWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/characterwildcard");
    var foobaa = EnsureFileExist(root.CombineWithFilePath("foobaa"));
    var foobao = EnsureFileExist(root.CombineWithFilePath("foobao"));
    var foobau = EnsureFileExist(root.CombineWithFilePath("foobau"));
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var paths = GetPaths($"{root}/fooba?");

    // Then
    paths.AssertPaths(foobaa, foobao, foobau, foobar, foobaz, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetFiles.BracketWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/bracketwildcard");
    var foobar = EnsureFileExist(root.CombineWithFilePath("foobar.txt"));
    var foobaz = EnsureFileExist(root.CombineWithFilePath("foobaz.txt"));
    var foobax = EnsureFileExist(root.CombineWithFilePath("foobax.txt"));

    // When
    var files = GetFiles($"{root}/fooba[rz].txt");

    // Then
    files.AssertFiles(foobar, foobaz);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.BracketWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/bracketwildcard");
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var files = GetDirectories($"{root}/fooba[rz]");

    // Then
    files.AssertDirectories(foobar, foobaz);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.BracketWildcard")
    .Does(context =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/bracketwildcard");
    var foobaa = EnsureFileExist(root.CombineWithFilePath("foobaa"));
    var foobao = EnsureFileExist(root.CombineWithFilePath("foobao"));
    var foobau = EnsureFileExist(root.CombineWithFilePath("foobau"));
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var paths = GetPaths($"{root}/fooba[aorz]");

    // Then
    paths.AssertPaths(foobaa, foobao, foobar, foobaz);
});

Task("Cake.Common.IO.GlobbingAliases.GetFiles.BraceExpansion")
    .Does(() =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansion");
    var foobar = EnsureFileExist(root.CombineWithFilePath("foobar.txt"));
    var foobaz = EnsureFileExist(root.CombineWithFilePath("foobaz.txt"));
    var foobax = EnsureFileExist(root.CombineWithFilePath("foobax.txt"));

    // When
    var files = GetFiles($"{root}/foo{{bar,bax}}.txt");

    // Then
    files.AssertFiles(foobar, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.BraceExpansion")
    .Does(() =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansion");
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var files = GetDirectories($"{root}/foo{{bar,bax}}");

    // Then
    files.AssertDirectories(foobar, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.BraceExpansion")
    .Does(() =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansion");
    var foobaa = EnsureFileExist(root.CombineWithFilePath("foobaa"));
    var foobao = EnsureFileExist(root.CombineWithFilePath("foobao"));
    var foobau = EnsureFileExist(root.CombineWithFilePath("foobau"));
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var paths = GetPaths($"{root}/foo{{baa,bau,bar,bax}}");

    // Then
    paths.AssertPaths(foobaa, foobau, foobar, foobax);
});

Task("Cake.Common.IO.GlobbingAliases.GetFiles.BraceExpansionOnlySegment")
    .Does(() =>
{
    // Given (GitHub issue #2666: GetFiles with glob curly braces as only path segment)
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansiononlysegment");
    var packageJson = EnsureFileExist(root.CombineWithFilePath("package.json"));
    var packageLock = EnsureFileExist(root.CombineWithFilePath("package-lock.json"));
    var tsconfig = EnsureFileExist(root.CombineWithFilePath("tsconfig.json"));

    // When
    var files = GetFiles($"{root}/{{package.json,package-lock.json,tsconfig.json}}");

    // Then
    files.AssertFiles(packageJson, packageLock, tsconfig);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.BraceExpansionOnlySegment")
    .Does(() =>
{
    // Given (GitHub issue #2666: GetDirectories with glob curly braces as only path segment)
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansiononlysegment");
    var dir1 = EnsureDirectoryExist(root.Combine("config"));
    var dir2 = EnsureDirectoryExist(root.Combine("src"));
    var dir3 = EnsureDirectoryExist(root.Combine("tools"));

    // When
    var directories = GetDirectories($"{root}/{{config,src}}");

    // Then
    directories.AssertDirectories(dir1, dir2);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.BraceExpansionOnlySegment")
    .Does(() =>
{
    // Given (GitHub issue #2666: GetPaths with glob curly braces as only path segment)
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansiononlysegment");
    var readme = EnsureFileExist(root.CombineWithFilePath("README.md"));
    var license = EnsureFileExist(root.CombineWithFilePath("LICENSE"));
    var docs = EnsureDirectoryExist(root.Combine("docs"));

    // When
    var paths = GetPaths($"{root}/{{README.md,LICENSE,docs}}");

    // Then
    paths.AssertPaths(readme, license, docs);
});

Task("Cake.Common.IO.GlobbingAliases.GetFiles.BraceExpansionNegation")
    .Does(() =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansionnegation");
    var foobar = EnsureFileExist(root.CombineWithFilePath("foobar.txt"));
    var foobaz = EnsureFileExist(root.CombineWithFilePath("foobaz.txt"));
    var foobax = EnsureFileExist(root.CombineWithFilePath("foobax.txt"));

    // When
    var files = GetFiles($"{root}/fooba[!x].txt");

    // Then
    files.AssertFiles(foobar, foobaz);
});

Task("Cake.Common.IO.GlobbingAliases.GetDirectories.BraceExpansionNegation")
    .Does(() =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansionnegation");
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var files = GetDirectories($"{root}/fooba[!x]");

    // Then
    files.AssertDirectories(foobar, foobaz);
});

Task("Cake.Common.IO.GlobbingAliases.GetPaths.BraceExpansionNegation")
    .Does(() =>
{
    // Given
    var root = EnsureDirectoryExist($"{Paths.Temp}/Cake.Common.IO.GlobbingAliases/braceexpansionnegation");
    var foobaa = EnsureFileExist(root.CombineWithFilePath("foobaa"));
    var foobao = EnsureFileExist(root.CombineWithFilePath("foobao"));
    var foobau = EnsureFileExist(root.CombineWithFilePath("foobau"));
    var foobar = EnsureDirectoryExist(root.Combine("foobar"));
    var foobaz = EnsureDirectoryExist(root.Combine("foobaz"));
    var foobax = EnsureDirectoryExist(root.Combine("foobax"));

    // When
    var paths = GetPaths($"{root}/fooba[!ux]");

    // Then
    paths.AssertPaths(foobaa, foobao, foobar, foobaz);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.IO.GlobbingAliases")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.Wildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.Wildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.Wildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.RecursiveWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.RecursiveWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.RecursiveWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.CharacterWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.CharacterWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.CharacterWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.BracketWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.BracketWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.BracketWildcard")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.BraceExpansion")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.BraceExpansion")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.BraceExpansion")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.BraceExpansionOnlySegment")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.BraceExpansionOnlySegment")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.BraceExpansionOnlySegment")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetFiles.BraceExpansionNegation")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetDirectories.BraceExpansionNegation")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases.GetPaths.BraceExpansionNegation");
