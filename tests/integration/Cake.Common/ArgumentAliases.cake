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
    Assert.Equal(new[] {"a", "b"}, arg);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.ArgumentAliases")
  .IsDependentOn("Cake.Common.ArgumentAliases.HasArgument")
  .IsDependentOn("Cake.Common.ArgumentAliases.HasArgument.ThatDoNotExist")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.WithDefaultValue")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument.MultipleArguments");
