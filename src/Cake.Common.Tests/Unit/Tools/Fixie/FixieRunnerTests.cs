using System.Collections.Generic;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.Fixie;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Fixie
{
    public sealed class FixieRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run((FilePath) null, new FixieSettings()));

                // Then
                Assert.IsArgumentNullException(result, "assemblyPath");
            }

            [Fact]
            public void Should_Throw_If_Fixie_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new FixieRunnerFixture(defaultToolExist: false);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new FixieSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Fixie: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/Fixie/Fixie.Console.exe", "C:/Fixie/Fixie.Console.exe")]
            [InlineData("./tools/Fixie/Fixie.Console.exe", "/Working/tools/Fixie/Fixie.Console.exe")]
            public void Should_Use_Fixie_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new FixieRunnerFixture(expected);
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new FixieSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_Fixie_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new FixieSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/Fixie.Console.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new FixieSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/Test1.dll\""));
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new List<FilePath> { "./Test1.dll", "./Test2.dll" }, new FixieSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == "\"/Working/Test1.dll\" \"/Working/Test2.dll\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new FixieSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new FixieSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Fixie: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new FixieSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Fixie: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_set_NUnitXml_Output_File()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./blarg.dll", new FixieSettings
                {
                    NUnitXml = "nunit-style-results.xml",
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "\"/Working/blarg.dll\" --NUnitXml \"/Working/nunit-style-results.xml\""));
            }

            [Fact]
            public void Should_set_xUnitXml_Output_File()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./blarg.dll", new FixieSettings
                {
                    XUnitXml = "xunit-results.xml",
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() == "\"/Working/blarg.dll\" --xUnitXml \"/Working/xunit-results.xml\""));
            }

            [Theory]
            [InlineData(TeamCityOutput.On, "on")]
            [InlineData(TeamCityOutput.Off, "off")]
            public void Should_Set_TeamCity_value(TeamCityOutput? teamCityOutput, string teamCityValue)
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Tests.dll", new FixieSettings
                {
                    TeamCity = teamCityOutput,
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == string.Format("\"/Working/Tests.dll\" --TeamCity {0}", teamCityValue)));
            }

            [Fact]
            public void Should_Set_custom_options()
            {
                // Given
                var fixture = new FixieRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Tests.dll", new FixieSettings()
                    .WithOption("--include", "CategoryA")
                    .WithOption("--include", "CategoryB"));

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(
                        p => p.Arguments.Render() == string.Format("\"/Working/Tests.dll\" --include CategoryA --include CategoryB")));
            }
        }
    }
}