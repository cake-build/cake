#load "./../scripts/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.ArgumentAliases.HasArgument")
    .Does(() =>
{
    // Given, When
    var result = HasArgument("customarg");

    // Then
    Assert.True(result);
});

Task("Cake.Common.ArgumentAliases.Argument")
    .Does(() =>
{
    // Given, When
    var arg = Argument<string>("customarg");

    // Then
    Assert.Equal("hello", arg);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.ArgumentAliases")
  .IsDependentOn("Cake.Common.ArgumentAliases.HasArgument")
  .IsDependentOn("Cake.Common.ArgumentAliases.Argument");

