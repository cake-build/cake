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
    public sealed class NUnitRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Assemblies = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_NUnit_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/NUnit/nunit.exe", "/bin/tools/NUnit/nunit.exe")]
            [InlineData("./tools/NUnit/nunit.exe", "/Working/tools/NUnit/nunit.exe")]
            public void Should_Use_NUnit_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/NUnit/nunit.exe", "C:/NUnit/nunit.exe")]
            public void Should_Use_NUnit_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_NUnit_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NUnitRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/nunit-console.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_X86NUnit_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NUnitRunnerFixture(@"nunit-console-x86.exe");
                fixture.Settings.X86 = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/nunit-console-x86.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnitRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
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
                var fixture = new NUnitRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit: Process was not started.", result?.Message);
            }

            [Theory]
            [InlineData(10, "NUnit: 10 test(s) failed (exit code 10).")]
            [InlineData(-1, "NUnit: Invalid argument (exit code -1).")]
            [InlineData(-2, "NUnit: File not found (exit code -2).")]
            [InlineData(-3, "NUnit: Test fixture not found (exit code -3).")]
            [InlineData(-100, "NUnit: Unexpected error (exit code -100).")]
            [InlineData(-10, "NUnit: Unrecognised error (exit code -10).")]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code(int exitCode, string expectedMessage)
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.GivenProcessExitsWithCode(exitCode);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal(expectedMessage, result?.Message);
            }

            [Fact]
            public void Should_Not_Use_Shadow_Copying_If_Disabled_In_Settings()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.ShadowCopy = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -noshadow", result.Args);
            }

            [Fact]
            public void Should_Not_Allow_NoResults_And_ResultsFile()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.ResultsFile = "NewResults.xml";
                fixture.Settings.NoResults = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("NUnit: You can't specify both a results file and set NoResults to true.", result?.Message);
            }

            [Fact]
            public void Should_Set_Result_Switch()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.ResultsFile = "NewTestResult.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" " +
                             "\"-result:/Working/NewTestResult.xml\"",
                             result.Args);
            }

            [Fact]
            public void Should_Set_Commandline_Switches()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.ResultsFile = "NewTestResult.xml";
                fixture.Settings.NoLogo = true;
                fixture.Settings.NoThread = true;
                fixture.Settings.StopOnError = true;
                fixture.Settings.Trace = "Debug";
                fixture.Settings.Timeout = 5;
                fixture.Settings.Include = "Database";
                fixture.Settings.Exclude = "Database_Users";
                fixture.Settings.Framework = "net1_1";
                fixture.Settings.OutputFile = "stdout.txt";
                fixture.Settings.ErrorOutputFile = "stderr.txt";
                fixture.Settings.Process = NUnitProcessOption.Multiple;
                fixture.Settings.UseSingleThreadedApartment = true;
                fixture.Settings.AppDomainUsage = NUnitAppDomainUsage.Single;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"-framework:net1_1\" " +
                        "\"-include:Database\" \"-exclude:Database_Users\" " +
                        "-timeout:5 -nologo -nothread -stoponerror " +
                        "-trace:Debug \"-output:/Working/stdout.txt\" " +
                        "\"-err:/Working/stderr.txt\" " +
                        "\"-result:/Working/NewTestResult.xml\" " +
                        "\"-process:Multiple\" \"-apartment:STA\" " +
                        "\"-domain:Single\"", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Process_Switch_For_DefaultSingleValue()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.Process = NUnitProcessOption.Single;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Not_Set_Switch_For_Default_AppDomainUsage()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Settings.AppDomainUsage = NUnitAppDomainUsage.Default;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }
        }
    }
}