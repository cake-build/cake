// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.XUnit
{
    public sealed class XUnitRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.AssemblyPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPath");
            }

            [Fact]
            public void Should_Throw_If_XUnit_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v1): Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/xUnit/xunit.exe", "/bin/tools/xUnit/xunit.exe")]
            [InlineData("./tools/xUnit/xunit.exe", "/Working/tools/xUnit/xunit.exe")]
            public void Should_Use_XUnit_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/xUnit/xunit.exe", "C:/xUnit/xunit.exe")]
            public void Should_Use_XUnit_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_XUnit_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new XUnitRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/xunit.console.clr4.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new XUnitRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new XUnitRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v1): Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v1): Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_HtmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.HtmlReport = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate HTML report when no output directory has been set.", result?.Message);
            }

            [Fact]
            public void Should_Generate_Html_Report_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/html\" \"/Output/Test1.dll.html\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_XmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.XmlReport = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate XML report when no output directory has been set.", result?.Message);
            }

            [Fact]
            public void Should_Generate_Xml_Report_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/xml\" \"/Output/Test1.dll.xml\"", result.Args);
            }

            [Fact]
            public void Should_Not_Use_Shadow_Copying_If_Disabled_In_Settings()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.ShadowCopy = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/noshadow\"", result.Args);
            }

            [Fact]
            public void Should_Set_Silent_Mode_If_Enabled_In_Settings()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Settings.Silent = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" /silent", result.Args);
            }
        }
    }
}