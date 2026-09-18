#load "./../../utilities/paths.cake"
#load "./../../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////
// Public-contract test: a community ICakeModule loads through #module on MS.DI.
//////////////////////////////////////////////////////////////////////////////

CakeSettings CreateModuleDirectiveCakeSettings(FilePath cake, string modulesFolder)
{
    return new CakeSettings {
        EnvironmentVariables = new Dictionary<string, string>{
            {"CAKE_PATHS_ADDINS", $"{Paths.Temp}/{modulesFolder}/Addins"},
            {"CAKE_PATHS_TOOLS", $"{Paths.Temp}/{modulesFolder}"},
            {"CAKE_PATHS_MODULES", $"{Paths.Temp}/{modulesFolder}/Modules"},
            {"NUGET_PACKAGES", $"{Paths.Temp}/nuget/Packages"},
            {"NUGET_HTTP_CACHE_PATH ", $"{Paths.Temp}/nuget/Cache"}
        },
        ToolPath = cake,
        Verbosity = Context.Log.Verbosity
    };
}

void RunModuleDirectiveScript(string name, string script)
{
    var workingDirectory = Paths.Temp.Combine($"./Cake.Core/Scripting/ModuleDirective/{name}");
    EnsureDirectoryExist(workingDirectory);
    var scriptPath = workingDirectory.CombineWithFilePath("build.cake");
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    CakeExecuteScript(scriptPath, CreateModuleDirectiveCakeSettings(Paths.CakeTool, $"ModuleDirective/{name}"));
}

Task("Cake.Core.Scripting.ModuleDirective.Setup")
    .Does(() =>
{
    var path = Paths.Temp.Combine("./Cake.Core/Scripting/ModuleDirective");
    CleanDirectory(path);
});

Task("Cake.Core.Scripting.ModuleDirective.BuildSystemsModule")
    .IsDependentOn("Cake.Core.Scripting.ModuleDirective.Setup")
    .Does(() =>
{
    RunModuleDirectiveScript("BuildSystemsModule",
        $$"""
        {{"#"}}module nuget:?package=Cake.BuildSystems.Module&version=9.0.0

        Task("Default")
            .Does(() =>
        {
            Information("Cake.BuildSystems.Module loaded.");
        });

        RunTarget("Default");
        """);
});

Task("Cake.Core.Scripting.ModuleDirective")
    .IsDependentOn("Cake.Core.Scripting.ModuleDirective.Setup")
    .IsDependentOn("Cake.Core.Scripting.ModuleDirective.BuildSystemsModule");
