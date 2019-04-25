// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.OctopusDeploy;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.OctopusDeploy
{
    public sealed class OctopusDeploymentQueryTests
    {
        public sealed class TheQueryDeploymentsMethod
        {
            [Fact]
            public void Should_Throw_If_Count_Is_Less_Than_1()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.Count = 0;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentOutOfRangeException(result, "Query must return at least one result");
            }

            [Fact]
            public void Should_Map_Count_To_Query_Filter()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.Count = 10;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list-deployments --number 10 " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Add_ProjectName_To_Query_Filter()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.ProjectName = "Project A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list-deployments --project \"Project A\" --number 1 " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Add_EnvironmentName_To_Query_Filter()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.EnvironmentName = "Env A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list-deployments --environment \"Env A\" --number 1 " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Add_TenantName_To_Query_Filter()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.TenantName = "Tenant A";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list-deployments --tenant \"Tenant A\" --number 1 " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Add_Space_To_Query_Filter_If_Not_Null()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.TenantName = "Tenant A";
                fixture.Settings.Space = "spacename";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list-deployments --tenant \"Tenant A\" --number 1 " +
                             "--server http://octopus --apiKey API-12345 --space \"spacename\"", result.Args);
            }

            [Fact]
            public void Should_Map_All_Query_Filter_Options()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.ProjectName = "Project A";
                fixture.Settings.EnvironmentName = "Env A";
                fixture.Settings.TenantName = "Tenant A";
                fixture.Settings.Count = 5;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("list-deployments --environment \"Env A\" " +
                             "--project \"Project A\" " +
                             "--tenant \"Tenant A\" --number 5 " +
                             "--server http://octopus --apiKey API-12345", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Octo_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Octo: Could not locate executable.");
            }

            [Theory]
            [InlineData("/bin/tools/octopus/Octo.exe", "/bin/tools/octopus/Octo.exe")]
            [InlineData("./tools/octopus/Octo.exe", "/Working/tools/octopus/Octo.exe")]
            public void Should_Use_Octo_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/octopusDeploy/Octo.exe", "C:/octopusDeploy/Octo.exe")]
            public void Should_Use_Octo_Executable_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
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
                var fixture = new OctopusDeploymentQuerierFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/Octo.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new OctopusDeploymentQuerierFixture();
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
                var fixture = new OctopusDeploymentQuerierFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Octo: Process returned an error (exit code 1).");
            }
        }

        public sealed class TheResultParser
        {
            [Fact]
            public void Should_Return_Empty_List_If_No_Valid_Results_Exist()
            {
                var parser = new DeploymentQueryResultParser();
                var t = Results.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);

                var results = parser.ParseResults(t.ToList().Take(4));

                Assert.Empty(results);
            }

            [Fact]
            public void Should_Handle_Empty_Response_Gracefully()
            {
                var parser = new DeploymentQueryResultParser();

                var results = parser.ParseResults(null);
                Assert.Empty(results);
            }

            [Fact]
            public void Should_Parse_Correct_Response_From_Octo_EXE()
            {
                var parser = new DeploymentQueryResultParser();
                var t = Results.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);

                var results = parser.ParseResults(t);
                Assert.Equal(3, results.Count());

                var expected = new OctopusDeployment
                                {
                                    Assembled = DateTimeOffset.Parse("11/2/2017 12:53:34 -04:00"),
                                    Channel = "Default",
                                    Created = DateTimeOffset.Parse("11/2/2017 12:53:35 -04:00"),
                                    Environment = "Staging",
                                    PackageVersions = "Package A 0.9.104; Package B 0.9.104",
                                    ProjectName = "Project A",
                                    ReleaseNotesHtml = "<h1> Project A </h1>",
                                    Version = "0.9.104"
                                };
                var actual = results.First();
                Assert.Equal(expected.Environment, actual.Environment);
                Assert.Equal(expected.Assembled, actual.Assembled);
                Assert.Equal(expected.Channel, actual.Channel);
                Assert.Equal(expected.Created, actual.Created);
                Assert.Equal(expected.PackageVersions, actual.PackageVersions);
                Assert.Equal(expected.ProjectName, actual.ProjectName);
                Assert.Equal(expected.ReleaseNotesHtml, actual.ReleaseNotesHtml);
                Assert.Equal(expected.Version, actual.Version);

                Assert.Equal("Package A 0.5.114; Package B 0.5.114", results.Last().PackageVersions);
            }

            private string Results =
@"Octopus Deploy Command Line Tool, version 4.24.4

Handshaking with Octopus server: http://octo
Handshake successful. Octopus version: 3.17.2; API version: 3.0.0
Authenticated as: user (a service account)
Loading projects...
Loading environments...
Loading tenants...
Loading deployments...
Showing 3 results...
 - Project: Project A
 - Environment: Staging
 - Channel: Default
   Created: 11/2/2017 12:53:35 -04:00
   Version: 0.9.104
   Assembled: 11/2/2017 12:53:34 -04:00
   Package Versions: Package A 0.9.104; Package B 0.9.104
   Release Notes: <h1> Project A </h1>

 - Project: Project B
 - Environment: Production
 - Channel: Default
   Created: 11/1/2017 16:43:36 -04:00
   Version: 0.5.115
   Assembled: 11/1/2017 16:43:35 -04:00
   Package Versions: Package C 0.5.115; Package D 0.5.115
   Release Notes: <h1> Project B</h1>

 - Project: Project A
 - Environment: Production
 - Channel: Default
   Created: 11/1/2017 16:31:09 -04:00
   Version: 0.5.114
   Assembled: 11/1/2017 16:31:09 -04:00
   Package Versions: Package A 0.5.114; Package B 0.5.114
   Release Notes: <h1> Public Website</h1><p>lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor lorem ipsum dolor </p>

";
        }
    }
}
