#load "./../../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Tooling.ToolLocator.RegisterFile")
    .Does(() =>
{
    // Given, When, Then
    Context.Tools.RegisterFile("lol.txt");
});

Task("Cake.Core.Tooling.ToolLocator.Resolve")
    .Does(() =>
{
    // Given
    Context.Tools.RegisterFile("./../../build.ps1");
    
    // When
    var result = Context.Tools.Resolve("build.ps1");
    
    // Then
    Assert.NotNull(result);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Tooling.ToolLocator")
    .IsDependentOn("Cake.Core.Tooling.ToolLocator.RegisterFile")
    .IsDependentOn("Cake.Core.Tooling.ToolLocator.Resolve");
