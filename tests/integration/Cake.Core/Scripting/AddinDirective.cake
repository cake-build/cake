#addin nuget:?package=Cake.Kudu.Client&version=2.0.0
#load "./../../utilities/paths.cake"
#load "./../../utilities/xunit.cake"

(FilePath Cake, string Version) PackScriptingTestAddin()
{
    CleanDirectories($"{Paths.Resources}/Cake.Core/Scripting/addin/{{bin,obj}}");

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
                                        FilePath net10_0Path        when net10_0Path.FullPath.Contains("net10.0")               => "net10.0",
                                        _ => "net11.0"
                                     }
                                 );

    DotNetPack($"{Paths.Resources}/Cake.Core/Scripting/addin/addin.csproj",
        new DotNetPackSettings {
            Configuration = "Release",
            MSBuildSettings = msBuildSettings
        });

    return (cake, msBuildSettings.Version);
}

CakeSettings CreateNestedCakeSettings(FilePath cake, string addinsFolder, Action<ProcessSettings> setupProcessSettings = null)
{
    return new CakeSettings {
        EnvironmentVariables = new Dictionary<string, string>{
            {"CAKE_PATHS_ADDINS", $"{Paths.Temp}/{addinsFolder}/Addins"},
            {"CAKE_PATHS_TOOLS", $"{Paths.Temp}/{addinsFolder}"},
            {"CAKE_PATHS_MODULES", $"{Paths.Temp}/{addinsFolder}/Modules"},
            {"NUGET_PACKAGES", $"{Paths.Temp}/nuget/Packages"},
            {"NUGET_HTTP_CACHE_PATH ", $"{Paths.Temp}/nuget/Cache"}
        },
        SetupProcessSettings = setupProcessSettings,
        ToolPath = cake,
        Verbosity = Context.Log.Verbosity
    };
}

Task("Cake.Core.Scripting.AddinDirective.LoadTargetedAddin")
    .Does(() =>
{
    var (cake, version) = PackScriptingTestAddin();

    var script = 
        $$"""
        {{"#"}}addin nuget:{{Paths.Resources}}/Cake.Core/Scripting/addin/bin/Release?package=addin&version={{version}}
        Information("Magic number: {0}", GetMagicNumber(false));
        Information("The answer to life: {0}", TheAnswerToLife);
        Information("Get Dynamic Magic Number: {0}", GetDynamicMagicNumber(false).MagicNumber);
        Information("Dynamic Magic Number: {0}", TheDynamicAnswerToLife.TheAnswerToLife);
        """;

    CakeExecuteExpression(script, CreateNestedCakeSettings(cake, "tools"));
});

Task("Cake.Core.Scripting.AddinDirective.LoadNullableAddin")
    .Does(() =>
{
    var (cake, version) = PackScriptingTestAddin();
    var output = new List<string>();
    var error = new List<string>();

    var script = 
        $$"""
        {{"#"}}addin nuget:{{Paths.Resources}}/Cake.Core/Scripting/addin/bin/Release?package=addin&version={{version}}
        {{"#"}}nullable disable
        Information("Nullable label: {0}", GetNullableLabel(null));
        Information("Nullable answer: {0}", TheNullableAnswerToLife);
        Information("Nullable labels: {0}", CountNullableLabels(new[] { "a", (string)null }));
        Information("Not null value: {0}", GetNotNullValue("cake"));
        Information("Unconstrained value: {0}", GetUnconstrainedValue("cake"));
        Information("Unconstrained values: {0}", CountUnconstrainedValues(new[] { "a", "b" }));
        Information("Created value: {0}", CreateNotNullValue<System.Text.StringBuilder>().Append("cake"));
        """;

    var settings = CreateNestedCakeSettings(
        cake,
        "nullable-tools",
        processSettings =>
        {
            processSettings.RedirectStandardOutput = true;
            processSettings.RedirectStandardError = true;
            processSettings.RedirectedStandardOutputHandler = line =>
            {
                if (!string.IsNullOrEmpty(line))
                {
                    output.Add(line);
                }

                return line;
            };
            processSettings.RedirectedStandardErrorHandler = line =>
            {
                if (!string.IsNullOrEmpty(line))
                {
                    error.Add(line);
                }

                return line;
            };
        });

    // the integration tests run at quiet verbosity, which hides both the script output and the
    // compiler warnings asserted on below
    settings.Verbosity = Verbosity.Diagnostic;

    // CI build servers support ANSI, and the escape codes Cake then emits around each logged
    // argument would break up the messages asserted on below
    settings.EnvironmentVariables["NO_COLOR"] = "1";

    CakeExecuteExpression(script, settings);

    var diagnostics = output.Concat(error).ToArray();

    if (Context.Log.Verbosity == Verbosity.Diagnostic)
    {
        foreach (var line in diagnostics)
        {
            Verbose(line);
        }
    }

    Assert.Contains(diagnostics, line => line.Contains("Nullable label: none"));
    Assert.Contains(diagnostics, line => line.Contains("Nullable answer: 42"));
    Assert.Contains(diagnostics, line => line.Contains("Nullable labels: 1"));
    Assert.Contains(diagnostics, line => line.Contains("Not null value: cake"));
    Assert.Contains(diagnostics, line => line.Contains("Unconstrained value: cake"));
    Assert.Contains(diagnostics, line => line.Contains("Unconstrained values: 2"));
    Assert.Contains(diagnostics, line => line.Contains("Created value: cake"));

    // CS8632 nullable annotation outside a nullable context, CS8714 nullability of type argument
    // doesn't match the notnull constraint, CS8604 possible null reference argument,
    // CS8620 argument can't be used due to differences in nullability
    foreach (var id in new[] { "CS8632", "CS8714", "CS8604", "CS8620" })
    {
        Assert.DoesNotContain(diagnostics, line => line.Contains(id));
    }
});

Task("Cake.Core.Scripting.AddinDirective.CallDuplicatedMethod")
    .Does(context =>
{
    var result = context.EnvironmentVariable("CAKE_DOES_ROCK", true);
});

Task("Cake.Core.Scripting.AddinDirective.LoadNativeAssemblies")
    .WithCriteria(() => GitHubActions.Environment.Runner.Architecture != GitHubActionsArchitecture.ARM64)
    .Does(() =>
{
    FilePath cake = Paths.CakeTool;
    var script =
        $$"""
        {{"#"}}addin nuget:?package=Cake.Git&version=5.0.1

        var repoRoot = GitFindRootFromPath(Context.EnvironmentVariable("CAKE_TEST_DIR"));

        var hasUncommittedChanges = GitHasUncommitedChanges(repoRoot);
        """;

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
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.LoadNullableAddin")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.CallDuplicatedMethod")
    .IsDependentOn("Cake.Core.Scripting.AddinDirective.LoadNativeAssemblies");