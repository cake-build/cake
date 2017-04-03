// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.OctopusDeploy
{
    public sealed class OctoPushTests
    {
        public sealed class TheBaseArgumentBuilder
        {
            [Fact]
            public void Should_Throw_If_Server_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Server = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "settings", "No server specified.");
            }

            [Fact]
            public void Should_Throw_If_Api_Key_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.ApiKey = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "settings", "No API key specified.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Add_Username_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.Username = "mike123";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" --package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--user \"mike123\"", result.Args);
            }

            [Fact]
            public void Should_Add_Password_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.Password = "secret";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" --package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--pass \"secret\"", result.Args);
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.ConfigurationFile = "configFile.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" --package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--configFile \"/Working/configFile.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Debug_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.EnableDebugLogging = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" --package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--debug", result.Args);
            }

            [Fact]
            public void Should_Add_Ignore_Ssl_Errors_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.IgnoreSslErrors = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" --package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--ignoreSslErrors", result.Args);
            }

            [Fact]
            public void Should_Add_Enable_Service_Messages_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.EnableServiceMessages = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" --package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--enableServiceMessages", result.Args);
            }
        }
        public sealed class ThePushPackageMethod
        {
            [Fact]
            public void Should_Throw_If_No_Packages_Provided()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Packages = new List<FilePath>();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packagePaths");
            }

            [Fact]
            public void Should_Throw_If_Package_List_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Packages = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "packagePaths");
            }

            [Fact]
            public void Should_Add_Single_Argument_For_Single_Package()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Packages = new List<FilePath> { "MyPackage.1.0.0.zip" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_Arguments_For_Multiple_Packages()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Packages = new List<FilePath>
                {
                    "MyPackage.1.0.0.zip",
                    "MyOtherPackage.1.0.1.nupkg",
                    "MyExtraPackage.1.0.1.nupkg"
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" " +
                             "--package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--package \"/Working/MyExtraPackage.1.0.1.nupkg\" " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Add_Replace_Existing_Flag_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.ReplaceExisting = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("push --package \"/Working/MyPackage.1.0.0.zip\" " +
                             "--package \"/Working/MyOtherPackage.1.0.1.nupkg\" " +
                             "--replace-existing " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Octo_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Octo: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/tools/octopus/octo.exe", "/bin/tools/octopus/octo.exe")]
            [InlineData("./tools/octopus/octo.exe", "/Working/tools/octopus/octo.exe")]
            public void Should_Use_Octo_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/octopusDeploy/octo.exe", "C:/octopusDeploy/octo.exe")]
            public void Should_Use_Octo_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Octo_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/Octo.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Octo: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new OctopusDeployPusherFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Octo: Process returned an error (exit code 1).");
            }
        }
    }
}