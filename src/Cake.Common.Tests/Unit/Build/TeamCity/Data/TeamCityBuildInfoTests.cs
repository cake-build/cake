// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TeamCity.Data
{
    public sealed class TeamCityBuildInfoTests
    {
        public sealed class TheBuildConfNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildConfName;

                // Then
                Assert.Equal("Cake Build", result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TeamCityInfoFixture().CreateBuildInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal("10-Foo", result);
            }
        }
    }
}