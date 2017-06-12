// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.NuGet.Installer;
using Cake.Common.Tools.NuGet;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Install
{
    public sealed class NuGetInstallerTests
    {
        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Id_Is_Null()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.PackageId = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageId");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/nuget/nuget.exe", "/bin/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetInstallerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetInstallerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.RequireConsent = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -RequireConsent -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_SolutionDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.SolutionDirectory = "./solution";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -SolutionDirectory " +
                             "\"/Working/solution\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Source = new[] { "A;B;C" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -Source \"A;B;C\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.NoCache = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -NoCache -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.DisableParallelProcessing = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -DisableParallelProcessing " +
                             "-NonInteractive", result.Args);
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "install \"Cake\" -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "install \"Cake\" -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "install \"Cake\" -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -ConfigFile \"/Working/nuget.config\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_FallbackSources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFixture();
                fixture.Settings.Source = new[] { "A;B;C" };
                fixture.Settings.FallbackSource = new[] { "D;E;F" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"Cake\" -Source \"A;B;C\" -FallbackSource \"D;E;F\" -NonInteractive", result.Args);
            }
        }

        public sealed class TheInstallFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Target_Package_Config_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.PackageConfigPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageConfigPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/tools/nuget/nuget.exe", "/bin/tools/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/nuget/nuget.exe", "C:/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.RequireConsent = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -RequireConsent " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_SolutionDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.SolutionDirectory = "./solution";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" " +
                             "-SolutionDirectory \"/Working/solution\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.Source = new[] { "A;B;C" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -Source \"A;B;C\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.NoCache = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -NoCache " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.DisableParallelProcessing = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -DisableParallelProcessing " +
                             "-NonInteractive", result.Args);
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "detailed", "install \"/Working/packages.config\" -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "normal", "install \"/Working/packages.config\" -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "quiet", "install \"/Working/packages.config\" -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string name, string expected)
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_ConfigFile_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" " +
                             "-ConfigFile \"/Working/nuget.config\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_FallbackSources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetInstallerFromConfigFixture();
                fixture.Settings.Source = new[] { "A;B;C" };
                fixture.Settings.FallbackSource = new[] { "D;E;F" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("install \"/Working/packages.config\" -Source \"A;B;C\" -FallbackSource \"D;E;F\" " +
                             "-NonInteractive", result.Args);
            }
        }
    }
}