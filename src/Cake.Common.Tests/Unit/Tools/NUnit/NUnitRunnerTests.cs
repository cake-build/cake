using System;
using System.Collections.Generic;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tests.Fixtures.Tools;
using Cake.Common.Tools.NUnit;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnitRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run((FilePath) null, new NUnitSettings()));

                // Then
                Assert.IsArgumentNullException(result, "assemblyPath");
            }

            [Fact]
            public void Should_Throw_If_NUnit_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new NUnitRunnerFixture(defaultToolExist: false);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new NUnitSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit: Could not locate executable.", result.Message);
            }

            [Theory]
            [InlineData("C:/NUnit/nunit.exe", "C:/NUnit/nunit.exe")]
            [InlineData("./tools/NUnit/nunit.exe", "/Working/tools/NUnit/nunit.exe")]
            public void Should_Use_NUnit_Runner_From_Tool_Path_If_Provided(string toolPath, string expected)
            {
                // Given
                var fixture = new NUnitRunnerFixture(expected);
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == expected),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Find_NUnit_Runner_If_Tool_Path_Not_Provided()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "/Working/tools/nunit-console.exe"),
                    Arg.Any<ProcessSettings>());
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Path_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(
                    p => p.Arguments.Render() == "\"/Working/Test1.dll\""));
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run(new List<FilePath>{"./Test1.dll", "./Test2.dll"}, new NUnitSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(
                    p => p.Arguments.Render() == "\"/Working/Test1.dll\" \"/Working/Test2.dll\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(
                    p => p.WorkingDirectory.FullPath == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new NUnitSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit: Process was not started.", result.Message);
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1.dll", new NUnitSettings()));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("NUnit: Process returned an error.", result.Message);
            }

            [Fact]
            public void Should_Not_Use_Shadow_Copying_If_Disabled_In_Settings()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings
                {
                    ShadowCopy = false
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/Test1.dll\" /noshadow"));
            }

            [Fact]
            public void Should_Not_Allow_NoResults_And_ResultsFile()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => 
                    runner.Run("./Test1.dll", new NUnitSettings
                    {
                        ResultsFile = "NewResults.xml",
                        NoResults = true
                    }));

                // Then
                Assert.IsType<ArgumentException>(result);
                Assert.Equal("NUnit: You can't specify both a results file and set NoResults to true.", result.Message);
            }

            [Fact]
            public void Should_Set_Result_Switch()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings
                {
                    ResultsFile = "NewTestResult.xml"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(), 
                    Arg.Is<ProcessSettings>(p => 
                        p.Arguments.Render() == "\"/Working/Test1.dll\" \"/result:/Working/NewTestResult.xml\""));
            }

            [Fact]
            public void Should_Set_Commandline_Switches()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings
                {
                    ResultsFile = "NewTestResult.xml",
                    NoLogo = true,
                    NoThread = true,
                    StopOnError = true,
                    Trace = "Debug",
                    Timeout = 5,
                    Include = "Database",
                    Exclude = "Database_Users",
                    Framework = "net1_1",
                    OutputFile = "stdout.txt",
                    ErrorOutputFile = "stderr.txt"
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(p =>
                        p.Arguments.Render() ==
                        "\"/Working/Test1.dll\" " +
                        "\"/framework:net1_1\" " +
                        "\"/include:Database\" \"/exclude:Database_Users\" " +
                        "/timeout:5 /nologo /nothread /stoponerror /trace:Debug " +
                        "\"/output:/Working/stdout.txt\" " +
                        "\"/err:/Working/stderr.txt\" " +
                        "\"/result:/Working/NewTestResult.xml\""));
            }
        }
    }
}
