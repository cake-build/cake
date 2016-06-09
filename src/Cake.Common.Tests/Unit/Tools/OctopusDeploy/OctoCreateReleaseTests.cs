// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.OctopusDeploy
{
    public sealed class OctoCreateReleaseTests
    {
        public sealed class TheCreateReleaseMethod
        {
            [Fact]
            public void Should_Throw_If_Project_Name_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.ProjectName = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "projectName");
            }

            [Fact]
            public void Should_Throw_If_Server_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Server = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentException(result, "settings", "No server specified.");
            }

            [Fact]
            public void Should_Throw_If_Api_Key_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ApiKey = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentException(result, "settings", "No API key specified.");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Octo_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "Octo: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/tools/octopus/octo.exe", "/bin/tools/octopus/octo.exe")]
            [InlineData("./tools/octopus/octo.exe", "/Working/tools/octopus/octo.exe")]
            public void Should_Use_Octo_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
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
                var fixture = new OctopusDeployReleaseCreatorFixture();
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
                var fixture = new OctopusDeployReleaseCreatorFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/Octo.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "Octo: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsCakeException(result, "Octo: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Add_Project_Name_Server_And_Api_Key_To_Arguments()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.ProjectName = "myProject";
                fixture.Settings.Server = "http://myoctopusserver/";
                fixture.Settings.ApiKey = "API-ABCDEF123456";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"myProject\" " +
                             "--server http://myoctopusserver/ " +
                             "--apiKey API-ABCDEF123456", result.Args);
            }

            [Fact]
            public void Should_Add_Username_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Username = "mike123";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--username \"mike123\"", result.Args);
            }

            [Fact]
            public void Should_Add_Password_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Password = "secret";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--password \"secret\"", result.Args);
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ConfigurationFile = "configFile.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--configFile \"/Working/configFile.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Debug_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.EnableDebugLogging = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--debug", result.Args);
            }

            [Fact]
            public void Should_Add_Ignore_Ssl_Errors_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.IgnoreSslErrors = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--ignoreSslErrors", result.Args);
            }

            [Fact]
            public void Should_Add_Enable_Service_Messages_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.EnableServiceMessages = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--enableServiceMessages", result.Args);
            }

            [Fact]
            public void Should_Add_Release_Number_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ReleaseNumber = "3.0.0";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--releaseNumber \"3.0.0\"", result.Args);
            }

            [Fact]
            public void Should_Add_Default_Package_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DefaultPackageVersion = "1.5.2-beta";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--defaultpackageversion \"1.5.2-beta\"", result.Args);
            }

            [Fact]
            public void Should_Add_Package_And_Step_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Packages = new Dictionary<string, string>
                {
                    { "StepA", "1.0.1" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--package \"StepA:1.0.1\"", result.Args);
            }

            [Fact]
            public void Should_Add_Multiple_Package_And_Step_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Packages = new Dictionary<string, string>
                {
                    { "StepA", "1.0.1" },
                    { "StepB", "1.0.2" }
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--package \"StepA:1.0.1\" " +
                             "--package \"StepB:1.0.2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Packages_Folder_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.PackagesFolder = @"some\folder";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--packagesFolder \"/Working/some/folder\"", result.Args);
            }

            [Fact]
            public void Should_Add_Release_Notes_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ReleaseNotes = @"LOL";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--releasenotes \"LOL\"", result.Args);
            }

            [Fact]
            public void Should_Add_Release_Notes_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ReleaseNotesFile = @"some\folder\releaseNotes.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--releasenotesfile \"/Working/some/folder/releaseNotes.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Ignore_Existing_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.IgnoreExisting = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--ignoreexisting", result.Args);
            }
        }
    }
}
