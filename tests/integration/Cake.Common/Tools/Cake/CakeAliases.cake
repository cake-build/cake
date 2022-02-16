#load "./../../../utilities/xunit.cake"
#load "./../../../utilities/paths.cake"

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/Cake");
    var file = path.CombineWithFilePath("./build.cake");

    // When
     CakeExecuteScript(file);
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.Ok")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/Cake");
    var file = path.CombineWithFilePath("./test.cake");

    // When
    CakeExecuteScript(file, new CakeSettings{ Arguments = new Dictionary<string, string> {{ "ok", "yes" }}});
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.NotOk")
    .Does(() =>
{
    // Given
    var path = Paths.Resources.Combine("./Cake.Common/Tools/Cake");
    var file = path.CombineWithFilePath("./test.cake");
    var expect = "Process returned an error (exit code 1).";

    // When
    var exception = Record.Exception(
        () => CakeExecuteScript(file, new CakeSettings{ Arguments = new Dictionary<string, string> {{ "ok", "no" }}})
    );

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.EndsWith(expect, exception.Message);
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression")
    .Does(() =>
{
    // Given
    var script = "System.Environment.Exit(0);";
    // When
     CakeExecuteExpression(script);
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.Ok")
    .Does(() =>
{
    // Given
    var script = "System.Environment.Exit((Argument<string>(\"ok\", \"no\")==\"yes\") ? 0 : 1);";

    // When
    CakeExecuteExpression(script, new CakeSettings{ Arguments = new Dictionary<string, string> {{ "ok", "yes" }}});
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.NotOk")
    .Does(() =>
{
    // Given
    var script = "System.Environment.Exit((Argument<string>(\"ok\", \"no\")==\"yes\") ? 0 : 1);";
    var expect = "Process returned an error (exit code 1).";

    // When
    var exception = Record.Exception(
        () => CakeExecuteExpression(script, new CakeSettings{ Arguments = new Dictionary<string, string> {{ "ok", "no" }}})
    );

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.EndsWith(expect, exception.Message);
});

Task("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.CakeException.CustomExitCode")
    .Does(() =>
{
    // Given
    var script = "throw new CakeException(42);";
    var expect = 42;

    // When
     var exception = Record.Exception(
        () => CakeExecuteExpression(script)
    );

    // Then
    Assert.NotNull(exception);
    Assert.IsType<CakeException>(exception);
    Assert.Equal(expect, (exception as CakeException)?.ExitCode);
});

Task("Cake.Common.Tools.Cake.CakeAliases")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.Ok")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteScript.Settings.NotOk")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.Ok")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.Settings.NotOk")
    .IsDependentOn("Cake.Common.Tools.Cake.CakeAliases.CakeExecuteExpression.CakeException.CustomExitCode");
