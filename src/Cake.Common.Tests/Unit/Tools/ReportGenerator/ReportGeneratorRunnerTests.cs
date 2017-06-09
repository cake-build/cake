// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.ReportGenerator;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.ReportGenerator
{
    public sealed class ReportGeneratorRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Reports_Are_Null()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Reports = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "reports");
            }

            [Fact]
            public void Should_Throw_If_Reports_Are_Empty()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Reports = new FilePath[0];

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentException(result, "reports", "reports must not be empty");
            }

            [Fact]
            public void Should_Throw_If_TargetDir_Is_Null()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.TargetDir = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "targetDir");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Find_Report_Generator()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/ReportGenerator.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Tool_Path_If_Present()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.ToolPath = "/some/where/else/ReportGenerator.exe";
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/some/where/else/ReportGenerator.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReportGenerator: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReportGenerator: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Set_Report_And_Target_Directory()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\"", result.Args);
            }

            [Fact]
            public void Should_Set_Reports_And_Target_Directory()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Reports = new FilePath[] { "report1.xml", "report2.xml" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report1.xml;/Working/report2.xml\" \"-targetdir:/Working/output\"", result.Args);
            }

            [Fact]
            public void Should_Set_Report_Types()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.ReportTypes = new[] { ReportGeneratorReportType.Html, ReportGeneratorReportType.Latex };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\" \"-reporttypes:Html;Latex\"", result.Args);
            }

            [Fact]
            public void Should_Set_Source_Directories()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.SourceDirectories = new[] { DirectoryPath.FromString("source1"), DirectoryPath.FromString("/source2") };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\" \"-sourcedirs:/Working/source1;/source2\"", result.Args);
            }

            [Fact]
            public void Should_Set_History_Directory()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.HistoryDirectory = "history";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\" \"-historydir:/Working/history\"", result.Args);
            }

            [Fact]
            public void Should_Set_Assembly_Filters()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.AssemblyFilters = new[] { "+Included", "-Excluded.*" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\" \"-assemblyfilters:+Included;-Excluded.*\"", result.Args);
            }

            [Fact]
            public void Should_Set_Class_Filters()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.ClassFilters = new[] { "+Included", "-Excluded.*" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\" \"-classfilters:+Included;-Excluded.*\"", result.Args);
            }

            [Fact]
            public void Should_Set_Verbosity()
            {
                // Given
                var fixture = new ReportGeneratorRunnerFixture();
                fixture.Settings.Verbosity = ReportGeneratorVerbosity.Info;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"-reports:/Working/report.xml\" \"-targetdir:/Working/output\" \"-verbosity:Info\"", result.Args);
            }
        }
    }
}