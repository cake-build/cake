using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.Cake;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Path = Cake.Core.IO.Path;

namespace Cake.Common.Tests.Unit.Tools.Cake
{
    public sealed class CakeRunnerTests
    {
        public sealed class TheExecuteScriptMethod
        {
            [Fact]
            public void Should_Throw_If_Script_Path_Was_Null()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.ScriptPath = null;

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsArgumentNullException(result, "scriptPath");
            }

            [Fact]
            public void Should_Throw_If_Script_Is_Missing()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.ScriptPath = "/Working/missing.cake";

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<FileNotFoundException>(result);
                Assert.Equal("Cake script file not found.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Cake_Executable_Was_Not_Found()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Globber.Match("./tools/**/Cake.exe").Returns(Enumerable.Empty<Path>());

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Cake: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/Cake/Cake.exe", "C:/Cake/Cake.exe")]
            [InlineData("./tools/Cake/Cake.exe", "/Working/tools/Cake/Cake.exe")]
            public void Should_Use_Cake_Executable_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new CakeRunnerFixture(expected);
                fixture.Settings.ToolPath = toolPath;               

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Add_Provided_Script_To_Process_Arguments()
            {
                // Given
                var fixture = new CakeRunnerFixture();

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/build.cake\""));
            }

            [Fact]
            public void Should_Add_Provided_Verbosity_To_Process_Arguments()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Settings = new CakeSettings{ Verbosity = Verbosity.Diagnostic };

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/build.cake\" -verbosity=Diagnostic"));
            }

            [Fact]
            public void Should_Add_Provided_Arguments_To_Process_Arguments()
            {
                // Given
                var fixture = new CakeRunnerFixture();
                fixture.Settings = new CakeSettings
                {
                    Arguments = new Dictionary<string, string>
                    {
                        { "target", "Build" },
                        { "configuration", "Debug" }
                    }
                };

                // When
                fixture.Run();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/build.cake\" -target=\"Build\" -configuration=\"Debug\""));
            }
        }
    }
}
