// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.TeamCity;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core.IO;
using Cake.Testing.Extensions;
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
                var result = Record.Exception(() => new TeamCityProvider(null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new TeamCityFixture();

                // When
                var result = Record.Exception(() => new TeamCityProvider(fixture.Environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "log");
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

        public sealed class TheImportDotCoverCoverageMethod
        {
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
                    fixture.Log.AggregateLogMessages());
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
                    fixture.Log.AggregateLogMessages());
            }
        }

        public sealed class TheSetParameterMethod
        {
            [Fact]
            public void SetParameter_Should_Write_To_The_Log_Correctly()
            {
                // Given
                var fixture = new TeamCityFixture();
                var teamCity = fixture.CreateTeamCityService();

                // When
                teamCity.SetParameter("internal.artifactVersion", "1.2.3.4");

                // Then
                Assert.Equal("##teamcity[setParameter name='internal.artifactVersion' value='1.2.3.4']" + Environment.NewLine,
                    fixture.Log.AggregateLogMessages());
            }
        }
    }
}