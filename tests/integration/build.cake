// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"

// Tests
#load "./Cake.Common/ArgumentAliases.cake"
#load "./Cake.Common/EnvironmentAliases.cake"
#load "./Cake.Common/IO/DirectoryAliases.cake"

//////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////

var target = Argument<string>("target", "Run-All-Tests");

//////////////////////////////////////////////////
// SETUP / TEARDOWN
//////////////////////////////////////////////////

Setup(() =>
{
    CleanDirectory(Paths.Temp);
});

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Cake.Common")
    .IsDependentOn("Cake.Common.ArgumentAliases")
    .IsDependentOn("Cake.Common.EnvironmentAliases")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases");

Task("Run-All-Tests")
    .IsDependentOn("Cake.Common");

//////////////////////////////////////////////////

RunTarget(target);