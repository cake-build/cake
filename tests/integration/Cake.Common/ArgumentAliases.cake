#load "./../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.ArgumentAliases.HasArgument")
    .Does(() =>
{
    // Given, When
    var arg = HasArgument("customarg");

    // Then
    Assert.True(arg);
});

Task("Cake.Common.ArgumentAliases.HasArgument.ThatDoNotExist")
    .Does(() =>
{
    // Given, When
    var arg = HasArgument("nonexisting");

    // Then
    Assert.False(arg);
});

Task("Cake.Common.ArgumentAliases.Argument")
    .Does(() =>
{
    // Given, When
    var arg = Argument<string>("customarg");

    // Then
    Assert.Equal("hello", arg);
});

Task("Cake.Common.ArgumentAliases.Argument.WithDefaultValue")
    .Does(() =>
{
    // Given, When
    var arg = Argument<string>("nonexisting", "foo");

    // Then
    Assert.Equal("foo", arg);
});

Task("Cake.Common.ArgumentAliases.Argument.MultipleArguments")
    .Does(() =>
{

    // Given, When
    var arg = Arguments<string>("multipleargs");

    // Then
    Assert.Equal(new[] { "a", "b" }, arg);
});

Task("Cake.Common.ArgumentAliases.Argument.MultipleArguments.WithSingleDefaultValue")
    .Does(() =>
{
    // Given
    var expect = new[] { "a" };

    // When
    var arg = Arguments<string>("nonexistingmultipleargs", expect[0]);

    // Then
    Assert.Equal(expect, arg);
});

Task("Cake.Common.ArgumentAliases.Argument.MultipleArguments.WithMultipleDefaultValue")
    .Does(() =>
{
    // Given
    var expect = new[] { "a", "b" };

    // When
    var arg = Arguments<string>("nonexistingmultipleargs", expect);

    // Then
    Assert.Equal(expect, arg);
});

Task("Cake.Common.ArgumentAliases.Argument.MultipleArguments.WithLazyDefaultValue")
    .Does(() =>
{
    // Given
    var expect = new[] { "a", "b" };

    // When
    var arg = Arguments<string>("nonexistingmultipleargs", _ => expect);

    // Then
    Assert.Equal(expect, arg);
});

Task("Cake.Common.ArgumentAliases.Argument.DirectoryPathArgument")
    .Does(() =>
{
    // Given, When
    var arg = Argument<DirectoryPath>("testAssemblyDirectoryPath");

    // Then
    Assert.Equal(Context.Environment.ApplicationRoot.FullPath, arg.FullPath);
});

Task("Cake.Common.ArgumentAliases.Argument.FilePathArgument")
    .Does(() =>
{
    // Given
    var testAssemblyPath = Context
                            .Environment
                            .ApplicationRoot
                            .CombineWithFilePath("Cake.dll");

    // When
    var arg = Argument<FilePath>("testAssemblyFilePath");

    // Then
    Assert.Equal(testAssemblyPath.FullPath, arg.FullPath);
});

Task("Cake.Common.ArgumentAliases.Argument.DotNetCoreVerbosityArgument")
    .Does(() =>
{
    // Given, When
    var arg = Argument<DotNetCoreVerbosity>("testDotNetCoreVerbosity");

    // Then
    Assert.Equal(DotNetCoreVerbosity.Diagnostic, arg);
});

Task("Cake.Common.ArgumentAliases.Argument.DotNetCoreRollForwardArgument")
    .Does(() =>
{
    // Given, When
    var arg = Argument<DotNetCoreRollForward>("testDotNetRollForward");

    // Then
    Assert.Equal(DotNetCoreRollForward.LatestMajor, arg);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.ArgumentAliases")
  .IsDependentOn("Cake.Common.ArgumentAliases.HasArgument")
  .IsDependentOn("Cake.Common.ArgumentAliases.HasArgument.ThatDoNotExist")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.WithDefaultValue")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.MultipleArguments")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.MultipleArguments.WithSingleDefaultValue")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.MultipleArguments.WithMultipleDefaultValue")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.MultipleArguments.WithLazyDefaultValue")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.DirectoryPathArgument")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.FilePathArgument")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.DotNetCoreVerbosityArgument")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.DotNetCoreRollForwardArgument")
;
