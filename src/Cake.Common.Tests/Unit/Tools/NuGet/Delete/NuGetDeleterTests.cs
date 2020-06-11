// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.NuGet.Deleter;
using Cake.Common.Tools.NuGet;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NuGet.Delete
{
    public sealed class NuGetDeleterTests
    {
        public sealed class TheDeleteMethod
        {
            [Fact]
            public void Should_Throw_If_PackageID_Is_Null()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.PackageID = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageID");
            }

            [Fact]
            public void Should_Throw_If_PackageID_Is_Empty()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.PackageID = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageID");
            }

            [Fact]
            public void Should_Throw_If_PackageID_Is_WhiteSpace()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.PackageID = " ";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageID");
            }

            [Fact]
            public void Should_Throw_If_PackageVersion_Is_Null()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.PackageVersion = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageVersion");
            }

            [Fact]
            public void Should_Throw_If_PackageVersion_Is_Empty()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.PackageVersion = string.Empty;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageVersion");
            }

            [Fact]
            public void Should_Throw_If_PackageVersion_Is_WhiteSpace()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.PackageVersion = " ";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packageVersion");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
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
                var fixture = new NuGetDeleterFixture();
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
                var fixture = new NuGetDeleterFixture();
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
                var fixture = new NuGetDeleterFixture();
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
                var fixture = new NuGetDeleterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/NuGet.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
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
                var fixture = new NuGetDeleterFixture();
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
                var fixture = new NuGetDeleterFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("delete existing 0.1.0 -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Api_Key_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.Settings.ApiKey = "1234";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("delete existing 0.1.0 1234 -NonInteractive", result.Args);
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.Settings.ConfigFile = "./NuGet.config";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("delete existing 0.1.0 -NonInteractive " +
                             "-ConfigFile \"/Working/NuGet.config\"", result.Args);
            }

            [Fact]
            public void Should_Add_Source_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.Settings.Source = "http://customsource/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("delete existing 0.1.0 -NonInteractive " +
                             "-Source \"http://customsource/\"", result.Args);
            }

            [Theory]
            [InlineData(NuGetVerbosity.Detailed, "delete existing 0.1.0 -NonInteractive -Verbosity detailed")]
            [InlineData(NuGetVerbosity.Normal, "delete existing 0.1.0 -NonInteractive -Verbosity normal")]
            [InlineData(NuGetVerbosity.Quiet, "delete existing 0.1.0 -NonInteractive -Verbosity quiet")]
            public void Should_Add_Verbosity_To_Arguments_If_Not_Null(NuGetVerbosity verbosity, string expected)
            {
                // Given
                var fixture = new NuGetDeleterFixture();
                fixture.Settings.Verbosity = verbosity;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }
        }
    }
}