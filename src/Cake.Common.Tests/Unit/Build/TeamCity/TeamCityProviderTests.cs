using System;
using System.IO;
using Cake.Common.Build.TeamCity;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core.IO;
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
                var fixture = new TeamCityFixture();
                fixture.IsRunningOnTeamCity();
                var teamCity = fixture.CreateTeamCityService();

                // When
                var result = teamCity.IsRunningOnTeamCity;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_TeamCity()
            {
                // Given
                var fixture = new TeamCityFixture();
                var teamCity = fixture.CreateTeamCityService();

                // When
                var result = teamCity.IsRunningOnTeamCity;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheImportDotCoverCoverageMethod : IDisposable
        {
            private StringWriter _console;

            public TheImportDotCoverCoverageMethod()
            {
                _console = new StringWriter();
                Console.SetOut(_console);
            }

            public void Dispose()
            {
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                _console.Dispose();
            }

            [Fact]
            public void Should_Use_Bundled_DotCover_If_ToolPath_Is_Null()
            {
                // Given
                var fixture = new TeamCityFixture();
                fixture.IsRunningOnTeamCity();
                var teamCity = fixture.CreateTeamCityService();
                var snapshot = new FilePath("/path/to/result.dcvr");

                // When
                teamCity.ImportDotCoverCoverage(snapshot);

                // Then
                Assert.Equal("##teamcity[dotNetCoverage ]" + Environment.NewLine +
                    "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='/path/to/result.dcvr']" + Environment.NewLine,
                    _console.ToString());
            }

            [Fact]
            public void Should_Use_Provided_DotCover_If_ToolPath_Is_Not_Null()
            {
                // Given
                var fixture = new TeamCityFixture();
                fixture.IsRunningOnTeamCity();
                var teamCity = fixture.CreateTeamCityService();
                var snapshot = new FilePath("/path/to/result.dcvr");
                var dotCoverHome = new DirectoryPath("/path/to/dotcover_home");

                // When
                teamCity.ImportDotCoverCoverage(snapshot, dotCoverHome);

                // Then
                Assert.Equal("##teamcity[dotNetCoverage dotcover_home='/path/to/dotcover_home']" + Environment.NewLine +
                    "##teamcity[importData type='dotNetCoverage' tool='dotcover' path='/path/to/result.dcvr']" + Environment.NewLine,
                    _console.ToString());
            }
        }
    }
}