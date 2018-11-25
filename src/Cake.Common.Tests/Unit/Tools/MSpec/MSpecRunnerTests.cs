// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.MSpec
{
    public sealed class MSpecRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Assemblies = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_MSpec_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MSpec: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/MSpec/mspec.exe", "/bin/MSpec/mspec.exe")]
            [InlineData("./tools/MSpec/mspec.exe", "/Working/tools/MSpec/mspec.exe")]
            public void Should_Use_MSpec_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/MSpec/mspec.exe", "C:/MSpec/mspec.exe")]
            public void Should_Use_MSpec_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_MSpec_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new MSpecRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/mspec-clr4.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new MSpecRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
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
                var fixture = new MSpecRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MSpec: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MSpec: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_HtmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.HtmlReport = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate HTML report when no output directory has been set.", result?.Message);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Single_Assembly()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--html \"/Output/Test1.dll.html\" \"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Single_Assembly_ReportName()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;
                fixture.Settings.ReportName = "MSpecUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--html \"/Output/MSpecUnitReport.html\" \"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Multiple_Assemblies()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Assemblies.Clear();
                fixture.Assemblies.AddRange(new FilePath[] { "./Test1.dll", "./Test2.dll" });
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--html \"/Output/TestResults.html\" " +
                    "\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Multiple_Assemblies_ReportName()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Assemblies.Clear();
                fixture.Assemblies.AddRange(new FilePath[] { "./Test1.dll", "./Test2.dll" });
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;
                fixture.Settings.ReportName = "MSpecUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--html \"/Output/MSpecUnitReport.html\" " +
                             "\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_XmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.XmlReport = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate XML report when no output directory has been set.", result?.Message);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Single_Assembly()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--xml \"/Output/Test1.dll.xml\" \"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Single_Assembly_ReportName()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;
                fixture.Settings.ReportName = "MSpecUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--xml \"/Output/MSpecUnitReport.xml\" \"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Multiple_Assemblies()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Assemblies.Clear();
                fixture.Assemblies.AddRange(new FilePath[] { "./Test1.dll", "./Test2.dll" });
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--xml \"/Output/TestResults.xml\" " +
                    "\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Multiple_Assemblies_ReportName()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Assemblies.Clear();
                fixture.Assemblies.AddRange(new FilePath[] { "./Test1.dll", "./Test2.dll" });
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;
                fixture.Settings.ReportName = "MSpecUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("--xml \"/Output/MSpecUnitReport.xml\" " +
                             "\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Set_Commandline_Switches()
            {
                // Given
                var fixture = new MSpecRunnerFixture();
                fixture.Settings.Filters = "Filters.txt";
                fixture.Settings.Include = "foo,bar,foo_bar";
                fixture.Settings.Exclude = "foo,bar,foo_bar";
                fixture.Settings.TimeInfo = true;
                fixture.Settings.Silent = true;
                fixture.Settings.Progress = true;
                fixture.Settings.NoColor = true;
                fixture.Settings.Wait = true;
                fixture.Settings.TeamCity = true;
                fixture.Settings.NoTeamCity = true;
                fixture.Settings.AppVeyor = true;
                fixture.Settings.NoAppVeyor = true;
                fixture.Settings.HtmlReport = true;
                fixture.Settings.XmlReport = true;
                fixture.Settings.ReportName = "UnitTests";
                fixture.Settings.OutputDirectory = "Reports";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-f \"/Working/Filters.txt\" " +
                    "-i \"foo,bar,foo_bar\" " +
                    "-x \"foo,bar,foo_bar\" " +
                    "-t " +
                    "-s " +
                    "-p " +
                    "-c " +
                    "-w " +
                    "--teamcity " +
                    "--no-teamcity-autodetect " +
                    "--appveyor " +
                    "--no-appveyor-autodetect " +
                    "--html \"/Working/Reports/UnitTests.html\" " +
                    "--xml \"/Working/Reports/UnitTests.xml\" " +
                    "\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_Mspec_X86_Flag_Is_Not_Set_To_True()
            {
                // Given
                var fixture = new MSpecRunnerFixture("mspec-x86-clr4.exe");
                fixture.Settings.UseX86 = false;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MSpec: Could not locate executable.", result?.Message);
            }

            [Fact]
            public void Should_Find_MSpec_X86_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new MSpecRunnerFixture("mspec-x86-clr4.exe");
                fixture.Settings.UseX86 = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/mspec-x86-clr4.exe", result.Path.FullPath);
            }
        }
    }
}
