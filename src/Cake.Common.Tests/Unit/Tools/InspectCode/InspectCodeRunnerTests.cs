// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools.InspectCode;
using Cake.Common.Tools.InspectCode;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.InspectCode
{
    public sealed class InspectCodeRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Solution_Is_Null()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Solution = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "solution");
            }

            [Fact]
            public void Should_Find_Inspect_Code_Runner()
            {
                // Given
                var fixture = new InspectCodeRunFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/inspectcode.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Solution_In_Process_Arguments()
            {
                // Given
                var fixture = new InspectCodeRunFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InspectCode: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InspectCode: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Set_Output()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.OutputFile = "build/inspect_code.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/output:/Working/build/inspect_code.xml\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_OutputFile_Contains_Violations_And_Set_To_Throw()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.OutputFile = new FilePath("build/violations.xml");
                fixture.Settings.ThrowExceptionOnFindingViolations = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "Code Inspection Violations found in code base.");
            }

            [Fact]
            public void Should_Throw_If_Solution_Wide_Analysis_Is_Both_Disabled_And_Enabled()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.SolutionWideAnalysis = true;
                fixture.Settings.NoSolutionWideAnalysis = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("InspectCode: You can't set both SolutionWideAnalysis and NoSolutionWideAnalysis to true", result?.Message);
            }

            [Fact]
            public void Should_Set_Solution_Wide_Analysis_Switch()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.SolutionWideAnalysis = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/swea \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_No_Solution_Wide_Analysis_Switch()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.NoSolutionWideAnalysis = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/no-swea \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Project_Filter()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.ProjectFilter = "Test.*";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/project=Test.*\" \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_MsBuild_Properties()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.MsBuildProperties = new Dictionary<string, string>();
                fixture.Settings.MsBuildProperties.Add("TreatWarningsAsErrors", "true");
                fixture.Settings.MsBuildProperties.Add("Optimize", "false");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/properties:TreatWarningsAsErrors=true\" " +
                             "\"/properties:Optimize=false\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Caches_Home()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.CachesHome = "caches/";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/caches-home=/Working/caches\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Resharper_Plugins()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.Extensions = new[] { "ReSharper.AgentSmith", "X.Y" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/extensions=ReSharper.AgentSmith;X.Y\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Debug_Switch()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.Debug = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/debug \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_No_Buildin_Settings_Switch()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.NoBuildinSettings = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/no-buildin-settings \"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Disabled_Settings_Layers()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.DisabledSettingsLayers = new[]
                {
                    SettingsLayer.GlobalAll,
                    SettingsLayer.GlobalPerProduct,
                    SettingsLayer.SolutionShared,
                    SettingsLayer.SolutionPersonal,
                    SettingsLayer.ProjectShared,
                    SettingsLayer.ProjectPersonal,
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/dsl=GlobalAll;GlobalPerProduct;" +
                             "SolutionShared;SolutionPersonal;" +
                             "ProjectShared;ProjectPersonal\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }

            [Fact]
            public void Should_Set_Profile()
            {
                // Given
                var fixture = new InspectCodeRunFixture();
                fixture.Settings.Profile = "profile.DotSettings";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/profile=/Working/profile.DotSettings\" " +
                             "\"/Working/Test.sln\"", result.Args);
            }
        }

        public sealed class TheRunFromConfigMethod
        {
            [Fact]
            public void Should_Throw_If_Config_File_Is_Null()
            {
                // Given
                var fixture = new InspectCodeRunFromConfigFixture();
                fixture.Config = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "configFile");
            }

            [Fact]
            public void Should_Use_Provided_Config_File()
            {
                // Given
                var fixture = new InspectCodeRunFromConfigFixture();
                fixture.Config = "config.xml";

                // Then
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/config=/Working/config.xml\"", result.Args);
            }
        }
    }
}