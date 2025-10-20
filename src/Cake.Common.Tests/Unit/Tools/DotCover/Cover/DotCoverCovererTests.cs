// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotCover.Cover;
using Cake.Common.Tools.DotCover;
using Cake.Common.Tools.DotCover.Cover;
using Cake.Common.Tools.NUnit;
using Cake.Common.Tools.XUnit;
using Cake.Core.IO;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotCover.Cover
{
    public sealed class DotCoverCovererTests
    {
        public sealed class TheCoverMethod
        {
            [Fact]
            public void Should_Throw_If_Context_Is_Null()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
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
                var fixture = new DotCoverCovererFixture();
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
                var fixture = new DotCoverCovererFixture();
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
                var fixture = new DotCoverCovererFixture();
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
                var fixture = new DotCoverCovererFixture();
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
                var fixture = new DotCoverCovererFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Not_Capture_Arguments_From_Action_If_Excluded(string arguments)
            {
                // Given
                var fixture = new DotCoverCovererFixture();
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
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Append_TargetWorkingDir()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.TargetWorkingDir = new DirectoryPath("/Working");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--target-working-directory \"/Working\"", result.Args);
            }

            [Fact]
            public void Should_Append_Scope()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithScope("/Working/*.dll")
                    .WithScope("/Some/**/Other/*.dll");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "/Scope=\"/Working/*.dll;/Some/**/Other/*.dll\"", result.Args);
            }

            [Fact]
            public void Should_Append_Filters()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithFilter("+:module=Test.*")
                    .WithFilter("-:myassembly");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "/Filters=\"+:module=Test.*;-:myassembly\"", result.Args);
            }

            [Fact]
            public void Should_Append_AttributeFilters()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithAttributeFilter("filter1")
                    .WithAttributeFilter("filter2");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "/AttributeFilters=\"filter1;filter2\"", result.Args);
            }

            [Fact]
            public void Should_Append_DisableDefaultFilters()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.DisableDefaultFilters = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "/DisableDefaultFilters", result.Args);
            }

            [Fact]
            public void Should_Append_ProcessFilters()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithProcessFilter("+:test.exe")
                    .WithProcessFilter("-:sqlservr.exe");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "/ProcessFilters=\"+:test.exe;-:sqlservr.exe\"", result.Args);
            }

            [Fact]
            public void Should_Capture_XUnit()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
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
                Assert.Equal("cover --target-executable \"/Working/tools/xunit.console.exe\" " +
                             "--target-arguments \"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Capture_NUnit()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
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
                Assert.Equal("cover --target-executable \"/Working/tools/nunit-console.exe\" " +
                             "--target-arguments \"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Append_ConfigurationFile()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithConfigFile(new FilePath("./config.xml"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover \"/Working/config.xml\" --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Append_ExcludeAssemblies()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithExcludeAssembly("*.Tests")
                    .WithExcludeAssembly("Test.*");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--exclude-assemblies \"*.Tests,Test.*\"", result.Args);
            }

            [Fact]
            public void Should_Append_ExcludeAttributes()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithExcludeAttribute("System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute")
                    .WithExcludeAttribute("Custom.*Attribute");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--exclude-attributes \"System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute,Custom.*Attribute\"", result.Args);
            }

            [Fact]
            public void Should_Append_ExcludeProcesses()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithExcludeProcess("test.exe")
                    .WithExcludeProcess("*.vshost.exe");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--exclude-processes \"test.exe,*.vshost.exe\"", result.Args);
            }

            [Fact]
            public void Should_Append_JsonReportOutput()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithJsonReportOutput(new FilePath("/Working/coverage.json"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--json-report-output \"/Working/coverage.json\"", result.Args);
            }

            [Fact]
            public void Should_Append_JsonReportCoveringTestsScope()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithJsonReportCoveringTestsScope(DotCoverReportScope.Method);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--json-report-covering-tests-scope \"method\"", result.Args);
            }

            [Fact]
            public void Should_Append_XmlReportOutput()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithXmlReportOutput(new FilePath("/Working/coverage.xml"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--xml-report-output \"/Working/coverage.xml\"", result.Args);
            }

            [Fact]
            public void Should_Append_XmlReportCoveringTestsScope()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithXmlReportCoveringTestsScope(DotCoverReportScope.Statement);

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--xml-report-covering-tests-scope \"statement\"", result.Args);
            }

            [Fact]
            public void Should_Append_TemporaryDirectory()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithTemporaryDirectory(new DirectoryPath("/Working/temp"));

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--temporary-directory \"/Working/temp\"", result.Args);
            }

            [Fact]
            public void Should_Append_UseApi()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithUseApi();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--use-api", result.Args);
            }

            [Fact]
            public void Should_Append_NoNGen()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithNoNGen();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\" " +
                             "--no-ngen", result.Args);
            }

            [Fact]
            public void Should_Use_Legacy_Syntax_When_Enabled()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithLegacySyntax();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Use_New_Syntax_By_Default()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                // Don't set UseLegacySyntax - should default to false

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("cover --target-executable \"/Working/tools/Test.exe\" " +
                             "--target-arguments \"-argument\" " +
                             "--snapshot-output \"/Working/result.dcvr\"", result.Args);
            }

            [Fact]
            public void Should_Not_Support_New_Features_In_Legacy_Mode()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithLegacySyntax()
                    .WithJsonReportOutput(new FilePath("/Working/report.json"))
                    .WithExcludeAssembly("*.Tests");

                // When
                var result = fixture.Run();

                // Then - New format features should not appear in legacy mode
                Assert.Equal("cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
                Assert.DoesNotContain("--json-report-output", result.Args);
                Assert.DoesNotContain("--exclude-assemblies", result.Args);
            }

            [Fact]
            public void Should_Support_New_Features_In_New_Mode()
            {
                // Given
                var fixture = new DotCoverCovererFixture();
                fixture.Settings.WithJsonReportOutput(new FilePath("/Working/report.json"))
                    .WithExcludeAssembly("*.Tests");

                // When
                var result = fixture.Run();

                // Then - New format features should appear
                Assert.Contains("--json-report-output \"/Working/report.json\"", result.Args);
                Assert.Contains("--exclude-assemblies \"*.Tests\"", result.Args);
                Assert.Contains("--snapshot-output", result.Args);
            }
        }
    }
}