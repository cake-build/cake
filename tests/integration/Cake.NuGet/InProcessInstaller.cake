Task("Cake.NuGet.InProcessInstaller.Setup")
    .Does(() =>
{
    var path = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller");
    CleanDirectory(path);
});

Task("Cake.NuGet.InProcessInstaller.VersionPinExact")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.Setup")
    .Does(()=>
{
    var scriptPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/").CombineWithFilePath("build.cake");
    var script = $@"#addin nuget:?package=Serilog&version=2.7.1

        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion;

        Information(""Requested NuGet version: 2.7.1"");
        Information(""Loaded assembly file version: "" + fileVersion);

        var parsedVersion = Version.Parse(fileVersion);

        if (parsedVersion != new Version(2, 7, 1, 0))
            throw new Exception(""VersionPinExact failed"");
    ";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    var scriptToolsPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/tools");
    CakeExecuteScript(scriptPath,
        new CakeSettings
        {
            EnvironmentVariables =
            {
                { "CAKE_PATHS_TOOLS", scriptToolsPath.FullPath },
                { "CAKE_PATHS_ADDINS", scriptToolsPath.Combine("Addins").FullPath },
                { "CAKE_PATHS_MODULES", scriptToolsPath.Combine("Modules").FullPath }
            }
        });
});

Task("Cake.NuGet.InProcessInstaller.VersionPinRange.InclusiveExclusive")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.Setup")
    .Does(()=>
{
    var scriptPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/").CombineWithFilePath("build.cake");
    var script = $@"#addin nuget:?package=Serilog&version=[2.5.0,2.6.0)

        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion;

        Information(""Requested NuGet version range: [2.5.0,2.6.0)"");
        Information(""Loaded assembly file version: "" + fileVersion);

        var parsedVersion = Version.Parse(fileVersion);

        if (parsedVersion < new Version(2, 5, 0, 0) || parsedVersion >= new Version(2, 6, 0, 0))
            throw new Exception(""VersionPinRange.InclusiveExclusive failed"");
    ";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    var scriptToolsPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/tools");
    CakeExecuteScript(scriptPath,
        new CakeSettings
        {
            EnvironmentVariables =
            {
                { "CAKE_PATHS_TOOLS", scriptToolsPath.FullPath },
                { "CAKE_PATHS_ADDINS", scriptToolsPath.Combine("Addins").FullPath },
                { "CAKE_PATHS_MODULES", scriptToolsPath.Combine("Modules").FullPath }
            }
        });
});

Task("Cake.NuGet.InProcessInstaller.VersionPinRange.InclusiveInclusive")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.Setup")
    .Does(()=>
{
    var scriptPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/").CombineWithFilePath("build.cake");
    var script = $@"#addin nuget:?package=Serilog&version=[2.3.0,2.4.0]

        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion;

        Information(""Requested NuGet version range: [2.3.0,2.4.0]"");
        Information(""Loaded assembly file version: "" + fileVersion);

        var parsedVersion = Version.Parse(fileVersion);

        if (parsedVersion < new Version(2, 3, 0, 0) || parsedVersion > new Version(2, 4, 0, 0))
            throw new Exception(""VersionPinRange.InclusiveInclusive failed"");
    ";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    var scriptToolsPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/tools");
    CakeExecuteScript(scriptPath,
        new CakeSettings
        {
            EnvironmentVariables =
            {
                { "CAKE_PATHS_TOOLS", scriptToolsPath.FullPath },
                { "CAKE_PATHS_ADDINS", scriptToolsPath.Combine("Addins").FullPath },
                { "CAKE_PATHS_MODULES", scriptToolsPath.Combine("Modules").FullPath }
            }
        });
});

Task("Cake.NuGet.InProcessInstaller.VersionPinRange.Wildcard")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.Setup")
    .Does(()=>
{
    var scriptPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/").CombineWithFilePath("build.cake");
    var script = $@"#addin nuget:?package=Serilog&version=[2.2.*,2.3.0)

        string fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Serilog.LoggerConfiguration).Assembly.Location).FileVersion;

        Information(""Requested NuGet version range: [2.2.*,2.3.0)"");
        Information(""Loaded assembly file version: "" + fileVersion);

        var parsedVersion = Version.Parse(fileVersion);

        if (parsedVersion < new Version(2, 2, 0, 0) || parsedVersion >= new Version(2, 3, 0, 0))
            throw new Exception(""VersionPinRange.Wildcard failed"");
    ";
    System.IO.File.WriteAllText(scriptPath.FullPath, script);

    var scriptToolsPath = Paths.Temp.Combine("./Cake.NuGet.InProcessInstaller/tools");
    CakeExecuteScript(scriptPath,
        new CakeSettings
        {
            EnvironmentVariables =
            {
                { "CAKE_PATHS_TOOLS", scriptToolsPath.FullPath },
                { "CAKE_PATHS_ADDINS", scriptToolsPath.Combine("Addins").FullPath },
                { "CAKE_PATHS_MODULES", scriptToolsPath.Combine("Modules").FullPath }
            }
        });
});

Task("Cake.NuGet.InProcessInstaller")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.Setup")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.VersionPinExact")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.VersionPinRange.InclusiveExclusive")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.VersionPinRange.InclusiveInclusive")
    .IsDependentOn("Cake.NuGet.InProcessInstaller.VersionPinRange.Wildcard");
