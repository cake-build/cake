// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"

// Tests
#load "./Cake.Common/ArgumentAliases.cake"
#load "./Cake.Common/EnvironmentAliases.cake"
#load "./Cake.Common/IO/DirectoryAliases.cake"
#load "./Cake.Core/Tooling/ToolLocator.cake"

//////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////

var target = Argument<string>("target", "Run-All-Tests");

//////////////////////////////////////////////////
// SETUP / TEARDOWN
//////////////////////////////////////////////////

Setup(ctx =>
{
    CleanDirectory(Paths.Temp);
});

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Cake.Core")
    .IsDependentOn("Cake.Core.Tooling.ToolLocator");

Task("Cake.Common")
    .IsDependentOn("Cake.Common.ArgumentAliases")
    .IsDependentOn("Cake.Common.EnvironmentAliases")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases");

Task("Run-All-Tests")
    .IsDependentOn("Cake.Core")
    .IsDependentOn("Cake.Common");

//////////////////////////////////////////////////

RunTarget(target);