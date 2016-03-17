// Utilities
#load "./scripts/directory.cake"
#load "./scripts/paths.cake"
#load "./scripts/xunit.cake"

// Tests
#load "./Cake.Common/ArgumentAliases.cake"
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
    .IsDependentOn("Cake.Common.IO.DirectoryAliases");

Task("Run-All-Tests")
    .IsDependentOn("Cake.Common");

//////////////////////////////////////////////////

RunTarget(target);