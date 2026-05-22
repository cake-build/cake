#load "./../utilities/paths.cake"

Task("Cake.DotNetTool.Module.Setup")
    .Does(() =>
{
    var path = Paths.Temp.Combine("./Cake.DotNetTool.Module");
    CleanDirectory(path);
});

Task("Cake.DotNetTool.Module.Install")
    .IsDependentOn("Cake.DotNetTool.Module.Setup")
    .Does(() =>
{
    // Given
    var scriptPath = Paths.Temp.Combine("./Cake.DotNetTool.Module/").CombineWithFilePath("build.cake");
    var script = "#tool \"dotnet:?package=Octopus.DotNet.Cli&version=7.4.6\"";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    // When
    CakeExecuteScript(scriptPath);
});

Task("Cake.DotNetTool.Module.Update")
    .IsDependentOn("Cake.DotNetTool.Module.Install")
    .Does(() =>
{
    // Given
    var scriptPath = Paths.Temp.Combine("./Cake.DotNetTool.Module/").CombineWithFilePath("build.cake");
    var script = "#tool \"dotnet:?package=Octopus.DotNet.Cli&version=7.4.3121\"";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    // When
    CakeExecuteScript(scriptPath);
});

Task("Cake.DotNetTool.Module.Source.Setup")
    .Does(() =>
{
    var path = Paths.Temp.Combine("./Cake.DotNetTool.Module.Source");
    CleanDirectory(path);
});

Task("Cake.DotNetTool.Module.Source.Install")
    .IsDependentOn("Cake.DotNetTool.Module.Source.Setup")
    .Does(() =>
{
    // Given — explicit NuGet source (see https://github.com/cake-build/cake/issues/4834)
    var scriptPath = Paths.Temp.Combine("./Cake.DotNetTool.Module.Source/").CombineWithFilePath("build.cake");
    var script = "#tool \"dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=6.6.2\"";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    // When
    CakeExecuteScript(scriptPath);
});

Task("Cake.DotNetTool.Module.Source.Update")
    .IsDependentOn("Cake.DotNetTool.Module.Source.Install")
    .Does(() =>
{
    // Given — same source, newer tool version (see https://cakebuild.net/docs/writing-builds/tools/installing-tools)
    var scriptPath = Paths.Temp.Combine("./Cake.DotNetTool.Module.Source/").CombineWithFilePath("build.cake");
    var script = "#tool \"dotnet:https://api.nuget.org/v3/index.json?package=GitVersion.Tool&version=6.7.0\"";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    // When
    CakeExecuteScript(scriptPath);
});

Task("Cake.DotNetTool.Module.Source")
    .IsDependentOn("Cake.DotNetTool.Module.Source.Setup")
    .IsDependentOn("Cake.DotNetTool.Module.Source.Install")
    .IsDependentOn("Cake.DotNetTool.Module.Source.Update");

Task("Cake.DotNetTool.Module")
    .IsDependentOn("Cake.DotNetTool.Module.Setup")
    .IsDependentOn("Cake.DotNetTool.Module.Install")
    .IsDependentOn("Cake.DotNetTool.Module.Update")
    .IsDependentOn("Cake.DotNetTool.Module.Source");
