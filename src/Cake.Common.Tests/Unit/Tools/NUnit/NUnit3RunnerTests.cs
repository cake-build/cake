// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.NUnit;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnit3RunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Assemblies = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_NUnit3_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit3: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/NUnit/nunit.exe", "/bin/NUnit/nunit.exe")]
            [InlineData("./tools/NUnit/nunit.exe", "/Working/tools/NUnit/nunit.exe")]
            public void Should_Use_NUnit3_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/NUnit/nunit.exe", "C:/NUnit/nunit.exe")]
            public void Should_Use_NUnit3_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_NUnit3_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/nunit3-console.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Assemblies.Clear();
                fixture.Assemblies.Add(new FilePath("./Test1.dll"));
                fixture.Assemblies.Add(new FilePath("./Test2.dll"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit3: Process was not started.", result?.Message);
            }

            [Theory]
            [InlineData(10, "NUnit3: 10 test(s) failed (exit code 10).")]
            [InlineData(-1, "NUnit3: Invalid argument (exit code -1).")]
            [InlineData(-2, "NUnit3: Invalid assembly (exit code -2).")]
            [InlineData(-4, "NUnit3: Invalid test fixture (exit code -4).")]
            [InlineData(-100, "NUnit3: Unexpected error (exit code -100).")]
            [InlineData(-10, "NUnit3: Unrecognised error (exit code -10).")]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code(int exitCode, string expectedMessage)
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.GivenProcessExitsWithCode(exitCode);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal(expectedMessage, result?.Message);
            }

            [Fact]
            public void Should_Not_Allow_NoResults_And_ResultsFile()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.Results = "NewResults.xml";
                fixture.Settings.NoResults = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("NUnit3: You can't specify both a results file and set NoResults to true.", result?.Message);
            }

            [Fact]
            public void Should_Set_Result_Switch()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.Results = "NewTestResult.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" " +
                             "\"--result=/Working/NewTestResult.xml\"",
                             result.Args);
            }

            [Fact]
            public void Should_Set_Commandline_Switches()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.Test = "Test1,Test2";
                fixture.Settings.TestList = "Testlist.txt";
                fixture.Settings.Where = "cat==Data";
                fixture.Settings.Timeout = 5;
                fixture.Settings.Seed = 6;
                fixture.Settings.Workers = 7;
                fixture.Settings.StopOnError = true;
                fixture.Settings.Work = "out";
                fixture.Settings.OutputFile = "stdout.txt";
                fixture.Settings.ErrorOutputFile = "stderr.txt";
                fixture.Settings.Full = true;
                fixture.Settings.Results = "NewTestResult.xml";
                fixture.Settings.ResultFormat = "nunit2";
                fixture.Settings.ResultTransform = "nunit2.xslt";
                fixture.Settings.Labels = NUnit3Labels.All;
                fixture.Settings.TeamCity = true;
                fixture.Settings.NoHeader = true;
                fixture.Settings.NoColor = true;
                fixture.Settings.Verbose = true;
                fixture.Settings.Configuration = "Debug";
                fixture.Settings.Process = NUnit3ProcessOption.InProcess;
                fixture.Settings.AppDomainUsage = NUnit3AppDomainUsage.Single;
                fixture.Settings.Framework = "net3.5";
                fixture.Settings.X86 = true;
                fixture.Settings.DisposeRunners = true;
                fixture.Settings.ShadowCopy = true;
                fixture.Settings.Agents = 3;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" --test=Test1,Test2 \"--testlist=/Working/Testlist.txt\" " +
                        "--where \"cat==Data\" --timeout=5 --seed=6 --workers=7 " +
                        "--stoponerror \"--work=/Working/out\" \"--out=/Working/stdout.txt\" " +
                        "\"--err=/Working/stderr.txt\" --full " +
                        "\"--result=/Working/NewTestResult.xml;format=nunit2;transform=/Working/nunit2.xslt\" " +
                        "--labels=All --teamcity --noheader --nocolor --verbose " +
                        "\"--config=Debug\" \"--framework=net3.5\" --x86 " +
                        "--dispose-runners --shadowcopy --agents=3 " +
                        "--process=InProcess --domain=Single", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Switch_For_Default_Labels()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.Labels = NUnit3Labels.Off;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Process_Switch_For_DefaultMultipleValue()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.Process = NUnit3ProcessOption.Multiple;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Switch_For_Default_AppDomainUsage()
            {
                // Given
                var fixture = new NUnit3RunnerFixture();
                fixture.Settings.AppDomainUsage = NUnit3AppDomainUsage.Default;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }
        }
    }
}