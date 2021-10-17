#load "./../utilities/xunit.cake"
#load "./../utilities/paths.cake"
using System.Diagnostics;
using System.Reflection;

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
    var coreLocation = typeof(ICakeContext).GetTypeInfo().Assembly.Location;
    var argument = $"{coreLocation.Replace(".Core","")} --version";
    var version = FileVersionInfo.GetVersionInfo(coreLocation).Comments;

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

Task("Cake.Common.ProcessAliases")
    .IsDependentOn("Cake.Common.ProcessAliases.StartProcess")
    .IsDependentOn("Cake.Common.ProcessAliases.StartProcess.Output");