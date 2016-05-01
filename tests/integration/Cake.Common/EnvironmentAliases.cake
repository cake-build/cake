#load "./../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.EnvironmentAliases.HasEnvironmentVariable")
    .Does(() =>
{
    // Given, When
    var result = HasEnvironmentVariable("MyEnvironmentVariable");
    
    // Then
    Assert.True(result);
});

Task("Cake.Common.EnvironmentAliases.EnvironmentVariable")
    .Does(() =>
{
    // Given, When
    var result = EnvironmentVariable("MyEnvironmentVariable");

    // Then
    Assert.Equal("Hello World", result);
});

Task("Cake.Common.EnvironmentAliases.EnvironmentVariables")
    .Does(() =>
{
    // Given, When
    var result = EnvironmentVariables();

    // Then
    Assert.NotNull(result);
    Assert.True(result.Count > 0);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.EnvironmentAliases")
    .IsDependentOn("Cake.Common.EnvironmentAliases.HasEnvironmentVariable")
    .IsDependentOn("Cake.Common.EnvironmentAliases.EnvironmentVariable")
    .IsDependentOn("Cake.Common.EnvironmentAliases.EnvironmentVariables");
