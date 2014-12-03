using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.WiX;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit.Tools.WiX
{
    public sealed class CandleRunnerTests
    {
        public sealed class TheContructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateCandleRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Globber_Is_Null()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.Globber = null;

                // When
                var result = Record.Exception(() => fixture.CreateCandleRunner());

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("globber", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Source_Files_Is_Null()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                var result = Record.Exception(() => runner.Run(null, new CandleSettings()));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("sourceFiles", ((ArgumentNullException) result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Source_Files_Is_Empty()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                var result = Record.Exception(() => runner.Run(new FilePath[0], new CandleSettings()));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("sourceFiles", ((ArgumentException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Settings_Is_Null()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] {new FilePath("/Working/File.lol")}, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Candle_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.Globber.Match("./tools/**/candle.exe").Returns(Enumerable.Empty<Path>());
                var runner = fixture.CreateCandleRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] {new FilePath("/Test.wxs")}, new CandleSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Candle: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/WiX/candle.exe", "C:/WiX/candle.exe")]
            [InlineData("./tools/WiX/candle.exe", "/Working/tools/WiX/candle.exe")]
            public void Should_Use_Candle_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new WiXFixture(expected);
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wxs")}, new CandleSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected), 
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_Candle_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wxs")}, new CandleSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/candle.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Source_Files_In_Process_Arguments()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wxs"), new FilePath("./Test2.wxs")}, new CandleSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/Test.wxs\" \"/Working/Test2.wxs\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateCandleRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Candle: Process was not started.", result.Message);   
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new WiXFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateCandleRunner();

                // When
                var result = Record.Exception(() => runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Candle: Process returned an error.", result.Message);
            }

            [Theory]
            [InlineData(Architecture.IA64, "-arch ia64")]
            [InlineData(Architecture.X64, "-arch x64")]
            [InlineData(Architecture.X86, "-arch x86")]
            public void Should_Add_Architecture_To_Arguments_If_Provided(Architecture arch, string expected)
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    Architecture = arch
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == string.Concat(expected, " \"/Working/Test.wxs\"")));
            }

            [Fact]
            public void Should_Add_Defines_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    Defines = new Dictionary<string, string>
                    {
                        { "Foo", "Bar" }
                    }
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-dFoo=Bar \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_Extensions_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] {new FilePath("./Test.wxs")}, new CandleSettings
                {
                    Extensions = new[] {"WixUIExtension"}
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-ext WixUIExtension \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_FIPS_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    FIPS = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-fips \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_NoLogo_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    NoLogo = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-nologo \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_Output_Directory_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    OutputDirectory =  "obj"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-o \"/Working/obj\\\\\" \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_Pedantic_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    Pedantic = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-pedantic \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_Show_Source_Trace_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    ShowSourceTrace = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-trace \"/Working/Test.wxs\""));
            }

            [Fact]
            public void Should_Add_Verbose_To_Arguments_If_Provided()
            {
                // Given
                var fixture = new WiXFixture();
                var runner = fixture.CreateCandleRunner();

                // When
                runner.Run(new[] { new FilePath("./Test.wxs") }, new CandleSettings
                {
                    Verbose = true
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "-v \"/Working/Test.wxs\""));
            }
        }
    }
}
