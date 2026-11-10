#load "./../../utilities/xunit.cake"
#load "./../../utilities/paths.cake"

//////////////////////////////////////////////////////////////////////////////
// Integration tests for on-demand InstallTool / InstallTools aliases.
// Installs inside a task (not via #tool). See https://github.com/cake-build/cake/issues/2471
//////////////////////////////////////////////////////////////////////////////

const string NuGetToolUri = "nuget:?package=xunit.runner.console&version=2.9.3";
const string DotNetToolUri = "dotnet:?package=GitVersion.Tool&version=6.6.0";

void RunInstallToolScript(string name, string script)
{
    var workingDirectory = Paths.Temp.Combine($"./Cake.Common/Tools/ToolAliases/{name}");
    EnsureDirectoryExist(workingDirectory);
    var scriptPath = workingDirectory.CombineWithFilePath("build.cake");
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    var scriptToolsPath = workingDirectory.Combine("tools");
    CakeExecuteScript(scriptPath,
        new CakeSettings
        {
            WorkingDirectory = workingDirectory,
            EnvironmentVariables =
            {
                { "CAKE_PATHS_TOOLS", scriptToolsPath.FullPath },
                { "CAKE_PATHS_ADDINS", scriptToolsPath.Combine("Addins").FullPath },
                { "CAKE_PATHS_MODULES", scriptToolsPath.Combine("Modules").FullPath }
            }
        });
}

Task("Cake.Common.Tools.ToolAliases.Setup")
    .Does(() =>
{
    var path = Paths.Temp.Combine("./Cake.Common/Tools/ToolAliases");
    CleanDirectory(path);
});

Task("Cake.Common.Tools.ToolAliases.InstallTool.NuGet")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.Setup")
    .Does(() =>
{
    // Given, When — install a NuGet tool inside a task, not via #tool
    RunInstallToolScript("InstallTool.NuGet",
        $$"""
        Task("Install")
            .Does(() =>
        {
            var paths = InstallTool("{{NuGetToolUri}}");
            if (paths == null || paths.Length == 0)
            {
                throw new Exception("InstallTool returned no paths.");
            }

            var tool = Context.Tools.Resolve("xunit.console") ?? Context.Tools.Resolve("xunit.console.exe");
            if (tool == null)
            {
                throw new Exception("Installed NuGet tool was not registered with the tool locator.");
            }
        });

        RunTarget("Install");
        """);
});

Task("Cake.Common.Tools.ToolAliases.InstallTool.PackageReference")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.Setup")
    .Does(() =>
{
    // Given, When
    RunInstallToolScript("InstallTool.PackageReference",
        $$"""
        Task("Install")
            .Does(() =>
        {
            var paths = InstallTool(new PackageReference("{{NuGetToolUri}}"));
            if (paths == null || paths.Length == 0)
            {
                throw new Exception("InstallTool(PackageReference) returned no paths.");
            }
        });

        RunTarget("Install");
        """);
});

Task("Cake.Common.Tools.ToolAliases.InstallTools.Strings")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.InstallTool.NuGet")
    .Does(() =>
{
    // Given, When — install two tools in one call
    RunInstallToolScript("InstallTools.Strings",
        $$"""
        Task("Install")
            .Does(() =>
        {
            var results = InstallTools(
                "{{NuGetToolUri}}",
                "{{DotNetToolUri}}");

            if (results == null || results.Length != 2)
            {
                throw new Exception("InstallTools should return one result per tool.");
            }

            var (firstTool, firstPaths) = results[0];
            var (secondTool, secondPaths) = results[1];
            if (firstPaths.Length == 0 || secondPaths.Length == 0)
            {
                throw new Exception("InstallTools returned a tool with no installed paths.");
            }
            if (firstTool.Package != "xunit.runner.console" || secondTool.Package != "GitVersion.Tool")
            {
                throw new Exception("InstallTools did not preserve package identities.");
            }
        });

        RunTarget("Install");
        """);
});

Task("Cake.Common.Tools.ToolAliases.InstallTool.DotNet")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.Setup")
    .Does(() =>
{
    // Given, When
    RunInstallToolScript("InstallTool.DotNet",
        $$"""
        Task("Install")
            .Does(() =>
        {
            var paths = InstallTool("{{DotNetToolUri}}");
            if (paths == null || paths.Length == 0)
            {
                throw new Exception("InstallTool(dotnet:) returned no paths.");
            }

            var tool = Context.Tools.Resolve("dotnet-gitversion")
                ?? Context.Tools.Resolve("dotnet-gitversion.exe")
                ?? Context.Tools.Resolve("gitversion")
                ?? Context.Tools.Resolve("gitversion.exe");
            if (tool == null)
            {
                throw new Exception("Installed dotnet tool was not registered with the tool locator.");
            }
        });

        RunTarget("Install");
        """);
});

Task("Cake.Common.Tools.ToolAliases")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.Setup")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.InstallTool.NuGet")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.InstallTool.PackageReference")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.InstallTools.Strings")
    .IsDependentOn("Cake.Common.Tools.ToolAliases.InstallTool.DotNet");
