// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NSIS
{
    // ReSharper disable once InconsistentNaming
    public sealed class MakeNSISRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Script_File_Is_Null()
            {
                // Given
                var fixture = new NSISFixture();
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
                var fixture = new NSISFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_MakeNSIS_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MakeNSIS: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/nsis/makensis.exe", "/bin/nsis/makensis.exe")]
            [InlineData("./tools/nsis/makensis.exe", "/Working/tools/nsis/makensis.exe")]
            public void Should_Use_MakeNSIS_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/nsis/makensis.exe", "C:/nsis/makensis.exe")]
            public void Should_Use_MakeNSIS_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_MakeNSIS_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NSISFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/makensis.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new NSISFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MakeNSIS: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MakeNSIS: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Settings.Defines = new Dictionary<string, string>();
                fixture.Settings.Defines.Add("Foo", "Bar");
                fixture.Settings.Defines.Add("Test", null);
                fixture.Settings.Defines.Add("Test2", string.Empty);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/DFoo=Bar /DTest /DTest2 \"/Working/Test.nsi\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoChangeDirectory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Settings.NoChangeDirectory = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/NOCD \"/Working/Test.nsi\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoConfig_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new NSISFixture();
                fixture.Settings.NoConfig = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/NOCONFIG \"/Working/Test.nsi\"", result.Args);
            }
        }
    }
}