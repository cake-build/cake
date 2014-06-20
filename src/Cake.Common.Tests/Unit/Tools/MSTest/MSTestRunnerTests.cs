using System;
using System.Diagnostics;
using Cake.Common.Tests.Fixtures;
using Cake.Common.Tools.MSTest;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Common.Tests.Unit.Tools.MSTest
{
    public sealed class MSTestRunnerTests
    {
        [Fact]
        public void Should_Throw_If_Assembly_Path_Is_Null()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            var runner = fixture.CreateRunner();

            // When
            var result = Record.Exception(() => runner.Run(null, new MSTestSettings()));

            // Then
            Assert.IsType<ArgumentNullException>(result);
            Assert.Equal("assemblyPath", ((ArgumentNullException)result).ParamName);
        }

        [Fact]
        public void Should_Throw_If_Settings_Are_Null()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            var runner = fixture.CreateRunner();

            // When
            var result = Record.Exception(() => runner.Run("Test1.dll", null));

            // Then
            Assert.IsType<ArgumentNullException>(result);
            Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
        }

        [Fact]
        public void Should_Throw_If_Tool_Path_Was_Not_Found()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.ToolPath = null;
            var runner = fixture.CreateRunner();

            // When
            var result = Record.Exception(() => runner.Run("Test1.dll", new MSTestSettings()));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("Could not find MSTest.exe.", result.Message);
        }

        [Theory]
        [InlineData("C:/xUnit/xunit.exe", "C:/xUnit/xunit.exe")]
        [InlineData("./tools/xUnit/xunit.exe", "/Working/tools/xUnit/xunit.exe")]
        public void Should_Use_MSTest_From_Tool_Path_If_Provided(string toolPath, string expected)
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            var runner = fixture.CreateRunner();

            // When
            runner.Run("./Test1.dll", new MSTestSettings
            {
                ToolPath = toolPath
            });

            // Then
            fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                p => p.FileName == expected));
        }

        [Theory]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 12.0/Common7/IDE/mstest.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 11.0/Common7/IDE/mstest.exe")]
        [InlineData("/ProgramFilesX86/Microsoft Visual Studio 10.0/Common7/IDE/mstest.exe")]
        public void Should_Use_Available_Tool_Path(string existingToolPath)
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.ToolPath = new FilePath(existingToolPath); 
            var runner = fixture.CreateRunner();

            // When
            runner.Run("Test1.dll", new MSTestSettings());

            // Then
            fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                p => p.FileName == existingToolPath));
        }

        [Fact]
        public void Should_Set_Working_Directory()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            var runner = fixture.CreateRunner();

            // When
            runner.Run("./Test1.dll", new MSTestSettings());

            // Then
            fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                p => p.WorkingDirectory == "/Working"));
        }

        [Fact]
        public void Should_Throw_If_Process_Was_Not_Started()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
            var runner = fixture.CreateRunner();

            // When
            var result = Record.Exception(() => runner.Run("./Test1.dll", new MSTestSettings()));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("MSTest process was not started.", result.Message);
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            fixture.Process.GetExitCode().Returns(1);
            var runner = fixture.CreateRunner();

            // When
            var result = Record.Exception(() => runner.Run("./Test1.dll", new MSTestSettings()));

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("MSTest process returned failure.", result.Message);
        }

        [Fact]
        public void Should_Not_Use_Isolation_By_Default()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            var runner = fixture.CreateRunner();

            // When
            runner.Run("./Test1.dll", new MSTestSettings());

            // Then
            fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                p => p.Arguments == "\"/testcontainer:/Working/Test1.dll\" \"/noisolation\""));
        }

        [Fact]
        public void Should_Use_Isolation_If_Disabled_In_Settings()
        {
            // Given
            var fixture = new MSTestRunnerFixture();
            var runner = fixture.CreateRunner();

            // When
            runner.Run("./Test1.dll", new MSTestSettings
            {
                NoIsolation = false
            });

            // Then
            fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                p => p.Arguments == "\"/testcontainer:/Working/Test1.dll\""));
        }
    }
}
