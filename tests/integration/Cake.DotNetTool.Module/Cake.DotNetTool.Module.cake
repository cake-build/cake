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

Task("Cake.DotNetTool.Module")
    .IsDependentOn("Cake.DotNetTool.Module.Setup")
    .IsDependentOn("Cake.DotNetTool.Module.Install")
    .IsDependentOn("Cake.DotNetTool.Module.Update");
