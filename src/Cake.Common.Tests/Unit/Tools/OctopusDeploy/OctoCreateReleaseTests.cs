using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.OctopusDeploy;
using Cake.Core.IO;
using NSubstitute;
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
                var result = Record.Exception(() => fixture.CreateRelease());

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
                var result = Record.Exception(() => fixture.CreateRelease());

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
                var result = Record.Exception(() => fixture.CreateRelease());

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
                var result = Record.Exception(() => fixture.CreateRelease());

                // Then
                Assert.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Octo_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture(defaultToolExist: false);

                // When
                var result = Record.Exception(() => fixture.CreateRelease());

                // Then
                Assert.IsCakeException(result, "Octo: Could not locate executable.");
            }

            [Theory]
            [InlineData("C:/octopusDeploy/octo.exe", "C:/octopusDeploy/octo.exe")]
            [InlineData("./tools/octopus/octo.exe", "/Working/tools/octopus/octo.exe")]
            public void Should_Use_Octo_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture(expected);
                fixture.Settings.ToolPath = toolPath;

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_Octo_Executable_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/Octo.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.CreateRelease());

                // Then
                Assert.IsCakeException(result, "Octo: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.GivenProcessReturnError();

                // When
                var result = Record.Exception(() => fixture.CreateRelease());

                // Then
                Assert.IsCakeException(result, "Octo: Process returned an error.");
            }

            [Fact]
            public void Should_Add_Project_Name_Server_And_Api_Key_To_Arguments()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.ProjectName = "myProject";
                fixture.Settings = new CreateReleaseSettings
                    {
                        Server = "http://myoctopusserver/",
                        ApiKey = "API-ABCDEF123456"
                    };

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "create-release --project \"myProject\" --server http://myoctopusserver/ --apiKey API-ABCDEF123456"));
            }

            [Fact]
            public void Should_Add_Username_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Username = "mike123";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --username \"mike123\""));
            }

            [Fact]
            public void Should_Add_Password_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Password = "secret";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --password \"secret\""));
            }

            [Fact]
            public void Should_Redact_Api_Key_And_Password_Arguments()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.ProjectName = "myProject";
                fixture.Settings = new CreateReleaseSettings
                    {
                        Server = "http://myoctopusserver/",
                        ApiKey = "API-ABCDEF123456",
                        Password = "abc123"
                    };

                // When
                fixture.CreateRelease();
                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.RenderSafe() ==
                            "create-release --project \"myProject\" --server http://myoctopusserver/ --apiKey [REDACTED] --password \"[REDACTED]\""));
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ConfigurationFile = "configFile.txt";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --configFile \"/Working/configFile.txt\""));
            }

            [Fact]
            public void Should_Add_Debug_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.EnableDebugLogging = true;

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --debug"));
            }

            [Fact]
            public void Should_Add_Ignore_Ssl_Errors_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.IgnoreSslErrors = true;

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --ignoreSslErrors"));
            }

            [Fact]
            public void Should_Add_Enable_Service_Messages_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.EnableServiceMessages = true;

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --enableServiceMessages"));
            }

            [Fact]
            public void Should_Add_Release_Number_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ReleaseNumber = "3.0.0";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --releaseNumber \"3.0.0\""));
            }

            [Fact]
            public void Should_Add_Default_Package_Version_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DefaultPackageVersion = "1.5.2-beta";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --defaultpackageversion \"1.5.2-beta\""));
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
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --package \"StepA:1.0.1\""));
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
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --package \"StepA:1.0.1\" --package \"StepB:1.0.2\""));
            }

            [Fact]
            public void Should_Add_Packages_Folder_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.PackagesFolder = @"some\folder";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --packagesFolder \"/Working/some/folder\""));
            }

            [Fact]
            public void Should_Add_Release_Notes_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ReleaseNotes = @"No significant changes in this version...";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --releasenotes \"No significant changes in this version...\""));
            }

            [Fact]
            public void Should_Add_Release_Notes_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.ReleaseNotesFile = @"some\folder\releaseNotes.txt";

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --releasenotesfile \"/Working/some/folder/releaseNotes.txt\""));
            }

            [Fact]
            public void Should_Add_Ignore_Existing_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.IgnoreExisting = true;

                // When
                fixture.CreateRelease();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                            fixture.GetDefaultArguments() + " --ignoreexisting"));
            }
        }
    }
}