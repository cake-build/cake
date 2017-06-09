// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools.NuGet.Update;
using Cake.Common.Tools.NuGet;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Update
{
    public sealed class NuGetUpdaterTests
    {
        public sealed class TheUpdateMethod
        {
            [Fact]
            public void Should_Throw_If_Target_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.TargetFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "targetFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
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
                var fixture = new NuGetUpdateFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/nuget/nuget.exe", "/bin/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetUpdateFixture();
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
                var fixture = new NuGetUpdateFixture();
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
                var fixture = new NuGetUpdateFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetUpdateFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetUpdateFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("update \"/Working/packages.config\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Packages_If_Specified()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.Settings.Id = new List<string> { "A", "B" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("update \"/Working/packages.config\" -Id \"A;B\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Safe_Flag_If_Set()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.Settings.Safe = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("update \"/Working/packages.config\" " +
                             "-Safe -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Prerelease_Flag_If_Set()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.Settings.Prerelease = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("update \"/Working/packages.config\" " +
                             "-Prerelease -NonInteractive", result.Args);
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "update \"/Working/packages.config\" -Verbosity detailed -NonInteractive")]
            [InlineData(NuGetVerbosity.Normal, "update \"/Working/packages.config\" -Verbosity normal -NonInteractive")]
            [InlineData(NuGetVerbosity.Quiet, "update \"/Working/packages.config\" -Verbosity quiet -NonInteractive")]
            public void Should_Add_Verbosity_To_Arguments_If_Set(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Sources_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.Settings.Source = new[] { "A", "B", "C" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("update \"/Working/packages.config\" -Source \"A;B;C\" " +
                             "-NonInteractive", result.Args);
            }

            [Theory]
            [InlineData(NuGetMSBuildVersion.MSBuild4, "update \"/Working/packages.config\" -MSBuildVersion 4 -NonInteractive")]
            [InlineData(NuGetMSBuildVersion.MSBuild12, "update \"/Working/packages.config\" -MSBuildVersion 12 -NonInteractive")]
            [InlineData(NuGetMSBuildVersion.MSBuild14, "update \"/Working/packages.config\" -MSBuildVersion 14 -NonInteractive")]
            public void Should_Add_MSBuildVersion_To_Arguments_If_Set(NuGetMSBuildVersion msBuildVersion, string expected)
            {
                // Given
                var fixture = new NuGetUpdateFixture();
                fixture.Settings.MSBuildVersion = msBuildVersion;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}