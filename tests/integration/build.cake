// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"

// Tests
#load "./Cake.Common/ArgumentAliases.cake"
#load "./Cake.Common/EnvironmentAliases.cake"
#load "./Cake.Common/IO/DirectoryAliases.cake"
#load "./Cake.Common/IO/FileAliases.cake"
#load "./Cake.Common/ReleaseNotesAliases.cake"
#load "./Cake.Common/Solution/SolutionAliases.cake"
#load "./Cake.Common/Solution/Project/ProjectAliases.cake"
#load "./Cake.Common/Solution/Project/Properties/AssemblyInfoAliases.cake"
#load "./Cake.Common/Solution/Project/XmlDoc/XmlDocAliases.cake"
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
    .IsDependentOn("Cake.Common.IO.DirectoryAliases")
    .IsDependentOn("Cake.Common.IO.FileAliases")
    .IsDependentOn("Cake.Common.ReleaseNotesAliases")
    .IsDependentOn("Cake.Common.Solution.SolutionAliases")
    .IsDependentOn("Cake.Common.Solution.Project.ProjectAliases")
    .IsDependentOn("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases")
    .IsDependentOn("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases");

Task("Run-All-Tests")
    .IsDependentOn("Cake.Core")
    .IsDependentOn("Cake.Common");

//////////////////////////////////////////////////

RunTarget(target);