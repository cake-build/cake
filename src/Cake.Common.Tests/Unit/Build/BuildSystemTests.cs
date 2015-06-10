using Cake.Common.Build;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.TeamCity;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build
{
    public sealed class BuildSystemTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_AppVeyor_Is_Null()
            {
                // Given
                var teamCityProvider = Substitute.For<ITeamCityProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(null, teamCityProvider));

                // Then
                Assert.IsArgumentNullException(result, "appVeyorProvider");
            }

            [Fact]
            public void Should_Throw_If_TeamCity_Is_Null()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();

                // When
                var result = Record.Exception(() => new BuildSystem(appVeyorProvider, null));

                // Then
                Assert.IsArgumentNullException(result, "teamCityProvider");
            }
        }

        public sealed class TheIsRunningOnAppVeyorProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AppVeyor()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider);

                // When
                var result = buildSystem.IsRunningOnAppVeyor;

                // Then
                Assert.True(result);
            }
        }
        public sealed class TheIsRunningOnTeamCityProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_TeamCity()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider);

                // When
                var result = buildSystem.IsRunningOnTeamCity;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheIsLocalBuildProperty
        {
            [Fact]
            public void Should_Return_False_If_Running_On_AppVeyor()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                appVeyorProvider.IsRunningOnAppVeyor.Returns(true);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_False_If_Running_On_TeamCity()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(true);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Return_True_If_Not_Running_On_Any()
            {
                // Given
                var appVeyorProvider = Substitute.For<IAppVeyorProvider>();
                var teamCityProvider = Substitute.For<ITeamCityProvider>();
                appVeyorProvider.IsRunningOnAppVeyor.Returns(false);
                teamCityProvider.IsRunningOnTeamCity.Returns(false);
                var buildSystem = new BuildSystem(appVeyorProvider, teamCityProvider);

                // When
                var result = buildSystem.IsLocalBuild;

                // Then
                Assert.True(result);
            }
        }
    }
}
