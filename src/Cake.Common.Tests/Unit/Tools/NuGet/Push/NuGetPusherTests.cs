// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.NuGet.Pusher;
using Cake.Common.Tools.NuGet;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Push
{
    public sealed class NuGetPusherTests
    {
        public sealed class ThePushMethod
        {
            [Fact]
            public void Should_Throw_If_Nuspec_File_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetPusherFixture();
                fixture.PackageFilePath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageFilePath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetPusherFixture();
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
                var fixture = new NuGetPusherFixture();
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
                var fixture = new NuGetPusherFixture();
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
                var fixture = new NuGetPusherFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_NuGet_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NuGetPusherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetPusherFixture();
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
                var fixture = new NuGetPusherFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "NuGet: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_NuGet_Package_To_Arguments()
            {
                // Given
                var fixture = new NuGetPusherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Api_Key_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPusherFixture();
                fixture.Settings.ApiKey = "1234";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" 1234 -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPusherFixture();
                fixture.Settings.ConfigFile = "./NuGet.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" -NonInteractive " +
                             "-ConfigFile \"/Working/NuGet.config\"", result.Args);
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPusherFixture();
                fixture.Settings.Source = "http://customsource/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" -NonInteractive " +
                             "-Source \"http://customsource/\"", result.Args);
            }

            [Fact]
            public void Should_Add_Timeout_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetPusherFixture();
                fixture.Settings.Timeout = TimeSpan.FromSeconds(987);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push \"/Working/existing.nupkg\" -NonInteractive " +
                             "-Timeout 987", result.Args);
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "push \"/Working/existing.nupkg\" -NonInteractive -Verbosity detailed")]
            [InlineData(NuGetVerbosity.Normal, "push \"/Working/existing.nupkg\" -NonInteractive -Verbosity normal")]
            [InlineData(NuGetVerbosity.Quiet, "push \"/Working/existing.nupkg\" -NonInteractive -Verbosity quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Not_Null(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetPusherFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}