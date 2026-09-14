// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tests.Fixtures.Tools.DotNet;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Tool
{
    public sealed class DotNetToolCommandTests
    {
        public sealed class TheToolCommandAliases
        {
            public static IEnumerable<object[]> DotNetToolExecuteOverloadCases()
            {
                yield return new object[] { nameof(DotNetToolCommandInvocation.ExecutePackageOnly), "\"tool\" exec dotnetsay@1.2.3" };
                yield return new object[] { nameof(DotNetToolCommandInvocation.ExecutePackageWithArguments), "\"tool\" exec dotnetsay@1.2.3 -- --tool-option tool-value" };
                yield return new object[] { nameof(DotNetToolCommandInvocation.ExecutePackageWithSettings), "\"tool\" exec dotnetsay@1.2.3 --verbosity minimal" };
                yield return new object[] { nameof(DotNetToolCommandInvocation.ExecutePackageWithArgumentsAndSettings), "\"tool\" exec dotnetsay@1.2.3 --verbosity minimal -- --tool-option tool-value" };
            }

            public static IEnumerable<object[]> DotNetToolCommandCases()
            {
                yield return new object[] { nameof(DotNetToolCommandInvocation.ExecutePackageWithArgumentsAndSettings), "dotnetsay@1.2.3", "\"tool\" exec dotnetsay@1.2.3 --verbosity minimal -- --tool-option tool-value", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.InstallFull), "dotnetsay", "\"tool\" install dotnetsay --global --verbosity minimal", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.ListFull), "dotnetsay", "\"tool\" list dotnetsay --global", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.ListSettings), null, "\"tool\" list --global", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.ListPackageSettings), "dotnetsay", "\"tool\" list dotnetsay --global", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.RestoreFull), null, "\"tool\" restore --disable-parallel --verbosity minimal", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.RestoreSettings), null, "\"tool\" restore --disable-parallel --verbosity minimal", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.RunFull), "dotnetsay", "\"tool\" run dotnetsay -- Hello --name World", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.RunCommandArguments), "dotnetsay", "\"tool\" run dotnetsay -- Hello --name World", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.RunCommandArgumentsAndSettings), "dotnetsay", "\"tool\" run dotnetsay -- Hello --name World", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.RunCommandSettings), "dotnetsay", "\"tool\" run dotnetsay", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.SearchFull), "cake", "\"tool\" search cake --detail", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.UninstallFull), "dotnetsay", "\"tool\" uninstall dotnetsay --global", false };
                yield return new object[] { nameof(DotNetToolCommandInvocation.UpdateFull), "dotnetsay", "\"tool\" update dotnetsay --global --verbosity minimal", false };
            }

            public static IEnumerable<object[]> RequiredCommandArgumentCases()
            {
                var commandArgumentCases = new[]
                {
                    new { InvocationName = nameof(DotNetToolCommandInvocation.ExecutePackageWithSettings), ParameterName = "packageId" },
                    new { InvocationName = nameof(DotNetToolCommandInvocation.InstallFull), ParameterName = "packageId" },
                    new { InvocationName = nameof(DotNetToolCommandInvocation.RunFull), ParameterName = "commandName" },
                    new { InvocationName = nameof(DotNetToolCommandInvocation.SearchFull), ParameterName = "searchTerm" },
                    new { InvocationName = nameof(DotNetToolCommandInvocation.UninstallFull), ParameterName = "packageId" }
                };

                var invalidArguments = new[] { null, string.Empty, " " };
                foreach (var commandArgumentCase in commandArgumentCases)
                {
                    foreach (var invalidArgument in invalidArguments)
                    {
                        yield return new object[] { commandArgumentCase.InvocationName, commandArgumentCase.ParameterName, invalidArgument };
                    }
                }
            }

            [Fact]
            public void DotNetToolInstall_WithDefaultSettings_RendersCakeToolsPath()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.InstallFull,
                    CommandArgument = "dotnetsay"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"tool\" install dotnetsay --tool-path \"/Working/tools\"", result.Args);
            }

            [Fact]
            public void DotNetToolInstall_WithToolPathScopeAndWorkingDirectory_RendersToolsPathRelativeToWorkingDirectory()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.InstallFull,
                    CommandArgument = "dotnetsay"
                };
                fixture.InstallSettings.WorkingDirectory = "./temp";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"tool\" install dotnetsay --tool-path \"/Working/temp/tools\"", result.Args);
            }

            [Theory]
            [InlineData(DotNetToolInstallationScope.Global, "--global")]
            [InlineData(DotNetToolInstallationScope.Local, "--local")]
            [InlineData(DotNetToolInstallationScope.ToolPath, "--tool-path \"/Working/tools\"")]
            public void DotNetToolInstall_WithInstallationScope_RendersSingleScopeFlag(DotNetToolInstallationScope scope, string expectedScopeArgument)
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.InstallFull,
                    CommandArgument = "dotnetsay"
                };
                fixture.InstallSettings.InstallationScope = scope;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal($"\"tool\" install dotnetsay {expectedScopeArgument}", result.Args);
            }

            [Fact]
            public void DotNetToolExecute_WithDefaultSettings_RendersExpectedCommand()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ExecutePackageWithSettings,
                    CommandArgument = "dotnetsay"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"tool\" exec dotnetsay", result.Args);
            }

            [Theory]
            [MemberData(nameof(RequiredCommandArgumentCases))]
            public void DotNetToolCommand_WithInvalidRequiredArgument_ThrowsArgumentNullException(string invocationName, string parameterName, string commandArgument)
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = Enum.Parse<DotNetToolCommandInvocation>(invocationName),
                    CommandArgument = commandArgument
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, parameterName);
            }

            [Fact]
            public void DotNetToolExecute_WithNullSettings_UsesDefaultSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ExecutePackageWithSettings,
                    CommandArgument = "dotnetsay",
                    ExecuteSettings = null
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"tool\" exec dotnetsay", result.Args);
            }

            [Fact]
            public void DotNetToolExecute_WhenProcessCannotStart_ThrowsCakeException()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ExecutePackageWithSettings,
                    CommandArgument = "dotnetsay"
                };
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
            }

            [Fact]
            public void DotNetToolExecute_WhenProcessHasNonZeroExitCode_ThrowsCakeException()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ExecutePackageWithSettings,
                    CommandArgument = "dotnetsay"
                };
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Theory]
            [MemberData(nameof(DotNetToolExecuteOverloadCases))]
            public void DotNetToolExecute_AllOverloads_RenderDotNetToolExecCommand(string invocationName, string expectedArguments)
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = Enum.Parse<DotNetToolCommandInvocation>(invocationName),
                    CommandArgument = "dotnetsay@1.2.3"
                };
                fixture.ConfigureCommonSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expectedArguments, result.Args);
            }

            [Theory]
            [MemberData(nameof(DotNetToolCommandCases))]
            public void DotNetToolCommands_RenderExpectedArguments(string invocationName, string commandArgument, string expectedArguments, bool usesProjectPath)
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = Enum.Parse<DotNetToolCommandInvocation>(invocationName),
                    ProjectPath = "./src/project.csproj",
                    CommandArgument = commandArgument
                };
                fixture.ConfigureCommonSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expectedArguments, result.Args);
                if (usesProjectPath)
                {
                    Assert.Equal(fixture.ProjectPath.GetDirectory().FullPath, result.Process.WorkingDirectory.FullPath);
                }
            }

            [Fact]
            public void DotNetToolCommands_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.InstallFull,
                    CommandArgument = "dotnetsay"
                };
                fixture.InstallSettings.InstallationScope = DotNetToolInstallationScope.ToolPath;
                fixture.InstallSettings.ToolInstallationPath = "./tools";
                fixture.InstallSettings.Version = "1.2.3";
                fixture.InstallSettings.ConfigFile = "./nuget.config";
                fixture.InstallSettings.ToolManifest = "./.config/dotnet-tools.json";
                fixture.InstallSettings.AddSource.Add("https://example.com/add-source");
                fixture.InstallSettings.Source.Add("https://example.com/source");
                fixture.InstallSettings.Framework = "net10.0";
                fixture.InstallSettings.Prerelease = true;
                fixture.InstallSettings.DisableParallel = true;
                fixture.InstallSettings.IgnoreFailedSources = true;
                fixture.InstallSettings.NoHttpCache = true;
                fixture.InstallSettings.Interactive = true;
                fixture.InstallSettings.AllowDowngrade = true;
                fixture.InstallSettings.Architecture = "x64";
                fixture.InstallSettings.CreateManifestIfNeeded = true;
                fixture.InstallSettings.AllowRollForward = true;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" install dotnetsay --tool-path \"/Working/tools\" --version \"1.2.3\" --configfile \"/Working/nuget.config\" --source \"https://example.com/source\" --add-source \"https://example.com/add-source\" --prerelease --tool-manifest \"/Working/.config/dotnet-tools.json\" --framework \"net10.0\" --disable-parallel --ignore-failed-sources --no-http-cache --interactive --allow-downgrade --arch \"x64\" --create-manifest-if-needed --allow-roll-forward";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolExecute_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ExecutePackageWithSettings,
                    CommandArgument = "dotnetsay"
                };
                fixture.ExecuteSettings.Version = "1.2.3";
                fixture.ExecuteSettings.ConfigFile = "./nuget.config";
                fixture.ExecuteSettings.Source.Add("https://example.com/source");
                fixture.ExecuteSettings.AddSource.Add("https://example.com/add-source");
                fixture.ExecuteSettings.Prerelease = true;
                fixture.ExecuteSettings.AllowRollForward = true;
                fixture.ExecuteSettings.DisableParallel = true;
                fixture.ExecuteSettings.IgnoreFailedSources = true;
                fixture.ExecuteSettings.NoHttpCache = true;
                fixture.ExecuteSettings.Interactive = true;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" exec dotnetsay --version \"1.2.3\" --configfile \"/Working/nuget.config\" --source \"https://example.com/source\" --add-source \"https://example.com/add-source\" --prerelease --allow-roll-forward --disable-parallel --ignore-failed-sources --no-http-cache --interactive";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolList_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ListFull,
                    CommandArgument = "dotnetsay"
                };
                fixture.ListSettings.InstallationScope = DotNetToolInstallationScope.Local;
                fixture.ListSettings.Format = DotNetToolListFormat.Json;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" list dotnetsay --local --format json";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolRestore_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.RestoreFull
                };
                fixture.RestoreSettings.ConfigFile = "./nuget.config";
                fixture.RestoreSettings.AddSource.Add("https://example.com/add-source");
                fixture.RestoreSettings.ToolManifest = "./.config/dotnet-tools.json";
                fixture.RestoreSettings.DisableParallel = true;
                fixture.RestoreSettings.IgnoreFailedSources = true;
                fixture.RestoreSettings.NoHttpCache = true;
                fixture.RestoreSettings.Interactive = true;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" restore --configfile \"/Working/nuget.config\" --add-source \"https://example.com/add-source\" --tool-manifest \"/Working/.config/dotnet-tools.json\" --disable-parallel --ignore-failed-sources --no-http-cache --interactive";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolRun_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.RunFull,
                    CommandArgument = "dotnetsay"
                };
                fixture.RunSettings.AllowRollForward = true;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" run dotnetsay --allow-roll-forward";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolRun_WithArgumentCustomization_RestoresOriginalCustomization()
            {
                // Given
                Func<ProcessArgumentBuilder, ProcessArgumentBuilder> addInteractive = arguments => arguments.Append("--interactive");
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.RunCommandArgumentsAndSettings,
                    CommandArgument = "dotnetsay"
                };
                fixture.RunArguments = Arguments("--tool-option");
                fixture.RunSettings.ArgumentCustomization = addInteractive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"tool\" run dotnetsay --interactive -- --tool-option", result.Args);
                Assert.Same(addInteractive, fixture.RunSettings.ArgumentCustomization);
            }

            [Fact]
            public void DotNetToolSearch_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.SearchFull,
                    CommandArgument = "cake"
                };
                fixture.SearchSettings.Detail = true;
                fixture.SearchSettings.Skip = 10;
                fixture.SearchSettings.Take = 20;
                fixture.SearchSettings.Prerelease = true;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" search cake --detail --skip 10 --take 20 --prerelease";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolUninstall_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.UninstallFull,
                    CommandArgument = "dotnetsay"
                };
                fixture.UninstallSettings.InstallationScope = DotNetToolInstallationScope.Local;
                fixture.UninstallSettings.ToolManifest = "./.config/dotnet-tools.json";

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" uninstall dotnetsay --local --tool-manifest \"/Working/.config/dotnet-tools.json\"";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolUpdate_RenderCommandSpecificSettings()
            {
                // Given
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.UpdateFull,
                    CommandArgument = null
                };
                fixture.UpdateSettings.InstallationScope = DotNetToolInstallationScope.Global;
                fixture.UpdateSettings.Version = "1.2.3";
                fixture.UpdateSettings.ConfigFile = "./nuget.config";
                fixture.UpdateSettings.Source.Add("https://example.com/source");
                fixture.UpdateSettings.AddSource.Add("https://example.com/add-source");
                fixture.UpdateSettings.Prerelease = true;
                fixture.UpdateSettings.ToolManifest = "./.config/dotnet-tools.json";
                fixture.UpdateSettings.Framework = "net10.0";
                fixture.UpdateSettings.DisableParallel = true;
                fixture.UpdateSettings.IgnoreFailedSources = true;
                fixture.UpdateSettings.NoHttpCache = true;
                fixture.UpdateSettings.Interactive = true;
                fixture.UpdateSettings.AllowDowngrade = true;
                fixture.UpdateSettings.All = true;

                // When
                var result = fixture.Run();

                // Then
                var expected = "\"tool\" update --global --version \"1.2.3\" --configfile \"/Working/nuget.config\" --source \"https://example.com/source\" --add-source \"https://example.com/add-source\" --prerelease --tool-manifest \"/Working/.config/dotnet-tools.json\" --framework \"net10.0\" --disable-parallel --ignore-failed-sources --no-http-cache --interactive --allow-downgrade --all";
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void DotNetToolExecute_WithArgumentCustomization_RestoresOriginalCustomization()
            {
                // Given
                Func<ProcessArgumentBuilder, ProcessArgumentBuilder> addInteractive = arguments => arguments.Append("--interactive");
                var fixture = new DotNetToolCommandFixture
                {
                    Invocation = DotNetToolCommandInvocation.ExecutePackageWithArgumentsAndSettings,
                    CommandArgument = "dotnetsay"
                };
                fixture.ExecuteArguments = Arguments("--tool-option");
                fixture.ExecuteSettings.ArgumentCustomization = addInteractive;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"tool\" exec dotnetsay --interactive -- --tool-option", result.Args);
                Assert.Same(addInteractive, fixture.ExecuteSettings.ArgumentCustomization);
            }

            private static ProcessArgumentBuilder Arguments(params string[] arguments)
            {
                var builder = new ProcessArgumentBuilder();
                foreach (var argument in arguments)
                {
                    builder.Append(argument);
                }

                return builder;
            }
        }

        private enum DotNetToolCommandInvocation
        {
            ExecutePackageWithSettings,
            ExecutePackageOnly,
            ExecutePackageWithArguments,
            ExecutePackageWithArgumentsAndSettings,
            InstallFull,
            ListFull,
            ListSettings,
            ListPackageSettings,
            RestoreFull,
            RestoreSettings,
            RunFull,
            RunCommandArguments,
            RunCommandArgumentsAndSettings,
            RunCommandSettings,
            SearchFull,
            UninstallFull,
            UpdateFull
        }

        private sealed class DotNetToolCommandFixture : DotNetFixture<DotNetToolSettings>
        {
            public FilePath ProjectPath { get; set; }

            public string CommandArgument { get; set; }

            public DotNetToolCommandInvocation Invocation { get; set; }

            public DotNetToolExecuteSettings ExecuteSettings { get; set; } = new DotNetToolExecuteSettings();

            public ProcessArgumentBuilder ExecuteArguments { get; set; } = new ProcessArgumentBuilder();

            public DotNetToolInstallSettings InstallSettings { get; set; } = new DotNetToolInstallSettings();

            public DotNetToolListSettings ListSettings { get; set; } = new DotNetToolListSettings();

            public DotNetToolRestoreSettings RestoreSettings { get; set; } = new DotNetToolRestoreSettings();

            public DotNetToolRunSettings RunSettings { get; set; } = new DotNetToolRunSettings();

            public ProcessArgumentBuilder RunArguments { get; set; } = new ProcessArgumentBuilder();

            public DotNetToolSearchSettings SearchSettings { get; set; } = new DotNetToolSearchSettings();

            public DotNetToolUninstallSettings UninstallSettings { get; set; } = new DotNetToolUninstallSettings();

            public DotNetToolUpdateSettings UpdateSettings { get; set; } = new DotNetToolUpdateSettings();

            public void ConfigureCommonSettings()
            {
                ExecuteSettings.Verbosity = DotNetVerbosity.Minimal;
                ExecuteArguments = Arguments("--tool-option", "tool-value");

                InstallSettings.Verbosity = DotNetVerbosity.Minimal;
                InstallSettings.InstallationScope = DotNetToolInstallationScope.Global;

                ListSettings.Verbosity = DotNetVerbosity.Minimal;
                ListSettings.InstallationScope = DotNetToolInstallationScope.Global;

                RestoreSettings.Verbosity = DotNetVerbosity.Minimal;
                RestoreSettings.DisableParallel = true;

                RunSettings.Verbosity = DotNetVerbosity.Minimal;
                RunArguments = Arguments("Hello", "--name", "World");

                SearchSettings.Verbosity = DotNetVerbosity.Minimal;
                SearchSettings.Detail = true;

                UninstallSettings.Verbosity = DotNetVerbosity.Minimal;
                UninstallSettings.InstallationScope = DotNetToolInstallationScope.Global;

                UpdateSettings.Verbosity = DotNetVerbosity.Minimal;
                UpdateSettings.InstallationScope = DotNetToolInstallationScope.Global;
            }

            protected override void RunTool()
            {
                var context = CreateContext();

                switch (Invocation)
                {
                    case DotNetToolCommandInvocation.ExecutePackageOnly:
                        context.DotNetToolExecute(CommandArgument);
                        break;
                    case DotNetToolCommandInvocation.ExecutePackageWithArguments:
                        context.DotNetToolExecute(CommandArgument, ExecuteArguments);
                        break;
                    case DotNetToolCommandInvocation.ExecutePackageWithArgumentsAndSettings:
                        context.DotNetToolExecute(CommandArgument, ExecuteArguments, ExecuteSettings);
                        break;
                    case DotNetToolCommandInvocation.ExecutePackageWithSettings:
                        context.DotNetToolExecute(CommandArgument, ExecuteSettings);
                        break;
                    case DotNetToolCommandInvocation.InstallFull:
                        context.DotNetToolInstall(CommandArgument, InstallSettings);
                        break;
                    case DotNetToolCommandInvocation.ListFull:
                        context.DotNetToolList(CommandArgument, ListSettings);
                        break;
                    case DotNetToolCommandInvocation.ListSettings:
                        context.DotNetToolList(ListSettings);
                        break;
                    case DotNetToolCommandInvocation.ListPackageSettings:
                        context.DotNetToolList(CommandArgument, ListSettings);
                        break;
                    case DotNetToolCommandInvocation.RestoreFull:
                        context.DotNetToolRestore(RestoreSettings);
                        break;
                    case DotNetToolCommandInvocation.RestoreSettings:
                        context.DotNetToolRestore(RestoreSettings);
                        break;
                    case DotNetToolCommandInvocation.RunFull:
                        context.DotNetToolRun(CommandArgument, RunArguments, RunSettings);
                        break;
                    case DotNetToolCommandInvocation.RunCommandArguments:
                        context.DotNetToolRun(CommandArgument, RunArguments);
                        break;
                    case DotNetToolCommandInvocation.RunCommandArgumentsAndSettings:
                        context.DotNetToolRun(CommandArgument, RunArguments, RunSettings);
                        break;
                    case DotNetToolCommandInvocation.RunCommandSettings:
                        context.DotNetToolRun(CommandArgument, RunSettings);
                        break;
                    case DotNetToolCommandInvocation.SearchFull:
                        context.DotNetToolSearch(CommandArgument, SearchSettings);
                        break;
                    case DotNetToolCommandInvocation.UninstallFull:
                        context.DotNetToolUninstall(CommandArgument, UninstallSettings);
                        break;
                    case DotNetToolCommandInvocation.UpdateFull:
                        context.DotNetToolUpdate(CommandArgument, UpdateSettings);
                        break;
                }
            }

            private static ProcessArgumentBuilder Arguments(params string[] arguments)
            {
                var builder = new ProcessArgumentBuilder();
                foreach (var argument in arguments)
                {
                    builder.Append(argument);
                }

                return builder;
            }

            private ICakeContext CreateContext()
            {
                var arguments = new CakeArguments(Enumerable.Empty<string>().ToLookup(argument => argument, StringComparer.Ordinal));

                return new CakeContext(
                    FileSystem,
                    Environment,
                    Globber,
                    new FakeLog(),
                    arguments,
                    ProcessRunner,
                    Substitute.For<IRegistry>(),
                    Tools,
                    Substitute.For<ICakeDataService>(),
                    Configuration);
            }
        }
    }
}
