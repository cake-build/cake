// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.OctopusDeploy
{
    public sealed class OctoDeployReleaseTests
    {
        private const string MinimalParameters = "deploy-release --project=\"MyProject\" --deployto=\"Testing\" --releasenumber=\"0.15.1\" --server http://octopus --apiKey API-12345";

        public sealed class TheBaseArgumentBuilder
        {
            [Fact]
            public void Should_Throw_If_Server_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Server = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "server");
            }

            [Fact]
            public void Should_Throw_If_Api_Key_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.ApiKey = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "apiKey");
            }

            [Fact]
            public void Should_Throw_If_ProjectName_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Project = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectName");
            }

            [Fact]
            public void Should_Throw_If_DeployTo_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.DeployTo = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "deployTo");
            }

            [Fact]
            public void Should_Throw_If_ReleaseNumber_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.ReleaseNumber = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "releaseNumber");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Give_Default_Minimal_Parameters()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters, result.Args);
            }

            [Fact]
            public void Should_Add_Username_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.Username = "mike123";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --user \"mike123\"", result.Args);
            }

            [Fact]
            public void Should_Add_Password_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.Password = "secret";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --pass \"secret\"", result.Args);
            }

            [Fact]
            public void Should_Add_Configuration_File_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.ConfigurationFile = "configFile.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --configFile \"/Working/configFile.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Debug_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.EnableDebugLogging = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --debug", result.Args);
            }

            [Fact]
            public void Should_Add_Ignore_Ssl_Errors_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.IgnoreSslErrors = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --ignoreSslErrors", result.Args);
            }

            [Fact]
            public void Should_Add_Enable_Service_Messages_Flag_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.EnableServiceMessages = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --enableServiceMessages", result.Args);
            }
        }

        public sealed class DeploymentArgumentBuilder
        {
            [Fact]
            public void Should_Add_Progress_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.ShowProgress = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --progress", result.Args);
            }

            [Fact]
            public void Should_Add_FocePackageDownload_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.ForcePackageDownload = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --forcepackagedownload", result.Args);
            }

            [Fact]
            public void Should_Add_WaitForDeployment_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.WaitForDeployment = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --waitfordeployment", result.Args);
            }

            [Fact]
            public void Should_Add_DeploymentTimeout_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.DeploymentTimeout = TimeSpan.FromMinutes(1);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --deploymenttimeout=\"00:01:00\"", result.Args);
            }

            [Fact]
            public void Should_Add_CancelTimeout_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.CancelOnTimeout = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --cancelontimeout", result.Args);
            }

            [Fact]
            public void Should_Add_DeploymentChecksLeepCycle_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.DeploymentChecksLeepCycle = TimeSpan.FromMinutes(77);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --deploymentchecksleepcycle=\"01:17:00\"", result.Args);
            }

            [Fact]
            public void Should_Add_GuidedFailure_To_Arguments_If_True()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.GuidedFailure = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --guidedfailure=True", result.Args);
            }

            [Fact]
            public void Should_Add_GuidedFailure_To_Arguments_If_False()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.GuidedFailure = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --guidedfailure=True", result.Args);
            }

            [Fact]
            public void Should_Add_SpecificMachines_To_Arguments_If_NotNull()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.SpecificMachines = new string[] { "Machine1", "Machine2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --specificmachines=\"Machine1,Machine2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Force_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.Force = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --force", result.Args);
            }

            [Fact]
            public void Should_Add_SkipSteps_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.SkipSteps = new[] { "Step1", "Step2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --skip=\"Step1\" --skip=\"Step2\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoRawLog_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.NoRawLog = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --norawlog", result.Args);
            }

            [Fact]
            public void Should_Add_RawLogFile_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.RawLogFile = "someFile.txt";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --rawlogfile \"/Working/someFile.txt\"", result.Args);
            }

            [Fact]
            public void Should_Add_Variables_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.Variables.Add("var1", "value1");
                fixture.Settings.Variables.Add("var2", "value2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters +
                             " --variable=\"var1:value1\"" +
                             " --variable=\"var2:value2\"", result.Args);
            }

            [Fact]
            public void Should_Add_DeployAt_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.DeployAt = new DateTime(2010, 6, 15).AddMinutes(1);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --deployat=\"2010-06-15 00:01\"", result.Args);
            }

            [Fact]
            public void Should_Add_Tenants_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.Tenant = new[] { "Tenant1", "Tenant2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters +
                             " --tenant=\"Tenant1\"" +
                             " --tenant=\"Tenant2\"", result.Args);
            }

            [Fact]
            public void Should_Add_TenantTags_To_Arguments_If_Specified()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.TenantTags = new[] { "Tag1", "Tag2" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters +
                             " --tenanttag=\"Tag1\"" +
                             " --tenanttag=\"Tag2\"", result.Args);
            }

            [Fact]
            public void Should_Add_Channel_To_Arguments_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeployReleaseDeployerFixture();
                fixture.Settings.Channel = @"somechannel";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(MinimalParameters + " --channel \"somechannel\"", result.Args);
            }
        }
    }
}
