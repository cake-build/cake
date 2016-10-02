using Cake.Common.Tests.Fixtures.Tools;
using Cake.Core;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.SonarQube
{
    public sealed class SonarQubeRunnerTests
    {
        [Fact]
        public void Should_Throw_If_Solution_Path_Is_Null()
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
        public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
        {
            // Given
            var fixture = new SonarQubeRunnerFixture();
            fixture.GivenProcessExitsWithCode(1);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            Assert.IsType<CakeException>(result);
            Assert.Equal("MSTest: Process returned an error (exit code 1).", result?.Message);
        }
    }
}
