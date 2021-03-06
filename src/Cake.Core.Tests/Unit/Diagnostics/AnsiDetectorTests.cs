using Cake.Core.Diagnostics;
using Cake.Testing;
using Xunit;

namespace Cake.Core.Tests.Unit.Diagnostics
{
    public sealed class AnsiDetectorTests
    {
        public sealed class The_SupportsAnsi_Method
        {
            public sealed class UsingTeamCity
            {
                [Theory]
                [InlineData("2020.2")]
                [InlineData("2020.1.5")]
                [InlineData("2017.1")]
                [InlineData("10.0.5")]
                [InlineData("10.0")]
                [InlineData("9.1.7")]
                public void Should_Return_True_When_Running_TeamCity_9_1_or_2017_Or_Higher(string teamCityVersion)
                {
                    // Given
                    var fakeEnvironment = FakeEnvironment.CreateUnixEnvironment();

                    // When
                    fakeEnvironment.SetEnvironmentVariable("TEAMCITY_VERSION", teamCityVersion);

                    // Then
                    Assert.True(AnsiDetector.SupportsAnsi(fakeEnvironment));
                }

                [Theory]
                [InlineData("9.0.5")]
                [InlineData("9.0.1")]
                [InlineData("8.1.5")]
                [InlineData("7.1.5")]
                [InlineData("7.0.1")]
                [InlineData("6.0")]
                public void Should_Return_False_When_Running_TeamCity_9_0_Or_Lower(string teamCityVersion)
                {
                    // Given
                    var fakeEnvironment = FakeEnvironment.CreateUnixEnvironment();

                    // When
                    fakeEnvironment.SetEnvironmentVariable("TEAMCITY_VERSION", teamCityVersion);

                    // Then
                    Assert.False(AnsiDetector.SupportsAnsi(fakeEnvironment));
                }
            }
        }
    }
}
