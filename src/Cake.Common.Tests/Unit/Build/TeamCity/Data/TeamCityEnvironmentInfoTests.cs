// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TeamCity.Data
{
    public sealed class TeamCityEnvironmentInfoTests
    {
        public sealed class TheProjectProperty
        {
            [Fact]
            public void Should_Not_Be_Null()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Project;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheBuildProperty
        {
            [Fact]
            public void Should_Not_Be_Null()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Build;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class ThePullRequestProperty
        {
            [Fact]
            public void Should_Not_Be_Null()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.PullRequest;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}