// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotCover.Report;
using Cake.Common.Tools.DotCover;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing;
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
            public void Should_Throw_If_Output_File_Is_Null()
            {
                // Given
                var fixture = new DotCoverReporterFixture();
                fixture.OutputFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputFile");
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

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Report " +
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

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Report " +
                             "/Source=\"/Working/result.dcvr\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/LogFile=\"/Working/logfile.log\"", result.Args);
            }
        }
    }
}