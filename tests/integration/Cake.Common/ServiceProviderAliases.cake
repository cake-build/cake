#load "./../utilities/xunit.cake"
#load "./../utilities/paths.cake"

//////////////////////////////////////////////////////////////////////////////
// Integration tests for the ServiceProvider property alias.
// Packing and nested Cake.Tool runs happen inside task .Does() only.
//////////////////////////////////////////////////////////////////////////////

(FilePath Cake, string Version) PackMyServiceModule()
{
    CleanDirectories($"{Paths.Resources}/Cake.Core/Scripting/module/{{bin,obj}}");

    FilePath cake = Paths.CakeTool;

    var msBuildSettings = new DotNetMSBuildSettings
                                {
                                    Version = string.Format("{0}.{1}.{2}.{3}",
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    DateTime.Now.Day,
                                                    (DateTime.Now.Hour << 4) + DateTime.Now.Minute
                                                ),
                                }
                                 .WithProperty("CakeCorePath", Paths.CakeCore.FullPath)
                                 .SetTargetFramework(
                                     cake switch
                                     {
                                        FilePath net8_0Path         when net8_0Path.FullPath.Contains("net8.0")                 => "net8.0",
                                        FilePath net9_0Path         when net9_0Path.FullPath.Contains("net9.0")                 => "net9.0",
                                        _ => "net10.0"
                                     }
                                 );

    DotNetPack($"{Paths.Resources}/Cake.Core/Scripting/module/Cake.MyService.Module.csproj",
        new DotNetPackSettings {
            Configuration = "Release",
            MSBuildSettings = msBuildSettings
        });

    return (cake, msBuildSettings.Version);
}

void RunServiceProviderScript(string name, string script, FilePath cake)
{
    var workingDirectory = Paths.Temp.Combine($"./Cake.Common/ServiceProviderAliases/{name}");
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
                { "CAKE_PATHS_MODULES", scriptToolsPath.Combine("Modules").FullPath },
                { "NUGET_PACKAGES", $"{Paths.Temp}/nuget/Packages" },
                { "NUGET_HTTP_CACHE_PATH ", $"{Paths.Temp}/nuget/Cache" }
            },
            ToolPath = cake,
            Verbosity = Context.Log.Verbosity
        });
}

Task("Cake.Common.ServiceProviderAliases.Setup")
    .Does(() =>
{
    var path = Paths.Temp.Combine("./Cake.Common/ServiceProviderAliases");
    CleanDirectory(path);
});

Task("Cake.Common.ServiceProviderAliases.BuiltInServices")
    .IsDependentOn("Cake.Common.ServiceProviderAliases.Setup")
    .Does(() =>
{
    RunServiceProviderScript("BuiltInServices",
        """
        Task("Resolve")
            .Does(() =>
        {
            var log = ServiceProvider.GetRequiredService<ICakeLog>();
            log.Information("Hello from IoC");

            var context = ServiceProvider.GetRequiredService<ICakeContext>();
            if (!object.ReferenceEquals(context, Context))
            {
                throw new Exception("Resolved ICakeContext was not the script context.");
            }

            if (!object.ReferenceEquals(ServiceProvider, Context.ServiceProvider))
            {
                throw new Exception("ServiceProvider alias did not match Context.ServiceProvider.");
            }
        });

        RunTarget("Resolve");
        """,
        Paths.CakeTool);
});

Task("Cake.Common.ServiceProviderAliases.MyServiceModule")
    .IsDependentOn("Cake.Common.ServiceProviderAliases.Setup")
    .Does(() =>
{
    var (cake, version) = PackMyServiceModule();
    var feed = $"{Paths.Resources}/Cake.Core/Scripting/module/bin/Release";

    RunServiceProviderScript("MyServiceModule",
        $$"""
        {{"#"}}module nuget:{{feed}}?package=Cake.MyService.Module&version={{version}}

        Task("Resolve")
            .Does(() =>
        {
            var myService = ServiceProvider.GetRequiredService<IMyService>();
            var result = myService.DoSomething();
            if (result != "Hello from MyService")
            {
                throw new Exception($"Unexpected IMyService result: {result}");
            }
        });

        RunTarget("Resolve");
        """,
        cake);
});

Task("Cake.Common.ServiceProviderAliases")
    .IsDependentOn("Cake.Common.ServiceProviderAliases.Setup")
    .IsDependentOn("Cake.Common.ServiceProviderAliases.BuiltInServices")
    .IsDependentOn("Cake.Common.ServiceProviderAliases.MyServiceModule");
