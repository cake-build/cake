using Cake.Common.Build.TeamCity;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TeamCity
{
    public sealed class TeamCityProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new TeamCityProvider(null));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnTeamCityProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_TeamCity()
            {
                // Given
                var teamCity = Substitute.For<ITeamCityProvider>();
                teamCity.IsRunningOnTeamCity.Returns(true);

                // When
                var result = teamCity.IsRunningOnTeamCity;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_TeamCity()
            {
                // Given
                var teamCity = Substitute.For<ITeamCityProvider>();

                // When
                var result = teamCity.IsRunningOnTeamCity;

                // Then
                Assert.False(result);
            }
        }
    }
}
