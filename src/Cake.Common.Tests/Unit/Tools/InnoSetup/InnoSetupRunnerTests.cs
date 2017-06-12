// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.InnoSetup;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.InnoSetup
{
    public class InnoSetupRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Script_File_Is_Null()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.ScriptPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "scriptFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_InnoSetup_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InnoSetup: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/inno/iscc.exe", "/bin/inno/iscc.exe")]
            [InlineData("./tools/inno/iscc.exe", "/Working/tools/inno/iscc.exe")]
            public void Should_Use_InnoSetup_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/inno/iscc.exe", "C:/inno/iscc.exe")]
            public void Should_Use_InnoSetup_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_InnoSetup_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/iscc.exe", result.Path.FullPath);
            }

            [WindowsFact]
            public void Should_Find_InnoSetup_Runner_In_Installation_Path_If_Tool_Path_Not_Provided_x86()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.GivenDefaultToolDoNotExist();
                fixture.GivenToolIsInstalledX86();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(fixture.InstalledToolPath.FullPath, result.Path.FullPath);
            }

            [WindowsFact]
            public void Should_Find_InnoSetup_Runner_In_Installation_Path_If_Tool_Path_Not_Provided_x64()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.GivenDefaultToolDoNotExist();
                fixture.GivenToolIsInstalledX64();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(fixture.InstalledToolPath.FullPath, result.Path.FullPath);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new InnoSetupFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InnoSetup: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("InnoSetup: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.Defines = new Dictionary<string, string>();
                fixture.Settings.Defines.Add("Foo", "Bar");
                fixture.Settings.Defines.Add("Test", null);
                fixture.Settings.Defines.Add("Test2", string.Empty);
                fixture.Settings.Defines.Add("Test3", "hello world");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/DFoo=\"Bar\" /DTest /DTest2 /DTest3=\"hello world\" \"/Working/Test.iss\"", result.Args);
            }

            [Fact]
            public void Should_Add_OutputOn_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.EnableOutput = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/O+ \"/Working/Test.iss\"", result.Args);
            }

            [Fact]
            public void Should_Add_OutputOff_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.EnableOutput = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/O- \"/Working/Test.iss\"", result.Args);
            }

            [Fact]
            public void Should_Add_OutputDir_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.OutputDirectory = "SetupOutput";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/O\"/Working/SetupOutput\" \"/Working/Test.iss\"", result.Args);
            }

            [Fact]
            public void Should_Add_OutputBaseFilename_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.OutputBaseFilename = "Test-setup";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/F\"Test-setup\" \"/Working/Test.iss\"", result.Args);
            }

            [Fact]
            public void Should_Add_QuietMode_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.QuietMode = InnoSetupQuietMode.Quiet;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Q \"/Working/Test.iss\"", result.Args);
            }

            [Fact]
            public void Should_Add_QuietModeWithProgress_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new InnoSetupFixture();
                fixture.Settings.QuietMode = InnoSetupQuietMode.QuietWithProgress;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Qp \"/Working/Test.iss\"", result.Args);
            }
        }
    }
}