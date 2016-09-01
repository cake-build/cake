#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/Cake");
    var file = path.CombineWithFilePath("./build.cake");

    // When
     CakeExecuteScript(file, GetCakeSettings(Context));
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.Ok")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/Cake");
    var file = path.CombineWithFilePath("./test.cake");

    // When
    CakeExecuteScript(file, GetCakeSettings(Context, new Dictionary<string, string> {{ "ok", "yes" }}));
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.NotOk")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/Cake");
    var file = path.CombineWithFilePath("./test.cake");

    // When
    var exception = Record.Exception(
        () => CakeExecuteScript(file, GetCakeSettings(Context, new Dictionary<string, string> {{ "ok", "no" }}))
    );

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.Equal(exception.Message, "Cake: Process returned an error (exit code 1).");
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression")
    .Does(() =>
{
    // Given
    var script = "System.Environment.Exit(0);";
    // When
     CakeExecuteExpression(script, GetCakeSettings(Context));
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.Ok")
    .Does(() =>
{
    // Given
    var script = "System.Environment.Exit((Argument<string>(\"ok\", \"no\")==\"yes\") ? 0 : 1);";

    // When
    CakeExecuteExpression(script, GetCakeSettings(Context, new Dictionary<string, string> {{ "ok", "yes" }}));
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.NotOk")
    .Does(() =>
{
    // Given
    var script = "System.Environment.Exit((Argument<string>(\"ok\", \"no\")==\"yes\") ? 0 : 1);";

    // When
    var exception = Record.Exception(
        () => CakeExecuteExpression(script, GetCakeSettings(Context, new Dictionary<string, string> {{ "ok", "no" }}))
    );

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.Equal(exception.Message, "Cake: Process returned an error (exit code 1).");
});

Task("Cake.Common.Tools.Cake.CakeAliases")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.Ok")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.NotOk")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.Ok")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.NotOk");

public static FilePath FindToolInPath(ICakeContext context, string tool)
{
    var pathEnv = context.EnvironmentVariable("PATH");
    if (string.IsNullOrEmpty(pathEnv)||string.IsNullOrEmpty(tool))
    {
        return tool;
    }
    var paths = pathEnv.Split(new []{context.IsRunningOnUnix() ? ':' : ';'},  StringSplitOptions.RemoveEmptyEntries);
    return paths.Select(
            path=>new DirectoryPath(path).CombineWithFilePath(tool)
        ).FirstOrDefault(filePath=>System.IO.File.Exists(filePath.FullPath));
}

public static CakeSettings GetCakeSettings(ICakeContext context, IDictionary<string, string> arguments = null)
{
    var settings = new CakeSettings { Arguments = arguments };
    if (context.Environment.Runtime.IsCoreClr)
    {
        settings.ToolPath = FindToolInPath(context, context.IsRunningOnUnix() ? "dotnet" : "dotnet.exe");
        settings.ArgumentCustomization = args => "./tools/Cake.CoreCLR/Cake.dll " + args.Render();

    }
    else if (context.IsRunningOnUnix())
    {
        settings.ToolPath = FindToolInPath(context, "mono");
        settings.ArgumentCustomization = args => "./tools/Cake/Cake.exe " + args.Render();
    }
    return settings;
}
