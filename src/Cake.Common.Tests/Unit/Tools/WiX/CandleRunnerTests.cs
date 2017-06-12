// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.WiX;
using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.WiX
{
    public sealed class CandleRunnerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Source_Files_Is_Null()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.SourceFiles = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "sourceFiles");
            }

            [Fact]
            public void Should_Throw_If_Source_Files_Is_Empty()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.SourceFiles = new List<FilePath>();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("sourceFiles", ((ArgumentException)result)?.ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Candle_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Candle: Could not locate executable.", result?.Message);
            }

            [Theory]
            [InlineData("/bin/tools/WiX/candle.exe", "/bin/tools/WiX/candle.exe")]
            [InlineData("./tools/WiX/candle.exe", "/Working/tools/WiX/candle.exe")]
            public void Should_Use_Candle_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [WindowsTheory]
            [InlineData("C:/WiX/candle.exe", "C:/WiX/candle.exe")]
            public void Should_Use_Candle_Runner_From_Tool_Path_If_Provided_On_Windows(string toolPath, string expected)
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.ToolPath = toolPath;
                fixture.GivenSettingsToolPathExist();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Path.FullPath);
            }

            [Fact]
            public void Should_Find_Candle_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new CandleFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working/tools/candle.exe", result.Path.FullPath);
            }

            [Fact]
            public void Should_Use_Provided_Source_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.SourceFiles.Clear();
                fixture.SourceFiles.Add("./Test.wxs");
                fixture.SourceFiles.Add("./Test2.wxs");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"/Working/Test.wxs\" \"/Working/Test2.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new CandleFixture();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("/Working", result.Process.WorkingDirectory.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Candle: Process was not started.", result?.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Candle: Process returned an error (exit code 1).", result?.Message);
            }

            [Theory]
            [InlineData(Architecture.IA64, "-arch ia64 \"/Working/Test.wxs\"")]
            [InlineData(Architecture.X64, "-arch x64 \"/Working/Test.wxs\"")]
            [InlineData(Architecture.X86, "-arch x86 \"/Working/Test.wxs\"")]
            public void Should_Add_Architecture_To_Arguments_If_Provided(Architecture arch, string expected)
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.Architecture = arch;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal(expected, result.Args);
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.Defines = new Dictionary<string, string>();
                fixture.Settings.Defines.Add("Foo", "Foo Bar");

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-dFoo=\"Foo Bar\" \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Extensions_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.Extensions = new[] { "WixUIExtension" };

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-ext WixUIExtension \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_FIPS_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.FIPS = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-fips \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_NoLogo_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.NoLogo = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-nologo \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.OutputDirectory = "obj";

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-o \"/Working/obj\\\\\" \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Pedantic_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.Pedantic = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-pedantic \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Show_Source_Trace_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.ShowSourceTrace = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-trace \"/Working/Test.wxs\"", result.Args);
            }

            [Fact]
            public void Should_Add_Verbose_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new CandleFixture();
                fixture.Settings.Verbose = true;

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("-v \"/Working/Test.wxs\"", result.Args);
            }
        }
    }
}