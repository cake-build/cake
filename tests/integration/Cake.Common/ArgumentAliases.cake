#load "./../scripts/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.HasArgument")
    .Does(() =>
{
    // Given, When
    var arg = Argument<string>("customarg");
    
    // Then
    Assert.Equal("hello", arg);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.ArgumentAliases")
    .IsDependentOn("Cake.Common.HasArgument");
