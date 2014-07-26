using System;
using System.Diagnostics;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.NUnit;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit.Tools.NUnit
{
    public sealed class NUnitRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                var result = Record.Exception(() => runner.Run(null, new NUnitSettings()));

                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("assemblyPath", ((ArgumentNullException)result).ParamName);
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
                var fixture = new NUnitRunnerFixture(toolPath: expected);
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings
                {
                    ToolPath = toolPath
                });

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
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
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == "/Working/tools/nunit-console.exe"));
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1.dll", new NUnitSettings());

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "\"/Working/Test1.dll\""));
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
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.WorkingDirectory == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new NUnitRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
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
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "\"/Working/Test1.dll\" \"/noshadow\""));
            }
        }
    }
}
