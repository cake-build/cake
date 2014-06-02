using System.Diagnostics;
using Cake.Core;
using Cake.Core.IO;
using Cake.MSBuild.Tests.Fixtures;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.MSBuild.Tests
{
    public sealed class MSBuildRunnerTests
    {
        public sealed class TheRunMethod
        {
            [Theory]
            [InlineData(false, "/Program86/MSBuild/12.0/Bin/MSBuild.exe")]
            [InlineData(true, "/Program86/MSBuild/12.0/Bin/amd64/MSBuild.exe")]
            public void Should_Get_Correct_Path_To_MSBuild(bool is64BitOperativeSystem, string expected)
            {
                // Given
                var fixture = new MSBuildRunnerFixture(is64BitOperativeSystem);
                var runner = fixture.CreateRunner();
                
                // When
                runner.Run(fixture.Context, new MSBuildSettings("./src/Solution.sln"));

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.FileName == expected));
            }

            [Fact]
            public void Should_Use_Correct_Default_Target_In_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");

                // When
                runner.Run(fixture.Context, settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/target:Build src/Solution.sln"));
            }

            [Fact]
            public void Should_Append_Targets_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");
                settings.WithTarget("A");
                settings.WithTarget("B");

                // When
                runner.Run(fixture.Context, settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/target:A;B src/Solution.sln"));
            }

            [Fact]
            public void Should_Append_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");
                settings.WithProperty("A", "B");
                settings.WithProperty("C", "D");

                // When
                runner.Run(fixture.Context, settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/property:A=B;C=D /target:Build src/Solution.sln"));
            }

            [Fact]
            public void Should_Append_Configuration_As_Property_To_Process_Arguments()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();

                var settings = new MSBuildSettings("./src/Solution.sln");
                settings.SetConfiguration("Release");

                // When
                runner.Run(fixture.Context, settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.Arguments == "/property:Configuration=Release /target:Build src/Solution.sln"));          
            }

            [Fact]
            public void Should_Set_Working_Directory()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                var runner = fixture.CreateRunner();
                var settings = new MSBuildSettings("./src/Solution.sln");

                // When
                runner.Run(fixture.Context, settings);

                // Then
                fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(
                    p => p.WorkingDirectory == "/Working"));
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>()).Returns((IProcess)null);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(fixture.Context, new MSBuildSettings("./src/Solution.sln")));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("MSBuild process was not started.", result.Message);           
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new MSBuildRunnerFixture();
                fixture.Process.GetExitCode().Returns(1);
                var runner = fixture.CreateRunner();

                // When
                var result = Record.Exception(() => runner.Run(fixture.Context, new MSBuildSettings("./src/Solution.sln")));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Build failed.", result.Message);
            }
        }
    }
}
