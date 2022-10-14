#load "./../utilities/xunit.cake"

//////////////////////////////////////////////////////////////////////////////

Setup(
    context => new CommandSettings {
        ToolName = "dotnet",
        ToolExecutableNames = new []{ "dotnet", "dotnet.exe" },
    }
);

Task("Cake.Common.Tools.Command.CommandAliases.Command")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given, When, Then
    ctx.Command(settings.ToolExecutableNames, "--version");
});

Task("Cake.Common.Tools.Command.CommandAliases.Command.Settings")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given, When, Then
    ctx.Command(settings, "--version");
});

Task("Cake.Common.Tools.Command.CommandAliases.CommandStandardOutput")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given
    const string expectStandardOutput = @"Description:
  List tools installed globally or locally.

Usage:
  dotnet tool list [options]

Options:";

    // When
    var exitCode = ctx.Command(settings.ToolExecutableNames, out var standardOutput, "tool list -h");

    // Then
    Assert.Equal(0, exitCode);
    Assert.StartsWith(
      expectStandardOutput.NormalizeLineEndings(),
      standardOutput.NormalizeLineEndings());
});

Task("Cake.Common.Tools.Command.CommandAliases.CommandStandardOutput.Settings")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given
    const string expectStandardOutput = @"Description:
  List tools installed globally or locally.

Usage:
  dotnet tool list [options]

Options:";

    // When
    var exitCode = ctx.Command(settings, out var standardOutput, "tool list -h");

    // Then
    Assert.Equal(0, exitCode);
    Assert.StartsWith(
      expectStandardOutput.NormalizeLineEndings(),
      standardOutput.NormalizeLineEndings());
});

Task("Cake.Common.Tools.Command.CommandAliases.CommandStandardOutput.SettingsCustomization")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given
    const string expectStandardOutput = @"Description:
  List tools installed globally or locally.

Usage:
  dotnet tool list [options]

Options:";

    // When
    var exitCode = ctx.Command(
                    settings.ToolExecutableNames,
                    out var standardOutput,
                    settingsCustomization: settings => settings.WithArgumentCustomization(args => "tool list -h")
                    );

    // Then
    Assert.Equal(0, exitCode);
    Assert.StartsWith(
      expectStandardOutput.NormalizeLineEndings(),
      standardOutput.NormalizeLineEndings());
});

Task("Cake.Common.Tools.Command.CommandAliases.CommandStandardError")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given
    const string expectStandardOutput = @"Description:
  Install or work with tools that extend the .NET experience.

Usage:
  dotnet tool [command] [options]

Options:
  -?, -h, --help  Show command line help.

Commands:";
    const string expectStandardError = "Required command was not provided.";
    const int expectExitCode = 1;

    // When
    var result = ctx.Command(
                  settings.ToolExecutableNames,
                  out var standardOutput,
                  out var standardError,
                  "tool",
                  expectExitCode);

    // Then
    Assert.Equal(expectExitCode, result);
    Assert.StartsWith(
      expectStandardOutput.NormalizeLineEndings(),
      standardOutput.NormalizeLineEndings());
    Assert.Equal(
      expectStandardError.NormalizeLineEndings(),
      standardError.NormalizeLineEndings());
});

Task("Cake.Common.Tools.Command.CommandAliases.CommandStandardError.Settings")
    .Does<CommandSettings>(static (ctx, settings) =>
{
    // Given
    const string expectStandardOutput = @"Description:
  Install or work with tools that extend the .NET experience.

Usage:
  dotnet tool [command] [options]

Options:
  -?, -h, --help  Show command line help.

Commands:";
    const string expectStandardError = "Required command was not provided.";
    const int expectExitCode = 1;
    var errorSettings = new CommandSettings {
                            ToolName = settings.ToolName,
                            ToolExecutableNames = settings.ToolExecutableNames,
                        }.WithExpectedExitCode(expectExitCode);

    // When
    var result = ctx.Command(errorSettings, out var standardOutput, out var standardError, "tool");

    // Then
    Assert.Equal(expectExitCode, result);
    Assert.StartsWith(
      expectStandardOutput.NormalizeLineEndings(),
      standardOutput.NormalizeLineEndings());
    Assert.Equal(
      expectStandardError.NormalizeLineEndings(),
      standardError.NormalizeLineEndings());
});

//////////////////////////////////////////////////////////////////////////////

Task("Cake.Common.Tools.Command.CommandAliases")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.Command")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.Command.Settings")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.CommandStandardOutput")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.CommandStandardOutput.Settings")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.CommandStandardOutput.SettingsCustomization")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.CommandStandardError")
  .IsDependentOn("Cake.Common.Tools.Command.CommandAliases.CommandStandardError.Settings");