// Utilities
#load "./utilities/paths.cake"
#load "./utilities/xunit.cake"
#load "./utilities/context.cake"

// Tests
#load "setup.cake"
#load "teardown.cake"
#load "./Cake/ScriptCache.cake"
#load "./Cake.Common/ArgumentAliases.cake"
#load "./Cake.Common/Build/BuildSystemAliases.cake"
#load "./Cake.Common/EnvironmentAliases.cake"
#load "./Cake.Common/Diagnostics/LoggingAliases.cake"
#load "./Cake.Common/IO/DirectoryAliases.cake"
#load "./Cake.Common/IO/FileAliases.cake"
#load "./Cake.Common/IO/FileAsync.cake"
#load "./Cake.Common/IO/GlobbingAliases.cake"
#load "./Cake.Common/IO/ZipAliases.cake"
#load "./Cake.Common/ProcessAliases.cake"
#load "./Cake.Common/ReleaseNotesAliases.cake"
#load "./Cake.Common/Security/SecurityAliases.cake"
#load "./Cake.Common/Solution/SolutionAliases.cake"
#load "./Cake.Common/Solution/Project/ProjectAliases.cake"
#load "./Cake.Common/Solution/Project/Properties/AssemblyInfoAliases.cake"
#load "./Cake.Common/Solution/Project/XmlDoc/XmlDocAliases.cake"
#load "./Cake.Common/Text/TextTransformationAliases.cake"
#load "./Cake.Common/Tools/Cake/CakeAliases.cake"
#load "./Cake.Common/Tools/DotNet/DotNetAliases.cake"
#load "./Cake.Common/Tools/DotNetCore/DotNetCoreAliases.cake"
#load "./Cake.Common/Tools/NuGet/NuGetAliases.cake"
#load "./Cake.Common/Tools/Chocolatey/ChocolateyAliases.cake"
#load "./Cake.Common/Tools/TextTransform/TextTransformAliases.cake"
#load "./Cake.Core/Diagnostics/ICakeLog.cake"
#load "./Cake.Core/IO/Path.cake"
#load "./Cake.Core/Scripting/AddinDirective.cake"
#load "./Cake.Core/Scripting/DefineDirective.cake"
#load "./Cake.Core/Scripting/Dynamic.cake"
#load "./Cake.Core/Scripting/HttpClient.cake"
#load "./Cake.Core/Scripting/LoadDirective.cake"
#load "./Cake.Core/Scripting/SystemCollections.cake"
#load "./Cake.Core/Scripting/UsingDirective.cake"
#load "./Cake.Core/Scripting/SpectreConsole.cake"
#load "./Cake.Core/Tooling/ToolLocator.cake"
#load "./Cake.Core/CakeAliases.cake"
#load "./Cake.DotNetTool.Module/Cake.DotNetTool.Module.cake"
#load "./Cake.NuGet/InProcessInstaller.cake"

//////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////

var target = Argument<string>("target", "Run-All-Tests");

//////////////////////////////////////////////////
// TARGETS
//////////////////////////////////////////////////

Task("Cake")
    .IsDependentOn("Cake.ScriptCache");

Task("Cake.Core")
    .IsDependentOn("Cake.Core.Diagnostics")
    .IsDependentOn("Cake.Core.IO.Path")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective")
    .IsDependentOn("Cake.Core.Scripting.DefineDirective")
    .IsDependentOn("Cake.Core.Scripting.Dynamic")
    .IsDependentOn("Cake.Core.Scripting.HttpClient")
    .IsDependentOn("Cake.Core.Scripting.LoadDirective")
    .IsDependentOn("Cake.Core.Scripting.SystemCollections")
    .IsDependentOn("Cake.Core.Scripting.UsingDirective")
    .IsDependentOn("Cake.Core.Scripting.Spectre.Console")
    .IsDependentOn("Cake.Core.Tooling.ToolLocator")
    .IsDependentOn("Cake.Core.CakeAliases");

Task("Cake.Common")
    .IsDependentOn("Cake.Common.ArgumentAliases")
    .IsDependentOn("Cake.Common.Build.BuildSystemAliases")
    .IsDependentOn("Cake.Common.EnvironmentAliases")
    .IsDependentOn("Cake.Common.Diagnostics.LoggingAliases")
    .IsDependentOn("Cake.Common.IO.DirectoryAliases")
    .IsDependentOn("Cake.Common.IO.FileAliases")
    .IsDependentOn("Cake.Common.IO.FileAsync")
    .IsDependentOn("Cake.Common.IO.GlobbingAliases")
    .IsDependentOn("Cake.Common.IO.ZipAliases")
    .IsDependentOn("Cake.Common.ProcessAliases")
    .IsDependentOn("Cake.Common.ReleaseNotesAliases")
    .IsDependentOn("Cake.Common.Security.SecurityAliases")
    .IsDependentOn("Cake.Common.Solution.SolutionAliases")
    .IsDependentOn("Cake.Common.Solution.Project.ProjectAliases")
    .IsDependentOn("Cake.Common.Solution.Project.Properties.AssemblyInfoAliases")
    .IsDependentOn("Cake.Common.Solution.Project.XmlDoc.XmlDocAliases")
    .IsDependentOn("Cake.Common.Text.TextTransformationAliases")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases")
    .IsDependentOn("Cake.Common.Tools.DotNet.DotNetAliases")
    .IsDependentOn("Cake.Common.Tools.DotNetCore.DotNetCoreAliases")
    .IsDependentOn("Cake.Common.Tools.NuGet.NuGetAliases")
    .IsDependentOn("Cake.Common.Tools.TextTransform.TextTransformAliases");

Task("Cake.NuGet")
    .IsDependentOn("Cake.NuGet.InProcessInstaller");

Task("Cake.Chocolatey")
    .IsDependentOn("Cake.Common.Tools.Chocolatey.ChocolateyAliases");

Task("Run-All-Tests")
    .IsDependentOn("Setup-Tests")
    .IsDependentOn("Cake")
    .IsDependentOn("Cake.Core")
    .IsDependentOn("Cake.Common")
    .IsDependentOn("Cake.DotNetTool.Module")
    .IsDependentOn("Cake.NuGet")
    .IsDependentOn("Cake.Chocolatey");

//////////////////////////////////////////////////

RunTarget(target);
