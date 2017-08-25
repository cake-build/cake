// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.NuGet.Restorer;
using Cake.Common.Tools.NuGet;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Restore
{
    public sealed class NuGetRestorerTests
    {
        public sealed class TheRestoreMethod
        {
            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.TargetFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "targetFilePath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
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
                var fixture = new NuGetRestorerFixture();
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
                var fixture = new NuGetRestorerFixture();
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
                var fixture = new NuGetRestorerFixture();
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
                var fixture = new NuGetRestorerFixture();
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
                var fixture = new NuGetRestorerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetRestorerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_RequireConsent_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.RequireConsent = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -RequireConsent " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_PackageDirectory_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.PackagesDirectory = "./package";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" " +
                             "-PackagesDirectory \"/Working/package\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.Source = new[] { "A;B;C" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -Source \"A;B;C\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_NoCache_To_Arguments_If_True()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.NoCache = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -NoCache -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_DisableParallelProcessing_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.DisableParallelProcessing = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -DisableParallelProcessing " +
                             "-NonInteractive", result.Args);
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "restore \"/Working/project.sln\" -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "restore \"/Working/project.sln\" -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "restore \"/Working/project.sln\" -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetRestorerFixture();
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
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.ConfigFile = "./nuget.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -ConfigFile \"/Working/nuget.config\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_FallbackSources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.Source = new[] { "A;B;C" };
                fixture.Settings.FallbackSource = new[] { "D;E;F" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("restore \"/Working/project.sln\" -Source \"A;B;C\" -FallbackSource \"D;E;F\" -NonInteractive", result.Args);
            }

            [Theory]
            [InlineData(NuGetMSBuildVersion.MSBuild4, "restore \"/Working/project.sln\" -MSBuildVersion 4 -NonInteractive")]
            [InlineData(NuGetMSBuildVersion.MSBuild12, "restore \"/Working/project.sln\" -MSBuildVersion 12 -NonInteractive")]
            [InlineData(NuGetMSBuildVersion.MSBuild14, "restore \"/Working/project.sln\" -MSBuildVersion 14 -NonInteractive")]
            public void Should_Add_MSBuildVersion_To_Arguments_If_Set(NuGetMSBuildVersion msBuildVersion, string expected)
            {
                // Given
                var fixture = new NuGetRestorerFixture();
                fixture.Settings.MSBuildVersion = msBuildVersion;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}