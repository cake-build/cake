// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.NuGet.Sources;
using Cake.Common.Tools.NuGet;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Sources
{
    public sealed class NuGetSourcesTests
    {
        public sealed class TheAddSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Name = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "name");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Name = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Source name cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Source = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "source");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Source = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "source", "Source cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Adding_Source_That_Has_Already_Been_Added()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Source = "existingsource";
                fixture.GivenSourceAlreadyHasBeenAdded();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The source 'existingsource' already exist.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/nuget/nuget.exe", "/bin/tools/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
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
                var fixture = new NuGetAddSourceFixture();
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
                var fixture = new NuGetAddSourceFixture();
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
                var fixture = new NuGetAddSourceFixture();
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
                var fixture = new NuGetAddSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Add -Name \"name\" -Source \"source\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_UserName_And_Password_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Settings.UserName = "username";
                fixture.Settings.Password = "password";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Add -Name \"name\" -Source \"source\" " +
                             "-NonInteractive -UserName \"username\" " +
                             "-Password \"password\"", result.Args);
            }

            [Fact]
            public void Should_Add_Verbosity_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Settings.Verbosity = NuGetVerbosity.Detailed;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Add -Name \"name\" -Source \"source\" " +
                             "-Verbosity detailed -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Redact_Source_If_IsSensitiveSource_Set()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Settings.IsSensitiveSource = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Add -Name \"name\" -Source \"source\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_StorePasswordInClearText_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetAddSourceFixture();
                fixture.Settings.StorePasswordInClearText = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Add -Name \"name\" -Source \"source\" " +
                             "-NonInteractive -StorePasswordInClearText", result.Args);
            }
        }

        public sealed class TheRemoveSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Name_Is_Null()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.Name = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "name");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Name_Is_Empty(string name)
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.Name = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "name", "Source name cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.Source = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "source");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.Source = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "source", "Source cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Removing_Source_That_Do_Not_Exist()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("The source 'source' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_NuGet_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NuGet: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/nuget/nuget.exe", "/bin/tools/nuget/nuget.exe")]
            [InlineData("./tools/nuget/nuget.exe", "/Working/tools/nuget/nuget.exe")]
            public void Should_Use_NuGet_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
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
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
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
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
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
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
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
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Add_Mandatory_Arguments()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Remove -Name \"name\" -Source \"source\" " +
                             "-NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Verbosity_To_Arguments_If_Set()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
                fixture.Settings.Verbosity = NuGetVerbosity.Detailed;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Remove -Name \"name\" -Source \"source\" " +
                             "-Verbosity detailed -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Redact_Source_If_IsSensitiveSource_Set()
            {
                // Given
                var fixture = new NuGetRemoveSourceFixture();
                fixture.GivenExistingSource();
                fixture.Settings.IsSensitiveSource = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("sources Remove -Name \"name\" " +
                             "-Source \"source\" -NonInteractive", result.Args);
            }
        }

        public sealed class TheHasSourceMethod
        {
            [Fact]
            public void Should_Throw_If_Source_Is_Null()
            {
                // Given
                var fixture = new NuGetHasSourceFixture();
                fixture.Source = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "source");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Source_Is_Empty(string source)
            {
                // Given
                var fixture = new NuGetHasSourceFixture();
                fixture.Source = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "source", "Source cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetHasSourceFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }
        }
    }
}