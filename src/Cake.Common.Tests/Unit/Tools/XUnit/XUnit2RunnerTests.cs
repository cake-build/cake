// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.XUnit;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.XUnit
{
    public sealed class XUnit2RunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "assemblyPaths");
            }

            [Fact]
            public void Should_Throw_If_XUnit_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/xUnit/xunit.exe", "/bin/tools/xUnit/xunit.exe")]
            [InlineData("./tools/xUnit/xunit.exe", "/Working/tools/xUnit/xunit.exe")]
            public void Should_Use_XUnit_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
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
                var fixture = new XUnit2RunnerFixture();
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
                var fixture = new XUnit2RunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/xunit.console.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_XUnit_X86_Flag_Is_Not_Set_To_True()
            {
                // Given
                var fixture = new XUnit2RunnerFixture("xunit.console.x86.exe");
                fixture.Settings.UseX86 = false;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Could not locate executable.", result?.Message);
            }

            [Fact]
            public void Should_Find_XUnit_X86_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new XUnit2RunnerFixture("xunit.console.x86.exe");
                fixture.Settings.UseX86 = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/xunit.console.x86.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\"", result.Args);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit.net (v2): Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_HtmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
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
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -html \"/Output/Test1.dll.html\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Single_Assembly_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -html \"/Output/xUnitReport.html\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Multiple_Assemblies()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-html \"/Output/TestResults.html\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Html_Report_With_Correct_Name_For_Multiple_Assemblies_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.HtmlReport = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-html \"/Output/xUnitReport.html\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_XmlReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
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
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -xml \"/Output/Test1.dll.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Single_Assembly_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;
                fixture.Settings.ReportName = "xUnitReport";
                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -xml \"/Output/xUnitReport.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Multiple_Assemblies()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-xml \"/Output/TestResults.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Xml_Report_With_Correct_Name_For_Multiple_Assemblies_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReport = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-xml \"/Output/xUnitReport.xml\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_XmlReportV1_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.XmlReportV1 = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate XML report when no output directory has been set.", result?.Message);
            }

            [Fact]
            public void Should_Generate_Legacy_Xml_Report_With_Correct_Name_For_Single_Assembly()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReportV1 = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -xmlv1 \"/Output/Test1.dll.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Legacy_Xml_Report_With_Correct_Name_For_Single_Assembly_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReportV1 = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -xmlv1 \"/Output/xUnitReport.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Legacy_Xml_Report_With_Correct_Name_For_Multiple_Assemblies()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReportV1 = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-xmlv1 \"/Output/TestResults.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_Legacy_Xml_Report_With_Correct_Name_For_Multiple_Assemblies_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.XmlReportV1 = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-xmlv1 \"/Output/xUnitReport.xml\"", result.Args);
            }

            [Fact]
            public void Should_Throw_If_NUnitReport_Is_Set_But_OutputDirectory_Is_Null()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.NUnitReport = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cannot generate NUnit XML report when no output directory has been set.", result?.Message);
            }

            [Fact]
            public void Should_Generate_NUnit_Xml_Report_With_Correct_Name_For_Single_Assembly()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.NUnitReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -nunit \"/Output/Test1.dll.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_NUnit_Xml_Report_With_Correct_Name_For_Single_Assembly_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.NUnitReport = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -nunit \"/Output/xUnitReport.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_NUnit_Xml_Report_With_Correct_Name_For_Multiple_Assemblies()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.NUnitReport = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-nunit \"/Output/TestResults.xml\"", result.Args);
            }

            [Fact]
            public void Should_Generate_NUnit_Xml_Report_With_Correct_Name_For_Multiple_Assemblies_ReportName()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.AssemblyPaths = new FilePath[] { "./Test1.dll", "./Test2.dll" };
                fixture.Settings.OutputDirectory = "/Output";
                fixture.Settings.NUnitReport = true;
                fixture.Settings.ReportName = "xUnitReport";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" \"/Working/Test2.dll\" " +
                             "-nunit \"/Output/xUnitReport.xml\"", result.Args);
            }

            [Fact]
            public void Should_Not_Use_Shadow_Copying_If_Disabled_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.ShadowCopy = false;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -noshadow", result.Args);
            }

            [Fact]
            public void Should_Not_Use_App_Domains_If_Disabled_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.NoAppDomain = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -noappdomain", result.Args);
            }

            [Theory]
            [InlineData(ParallelismOption.All, "\"/Working/Test1.dll\" -parallel all")]
            [InlineData(ParallelismOption.Assemblies, "\"/Working/Test1.dll\" -parallel assemblies")]
            [InlineData(ParallelismOption.Collections, "\"/Working/Test1.dll\" -parallel collections")]
            [InlineData(ParallelismOption.None, "\"/Working/Test1.dll\"")]
            public void Should_Use_Parallel_Switch_If_Settings_Value_Is_Not_None(ParallelismOption option, string expected)
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.Parallelism = option;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Theory]
            [InlineData(null, "\"/Working/Test1.dll\"")]
            [InlineData(0, "\"/Working/Test1.dll\" -maxthreads unlimited")]
            [InlineData(3, "\"/Working/Test1.dll\" -maxthreads 3")]
            public void Should_Use_MaxThreads_Switch_If_Settings_Value_Is_Not_Null(int? option, string expected)
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.MaxThreads = option;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Set_Switches_For_TraitsToInclude_Defined_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.IncludeTrait("Trait1", "value1A", "value1B");
                fixture.Settings.IncludeTrait("Trait2", "value2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -trait \"Trait1=value1A\" -trait \"Trait1=value1B\" -trait \"Trait2=value2\"", result.Args);
            }

            [Fact]
            public void Should_Set_Switches_For_TraitsToExclude_Defined_In_Settings()
            {
                // Given
                var fixture = new XUnit2RunnerFixture();
                fixture.Settings.ExcludeTrait("Trait1", "value1A", "value1B");
                fixture.Settings.ExcludeTrait("Trait2", "value2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test1.dll\" -notrait \"Trait1=value1A\" -notrait \"Trait1=value1B\" -notrait \"Trait2=value2\"", result.Args);
            }
        }
    }
}