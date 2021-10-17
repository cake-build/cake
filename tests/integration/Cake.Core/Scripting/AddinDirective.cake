#addin nuget:?package=Cake.Kudu.Client&version=0.9.0

Task("Cake.Core.Scripting.AddinDirective.LoadTargetedAddin")
    .Does(() =>
{
    CleanDirectories($"{Paths.Resources}/Cake.Core/Scripting/addin/{{bin,obj}}");

    FilePath cakeCore = typeof(ICakeContext).GetTypeInfo().Assembly.Location;
    FilePath cake = cakeCore.GetDirectory().CombineWithFilePath("Cake.dll");

    var msBuildSettings = new DotNetCoreMSBuildSettings
                                {
                                    Version = "1.0.0",
                                }
                                 .WithProperty("CakeCorePath", typeof(ICakeContext).GetTypeInfo().Assembly.Location)
                                 .SetTargetFramework(
                                     cake switch
                                     {
                                        FilePath netCoreApp3_1Path  when netCoreApp3_1Path.FullPath.Contains("netcoreapp3.1")   => "netcoreapp3.1",
                                        FilePath net5_0Path         when net5_0Path.FullPath.Contains("net5.0")                 => "net5.0",
                                        _ => "net6.0"
                                     }
                                 );

    DotNetCorePack($"{Paths.Resources}/Cake.Core/Scripting/addin/addin.csproj",
        new DotNetCorePackSettings {
            Configuration = "Release",
            MSBuildSettings = msBuildSettings
        });

    var script = $@"#addin nuget:{Paths.Resources}/Cake.Core/Scripting/addin/bin/Release?package=addin&version=1.0.0
        Information(""Magic number: {0}"", GetMagicNumber(false));
        Information(""The answer to life: {0}"", TheAnswerToLife);
        Information(""Get Dynamic Magic Number: {0}"", GetDynamicMagicNumber(false).MagicNumber);
        Information(""Dynamic Magic Number: {0}"", TheDynamicAnswerToLife.TheAnswerToLife);
    ";

    CakeExecuteExpression(script,
        new CakeSettings {
            EnvironmentVariables = new Dictionary<string, string>{
                {"CAKE_PATHS_ADDINS", $"{Paths.Temp}/tools/Addins"},
                {"CAKE_PATHS_TOOLS", $"{Paths.Temp}/tools"},
                {"CAKE_PATHS_MODULES", $"{Paths.Temp}/tools/Modules"},
                {"NUGET_PACKAGES", $"{Paths.Temp}/nuget/Packages"},
                {"NUGET_HTTP_CACHE_PATH ", $"{Paths.Temp}/nuget/Cache"}
            },
            ToolPath = cake,
            Verbosity = Context.Log.Verbosity
        });
});

Task("Cake.Core.Scripting.AddinDirective.CallDuplicatedMethod")
    .Does(context =>
{
    var result = context.EnvironmentVariable("CAKE_DOES_ROCK", true);
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.AddinDirective")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.LoadTargetedAddin")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.CallDuplicatedMethod");