// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotCover.Report;
using Cake.Common.Tools.DotCover;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotCover.Report
{
    public sealed class DotCoverReporterTests
    {
        public sealed class TheReportMethod
        {
            [Fact]
            public void Should_Throw_If_Source_File_Is_Null()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.SourceFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "sourceFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            #region New Parameter Syntax

            [Fact]
            public void Should_Ignore_Output_File_If_Null()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Not_Ignore_Output_File_With_New_Syntax()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = "myoutputfile.xml";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\" " +
                             "--xml-report-output \"/Working/myoutputfile.xml\"", result.Args);
            }

            [Fact]
            public void Should_Not_Ignore_Output_File_With_New_Syntax_Json()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = "myoutputfile.json";
                fixture.Settings.ReportType = DotCoverReportType.JSON;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\" " +
                             "--json-report-output \"/Working/myoutputfile.json\"", result.Args);
            }


            [Fact]
            public void Should_Append_JsonReportOutput()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;
                fixture.Settings.JsonReportOutput = new FilePath("/Working/coverage.json");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\" " +
                             "--json-report-output \"/Working/coverage.json\"", result.Args);
            }

            [Theory]
            [InlineData(DotCoverReportScope.None, "none")]
            [InlineData(DotCoverReportScope.Assembly, "assembly")]
            [InlineData(DotCoverReportScope.Type, "type")]
            [InlineData(DotCoverReportScope.Method, "method")]
            [InlineData(DotCoverReportScope.Statement, "statement")]
            public void Should_Append_JsonReportScope(DotCoverReportScope reportScope, string reportScopeString)
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;
                fixture.Settings.JsonReportOutput = new FilePath("/Working/coverage.json");
                fixture.Settings.JsonReportCoveringTestsScope = reportScope;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\" " +
                             "--json-report-output \"/Working/coverage.json\" " +
                             "--json-report-covering-tests-scope \"" + reportScopeString + "\"", result.Args);
            }

            [Fact]
            public void Should_Append_XmlReportOutput()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;
                fixture.Settings.XmlReportOutput = new FilePath("/Working/coverage.json");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\" " +
                             "--xml-report-output \"/Working/coverage.json\"", result.Args);
            }

            [Theory]
            [InlineData(DotCoverReportScope.None, "none")]
            [InlineData(DotCoverReportScope.Assembly, "assembly")]
            [InlineData(DotCoverReportScope.Type, "type")]
            [InlineData(DotCoverReportScope.Method, "method")]
            [InlineData(DotCoverReportScope.Statement, "statement")]
            public void Should_Append_XmlReportScope(DotCoverReportScope reportScope, string reportScopeString)
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;
                fixture.Settings.XmlReportOutput = new FilePath("/Working/coverage.json");
                fixture.Settings.XmlReportCoveringTestsScope = reportScope;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "--snapshot-source \"/Working/result.dcvr\" " +
                             "--xml-report-output \"/Working/coverage.json\" " +
                             "--xml-report-covering-tests-scope \"" + reportScopeString + "\"", result.Args);
            }

            #endregion

            #region Legacy Parameter Syntax

            [Fact]
            public void Should_Throw_If_Output_File_Is_Null()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputFile");
            }

            [Theory]
            [InlineData(DotCoverReportType.DetailedXML, "DetailedXML")]
            [InlineData(DotCoverReportType.HTML, "HTML")]
            [InlineData(DotCoverReportType.JSON, "JSON")]
            [InlineData(DotCoverReportType.NDependXML, "NDependXML")]
            public void Should_Append_ReportType(DotCoverReportType reportType, string reportTypeString)
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.Settings.ReportType = reportType;
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "/Source=\"/Working/result.dcvr\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/ReportType=" + reportTypeString, result.Args);
            }

            [Fact]
            public void Should_Append_LogFile()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.Settings.LogFile = "./logfile.log";
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report " +
                             "/Source=\"/Working/result.dcvr\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/LogFile=\"/Working/logfile.log\"", result.Args);
            }

            [Fact]
            public void Should_Append_ConfigurationFile()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.Settings.WithConfigFile(new FilePath("./config.xml"));
                fixture.Settings.UseLegacySyntax = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("report \"/Working/config.xml\" " +
                             "/Source=\"/Working/result.dcvr\" " +
                             "/Output=\"/Working/result.xml\"", result.Args);
            }

            #endregion
        }
    }
}