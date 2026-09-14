Task("Cake.Core.Diagnostics.LogEscapedBraces")
    .Does(() => {
    Context.Log.Debug(
        Verbosity.Normal,
        string.Format("Executing: {0}", "if ($LASTEXITCODE -gt 0) {{ throw \"script failed with exit code $LASTEXITCODE\" }}")
        );
});

Task("Cake.Core.Diagnostics.Verbosity.FromEnvironment")
    .Does(() =>
{
    var script = Paths.Resources.CombineWithFilePath("./Cake.Core/Diagnostics/assert-verbosity.cake");
    CakeExecuteScript(script, new CakeSettings
    {
        ToolPath = Paths.CakeTool,
        EnvironmentVariables =
        {
            { "CAKE_SETTINGS_VERBOSITY", "Minimal" }
        },
        Arguments =
        {
            { "expected", "Minimal" }
        }
    });
});

Task("Cake.Core.Diagnostics.Verbosity.CommandLineOverridesEnvironment")
    .Does(() =>
{
    var script = Paths.Resources.CombineWithFilePath("./Cake.Core/Diagnostics/assert-verbosity.cake");
    CakeExecuteScript(script, new CakeSettings
    {
        ToolPath = Paths.CakeTool,
        Verbosity = Verbosity.Quiet,
        EnvironmentVariables =
        {
            { "CAKE_SETTINGS_VERBOSITY", "Diagnostic" }
        },
        Arguments =
        {
            { "expected", "Quiet" }
        }
    });
});

Task("Cake.Core.Diagnostics.Verbosity.ExplicitNormalOverridesEnvironment")
    .Does(() =>
{
    var script = Paths.Resources.CombineWithFilePath("./Cake.Core/Diagnostics/assert-verbosity.cake");
    CakeExecuteScript(script, new CakeSettings
    {
        ToolPath = Paths.CakeTool,
        Verbosity = Verbosity.Normal,
        EnvironmentVariables =
        {
            { "CAKE_SETTINGS_VERBOSITY", "Diagnostic" }
        },
        Arguments =
        {
            { "expected", "Normal" }
        }
    });
});

Task("Cake.Core.Diagnostics")
    .IsDependentOn("Cake.Core.Diagnostics.LogEscapedBraces")
    .IsDependentOn("Cake.Core.Diagnostics.Verbosity.FromEnvironment")
    .IsDependentOn("Cake.Core.Diagnostics.Verbosity.CommandLineOverridesEnvironment")
    .IsDependentOn("Cake.Core.Diagnostics.Verbosity.ExplicitNormalOverridesEnvironment");
