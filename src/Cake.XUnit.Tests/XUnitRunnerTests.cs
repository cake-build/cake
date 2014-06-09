using System;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.XUnit.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.XUnit.Tests
{
    public sealed class XUnitRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Fact]
            public void Should_Throw_If_Assembly_Path_Is_Null()
            {
                var fixture = new XUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                var result = Record.Exception(() => runner.Run(null));

                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("assemblyPath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_XUnit_Runner_Was_Not_Found()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Globber.Match("./tools/**/xunit.console.clr4.exe").Returns(Enumerable.Empty<Path>());
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Could not find xUnit runner.", result.Message);
            }

            [Fact]
            public void Should_Use_Provided_Assembly_Paths_In_Process_Arguments()
            {
                // Given
                var fixture = new XUnitRunnerFixture();                                
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1");

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "\"Test1\""));
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                var runner = fixture.CreateRunner();

                // When
                runner.Run("./Test1");

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.WorkingDirectory == "/Working"));                
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("xUnit process was not started.", result.Message);     
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new XUnitRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run("./Test1"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Failing xUnit tests.", result.Message);                  
            }
        }
    }
}
