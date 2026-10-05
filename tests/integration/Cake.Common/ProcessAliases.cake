#load "./../utilities/xunit.cake"
#load "./../utilities/paths.cake"
using System.Diagnostics;

Task("Cake.Common.ProcessAliases.StartProcess")
    .Does(() =>
{
    // Given
    var fileName = Context.Tools.Resolve("dotnet.exe")
                    ?? Context.Tools.Resolve("dotnet");
    var argument = "--version";

    // When
    var result = StartProcess(fileName, argument);

    // Then
    Assert.Equal(0, result);
});

Task("Cake.Common.ProcessAliases.StartProcess.Output")
    .Does(() =>
{
    // Given
    var fileName = Context.Tools.Resolve("dotnet.exe")
                    ?? Context.Tools.Resolve("dotnet");
    var argument = $"{Paths.CakeTool.FullPath} --version";
    var version = FileVersionInfo.GetVersionInfo(Paths.CakeCore.FullPath).Comments;

    // When
    IEnumerable<string> redirectedStandardOutput;
    var result = StartProcess(
	                 fileName,
	                 new ProcessSettings {
	                     Arguments = argument,
	                     RedirectStandardOutput = true
	                 },
	                 out redirectedStandardOutput);

    // Then
    Assert.Equal(0, result);
    Assert.Equal(version, string.Concat(redirectedStandardOutput));
});

#if NET11_0_OR_GREATER
Task("Cake.Common.ProcessAliases.StartProcessAndForget")
    .Does(() =>
{
    // Given
    var fileName = Context.Tools.Resolve("dotnet.exe")
                    ?? Context.Tools.Resolve("dotnet");
    var argument = "--version";

    // When
    var processId = StartProcessAndForget(fileName, argument);

    // Then
    Assert.True(processId > 0);
});

Task("Cake.Common.ProcessAliases.StartProcess.Net11Settings")
    .Does(() =>
{
    // Given
    var fileName = Context.Tools.Resolve("dotnet.exe")
                    ?? Context.Tools.Resolve("dotnet");
    var argument = "--version";

    // When
    var result = StartProcess(
        fileName,
        new ProcessSettings {
            Arguments = argument,
            RestrictInheritedHandles = true,
            StartDetached = true,
            DiscardStandardOutput = true,
            DiscardStandardError = true
        });

    // Then
    Assert.Equal(0, result);

    if (Context.Environment.Platform.Family == PlatformFamily.Windows ||
        Context.Environment.Platform.Family == PlatformFamily.Linux)
    {
        var killOnParentExitResult = StartProcess(
            fileName,
            new ProcessSettings {
                Arguments = argument,
                KillOnParentExit = true
            });
        Assert.Equal(0, killOnParentExitResult);
    }
});
#endif

Task("Cake.Common.ProcessAliases")
    .IsDependentOn("Cake.Common.ProcessAliases.StartProcess")
    .IsDependentOn("Cake.Common.ProcessAliases.StartProcess.Output")
#if NET11_0_OR_GREATER
    .IsDependentOn("Cake.Common.ProcessAliases.StartProcessAndForget")
    .IsDependentOn("Cake.Common.ProcessAliases.StartProcess.Net11Settings")
#endif
    ;