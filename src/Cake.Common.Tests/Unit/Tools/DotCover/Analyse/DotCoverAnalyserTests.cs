// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotCover.Analyse;
using Cake.Common.Tools.DotCover;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotCover.Analyse
{
    public sealed class DotCoverAnalyserTests
    {
        public sealed class TheAnalyseMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Context = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "context");
            }

            [Fact]
            public void Should_Throw_If_Action_Is_Null()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Action = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "action");
            }

            [Fact]
            public void Should_Throw_If_Output_File_Is_Null()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.OutputPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputPath");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_No_Tool_Was_Intercepted()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Action = context => { };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, "No tool was started.");
            }

            [Fact]
            public void Should_Capture_Tool_And_Arguments_From_Action()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\"", result.Args);
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Not_Capture_Arguments_From_Action_If_Excluded(string arguments)
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Action = context =>
                {
                    context.ProcessRunner.Start(
                        new FilePath("/Working/tools/Test.exe"),
                        new ProcessSettings()
                        {
                            Arguments = arguments
                        });
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/Output=\"/Working/result.xml\"", result.Args);
            }

            [Theory]
            [InlineData(DotCoverReportType.DetailedXML, "DetailedXML")]
            [InlineData(DotCoverReportType.HTML, "HTML")]
            [InlineData(DotCoverReportType.JSON, "JSON")]
            [InlineData(DotCoverReportType.NDependXML, "NDependXML")]
            public void Should_Append_ReportType(DotCoverReportType reportType, string reportTypeString)
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.ReportType = reportType;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/ReportType=" + reportTypeString, result.Args);
            }

            [Fact]
            public void Should_Append_TargetWorkingDir()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.TargetWorkingDir = new DirectoryPath("/Working");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/TargetWorkingDir=\"/Working\"", result.Args);
            }

            [Fact]
            public void Should_Append_Scope()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.WithScope("/Working/*.dll")
                    .WithScope("/Some/**/Other/*.dll");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/Scope=\"/Working/*.dll;/Some/**/Other/*.dll\"", result.Args);
            }

            [Fact]
            public void Should_Append_Filters()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.WithFilter("+:module=Test.*")
                    .WithFilter("-:myassembly");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/Filters=\"+:module=Test.*;-:myassembly\"", result.Args);
            }

            [Fact]
            public void Should_Append_AttributeFilters()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.WithAttributeFilter("filter1")
                    .WithAttributeFilter("filter2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/AttributeFilters=\"filter1;filter2\"", result.Args);
            }

            [Fact]
            public void Should_Append_DisableDefaultFilters()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.DisableDefaultFilters = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/DisableDefaultFilters", result.Args);
            }

            [Fact]
            public void Should_Append_LogFile()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.Settings.LogFile = "./logfile.log";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.xml\" " +
                             "/LogFile=\"/Working/logfile.log\"", result.Args);
            }

            [Fact]
            public void Should_Capture_XUnit()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.FileSystem.CreateFile("/Working/tools/xunit.console.exe");
                fixture.Action = context =>
                {
                    context.XUnit2(
                        new FilePath[] { "./Test.dll" },
                        new XUnit2Settings { ShadowCopy = false });
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/xunit.console.exe\" " +
                             "/TargetArguments=\"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "/Output=\"/Working/result.xml\"", result.Args);
            }

            [Fact]
            public void Should_Capture_NUnit()
            {
                // Given
                var fixture = new DotCoverAnalyserFixture();
                fixture.FileSystem.CreateFile("/Working/tools/nunit-console.exe");
                fixture.Action = context =>
                {
                    context.NUnit(
                        new FilePath[] { "./Test.dll" },
                        new NUnitSettings { ShadowCopy = false });
                };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("Analyse /TargetExecutable=\"/Working/tools/nunit-console.exe\" " +
                             "/TargetArguments=\"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "/Output=\"/Working/result.xml\"", result.Args);
            }
        }
    }
}