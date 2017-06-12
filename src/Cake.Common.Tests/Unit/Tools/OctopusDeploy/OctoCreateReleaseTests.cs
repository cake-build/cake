// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
                AssertEx.IsArgumentNullException(result, "projectName");
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
                AssertEx.IsArgumentException(result, "settings", "No server specified.");
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
                AssertEx.IsArgumentException(result, "settings", "No API key specified.");
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
                AssertEx.IsArgumentNullException(result, "settings");
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
                AssertEx.IsCakeException(result, "Octo: Could not locate executable.");
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
                AssertEx.IsCakeException(result, "Octo: Process was not started.");
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
                AssertEx.IsCakeException(result, "Octo: Process returned an error (exit code 1).");
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
                             "--user \"mike123\"", result.Args);
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
                             "--pass \"secret\"", result.Args);
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
            public void Should_Add_Ignore_Existing_Flag_To_Arguments_If_True()
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

        public sealed class DeploymentAgrumentsBuilder
        {
            [Fact]
            public void Should_Add_DeployTo_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\"", result.Args);
            }

            [Fact]
            public void Should_Add_Progress_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.ShowProgress = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--progress", result.Args);
            }

            [Fact]
            public void Should_Add_FocePackageDownload_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.ForcePackageDownload = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--forcepackagedownload", result.Args);
            }

            [Fact]
            public void Should_Add_WaitForDeployment_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.WaitForDeployment = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--waitfordeployment", result.Args);
            }

            [Fact]
            public void Should_Add_DeploymentTimeout_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.DeploymentTimeout = TimeSpan.FromMinutes(1);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--deploymenttimeout=\"00:01:00\"", result.Args);
            }

            [Fact]
            public void Should_Add_CancelTimeout_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.CancelOnTimeout = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--cancelontimeout", result.Args);
            }

            [Fact]
            public void Should_Add_DeploymentChecksLeepCycle_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.DeploymentChecksLeepCycle = TimeSpan.FromMinutes(77);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--deploymentchecksleepcycle=\"01:17:00\"", result.Args);
            }

            [Fact]
            public void Should_Add_GuidedFailure_To_Arguments_If_True()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.GuidedFailure = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--guidedfailure=True", result.Args);
            }

            [Fact]
            public void Should_Add_GuidedFailure_To_Arguments_If_False()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.GuidedFailure = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--guidedfailure=True", result.Args);
            }

            [Fact]
            public void Should_Add_SpecificMachines_To_Arguments_If_NotNull()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.SpecificMachines = new string[] { "Machine1", "Machine2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--specificmachines=\"Machine1,Machine2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Force_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.Force = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--force", result.Args);
            }

            [Fact]
            public void Should_Add_SkipSteps_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.SkipSteps = new[] { "Step1", "Step2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--skip=\"Step1\" " +
                             "--skip=\"Step2\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoRawLog_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.NoRawLog = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--norawlog", result.Args);
            }

            [Fact]
            public void Should_Add_RawLogFile_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.RawLogFile = "someFile.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--rawlogfile \"/Working/someFile.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Variables_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.Variables.Add(new KeyValuePair<string, string>("var1", "value1"));
                fixture.Settings.Variables.Add(new KeyValuePair<string, string>("var2", "value2"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--variable=\"var1:value1\" " +
                             "--variable=\"var2:value2\"", result.Args);
            }

            [Fact]
            public void Should_Add_DeployAt_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.DeployAt = new DateTime(2010, 6, 15).AddMinutes(1);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--deployat=\"2010-06-15 00:01\"", result.Args);
            }

            [Fact]
            public void Should_Add_Tenants_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.Tenant = new[] { "Tenant1", "Tenant2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--tenant=\"Tenant1\" " +
                             "--tenant=\"Tenant2\"", result.Args);
            }

            [Fact]
            public void Should_Add_TenantTags_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.TenantTags = new[] { "Tag1", "Tag2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--tenanttag=\"Tag1\" " +
                             "--tenanttag=\"Tag2\"", result.Args);
            }

            [Fact]
            public void Should_Add_All_Deploymnet_Arguments()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = "SomeEnvironment";
                fixture.Settings.ShowProgress = true;
                fixture.Settings.ForcePackageDownload = true;
                fixture.Settings.WaitForDeployment = true;
                fixture.Settings.DeploymentTimeout = TimeSpan.FromMinutes(1);
                fixture.Settings.CancelOnTimeout = true;
                fixture.Settings.DeploymentChecksLeepCycle = TimeSpan.FromMinutes(77);
                fixture.Settings.GuidedFailure = true;
                fixture.Settings.SpecificMachines = new string[] { "Machine1", "Machine2" };
                fixture.Settings.Force = true;
                fixture.Settings.SkipSteps = new[] { "Step1", "Step2" };
                fixture.Settings.NoRawLog = true;
                fixture.Settings.RawLogFile = "someFile.txt";
                fixture.Settings.Variables.Add(new KeyValuePair<string, string>("var1", "value1"));
                fixture.Settings.Variables.Add(new KeyValuePair<string, string>("var2", "value2"));
                fixture.Settings.DeployAt = new DateTime(2010, 6, 15).AddMinutes(1);
                fixture.Settings.Tenant = new[] { "Tenant1", "Tenant2" };
                fixture.Settings.TenantTags = new[] { "Tag1", "Tag2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"SomeEnvironment\" " +
                             "--progress " +
                             "--forcepackagedownload " +
                             "--waitfordeployment " +
                             "--deploymenttimeout=\"00:01:00\" " +
                             "--cancelontimeout " +
                             "--deploymentchecksleepcycle=\"01:17:00\" " +
                             "--guidedfailure=True " +
                             "--specificmachines=\"Machine1,Machine2\" " +
                             "--force " +
                             "--skip=\"Step1\" " +
                             "--skip=\"Step2\" " +
                             "--norawlog " +
                             "--rawlogfile \"/Working/someFile.txt\" " +
                             "--variable=\"var1:value1\" " +
                             "--variable=\"var2:value2\" " +
                             "--deployat=\"2010-06-15 00:01\" " +
                             "--tenant=\"Tenant1\" " +
                             "--tenant=\"Tenant2\" " +
                             "--tenanttag=\"Tag1\" " +
                             "--tenanttag=\"Tag2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Channel_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.Channel = @"somechannel";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--channel \"somechannel\"", result.Args);
            }

            [Fact]
            public void Should_Add_Ignore_Channel_Rules_To_Arguments_If_True()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.IgnoreChannelRules = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--ignorechannelrules", result.Args);
            }

            [Fact]
            public void Should_Add_Deployment_Environment_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeployTo = @"someenvironment";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--deployto \"someenvironment\"", result.Args);
            }

            [Fact]
            public void Should_Add_Deployment_Progress_To_Arguments_If_True()
            {
                // Given
                var fixture = new OctopusDeployReleaseCreatorFixture();
                fixture.Settings.DeploymentProgress = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("create-release --project \"testProject\" " +
                             "--server http://octopus --apiKey API-12345 " +
                             "--progress", result.Args);
            }
        }
    }
}