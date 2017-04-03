// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.ReportUnit;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.ReportUnit
{
    public sealed class ReportUnitRunnerTests
    {
        public sealed class TheRunMethodWithDirectories
        {
            [Fact]
            public void Should_Throw_If_InputFolder_Is_Null()
            {
                // Given
                var fixture = new ReportUnitDirectoryFixture();
                fixture.InputFolder = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "inputFolder");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new ReportUnitDirectoryFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Find_ReportUnit_Runner()
            {
                // Given
                var fixture = new ReportUnitDirectoryFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/ReportUnit.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ReportUnitDirectoryFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReportUnit: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ReportUnitDirectoryFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReportUnit: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Use_Provided_Directories_In_Process_Arguments()
            {
                // Given
                var fixture = new ReportUnitDirectoryFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/temp/input\" \"/temp/output\"", result.Args);
            }
        }

        public sealed class TheRunMethodWithFiles
        {
            [Fact]
            public void Should_Throw_If_InputFile_Is_Null()
            {
                // Given
                var fixture = new ReportUnitFileFixture();
                fixture.InputFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "inputFile");
            }

            [Fact]
            public void Should_Throw_If_OutputFile_Is_Null()
            {
                // Given
                var fixture = new ReportUnitFileFixture();
                fixture.OutputFile = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "outputFile");
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new ReportUnitFileFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Find_ReportUnit_Runner()
            {
                // Given
                var fixture = new ReportUnitFileFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/ReportUnit.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new ReportUnitFileFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReportUnit: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new ReportUnitFileFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("ReportUnit: Process returned an error (exit code 1).", result?.Message);
            }

            [Fact]
            public void Should_Use_Provided_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new ReportUnitFileFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/temp/input.xml\" \"/temp/output.html\"", result.Args);
            }
        }
    }
}