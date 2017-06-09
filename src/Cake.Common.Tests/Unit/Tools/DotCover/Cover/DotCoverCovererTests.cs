// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotCover.Cover;
using Cake.Common.Tools.DotCover;
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\" " +
                             "/TargetWorkingDir=\"/Working\"", result.Args);
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\" " +
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\" " +
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\" " +
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/Test.exe\" " +
                             "/TargetArguments=\"-argument\" " +
                             "/Output=\"/Working/result.dcvr\" " +
                             "/DisableDefaultFilters", result.Args);
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/xunit.console.exe\" " +
                             "/TargetArguments=\"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
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
                Assert.Equal("Cover /TargetExecutable=\"/Working/tools/nunit-console.exe\" " +
                             "/TargetArguments=\"\\\"/Working/Test.dll\\\" -noshadow\" " +
                             "/Output=\"/Working/result.dcvr\"", result.Args);
            }
        }
    }
}