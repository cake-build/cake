using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SonarQube
{
    public sealed class SonarQubeRunnerTests
    {
        [Fact]
        public void Should_Throw_If_Solution_Is_Null()
        {
            // Given
            var fixture = new SonarQubeRunnerFixture();
            fixture.SolutionPath = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsArgumentNullException(result, "solution");
        }

        [Fact]
        public void Should_Throw_If_Settings_Is_Null()
        {
            // Given
            var fixture = new SonarQubeRunnerFixture();
            fixture.Settings = null;

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsArgumentNullException(result, "settings");
        }

        [Fact]
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new SonarQubeRunnerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("SonarQube: Process returned an error (exit code 1).", result?.Message);
        }

        [Fact]
        public void Check_3_processes_are_launched()
        {
            // Given
            var fixture = new SonarQubeRunnerFixture();

            // When
            fixture.Run();

            // Then
            Assert.Equal(3, fixture.ProcessRunner.Results.Count);
            Assert.Equal("MSBuild.SonarQube.Runner.exe", fixture.ProcessRunner.Results[0].Path.GetFilename().ToString());
            Assert.Equal("MSBuild.exe", fixture.ProcessRunner.Results[1].Path.GetFilename().ToString());
            Assert.Equal("MSBuild.SonarQube.Runner.exe", fixture.ProcessRunner.Results[2].Path.GetFilename().ToString());
        }
    }
}
