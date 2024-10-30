#addin nuget:?package=Cake.Kudu.Client&version=2.0.0
#load "./../../utilities/paths.cake"


Task("Cake.Core.Scripting.AddinDirective.LoadTargetedAddin")
    .Does(() =>
{
    CleanDirectories($"{Paths.Resources}/Cake.Core/Scripting/addin/{{bin,obj}}");

    FilePath cakeCore = typeof(ICakeContext).GetTypeInfo().Assembly.Location;
    FilePath cake = cakeCore.GetDirectory().CombineWithFilePath("Cake.dll");

    var msBuildSettings = new DotNetMSBuildSettings
                                {
                                    Version = string.Format("{0}.{1}.{2}.{3}",
                                                    DateTime.Now.Year,
                                                    DateTime.Now.Month,
                                                    DateTime.Now.Day,
                                                    (DateTime.Now.Hour << 4) + DateTime.Now.Minute
                                                ),
                                }
                                 .WithProperty("CakeCorePath", typeof(ICakeContext).GetTypeInfo().Assembly.Location)
                                 .SetTargetFramework(
                                     cake switch
                                     {
                                        FilePath net8_0Path         when net8_0Path.FullPath.Contains("net8.0")                 => "net8.0",
                                        _ => "net9.0"
                                     }
                                 );

    DotNetPack($"{Paths.Resources}/Cake.Core/Scripting/addin/addin.csproj",
        new DotNetPackSettings {
            Configuration = "Release",
            MSBuildSettings = msBuildSettings
        });

    var script = $@"#addin nuget:{Paths.Resources}/Cake.Core/Scripting/addin/bin/Release?package=addin&version={msBuildSettings.Version}
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

Task("Cake.Core.Scripting.AddinDirective.LoadNativeAssemblies")
    .Does(() =>
{
    FilePath cakeCore = typeof(ICakeContext).GetTypeInfo().Assembly.Location;
    FilePath cake = cakeCore.GetDirectory().CombineWithFilePath("Cake.dll");
    var script = @"#addin nuget:?package=Cake.Git&version=4.0.0

var repoRoot = GitFindRootFromPath(Context.EnvironmentVariable(""CAKE_TEST_DIR""));

var hasUncommittedChanges = GitHasUncommitedChanges(repoRoot);";

    CakeExecuteExpression(script,
        new CakeSettings {
            EnvironmentVariables = new Dictionary<string, string>{
                {"CAKE_PATHS_ADDINS", $"{Paths.Temp}/native/tools/Addins"},
                {"CAKE_PATHS_TOOLS", $"{Paths.Temp}/native/tools"},
                {"CAKE_PATHS_MODULES", $"{Paths.Temp}/native/tools/Modules"},
                {"NUGET_PACKAGES", $"{Paths.Temp}/nuget/Packages"},
                {"NUGET_HTTP_CACHE_PATH ", $"{Paths.Temp}/nuget/Cache"},
                {"CAKE_TEST_DIR", Context.Environment.WorkingDirectory.FullPath}
            },
            ToolPath = cake,
            Verbosity = Context.Log.Verbosity
        });
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Core.Scripting.AddinDirective")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.LoadTargetedAddin")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.CallDuplicatedMethod")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.LoadNativeAssemblies");