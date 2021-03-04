// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TeamCity.Data
{
    public sealed class TeamCityPullRequestInfoTests
    {
        public sealed class TheIsPullRequestProperty
        {
            [Theory]
            [InlineData("refs/pull-requests/1/merge", true)]
            [InlineData("refs/merge-requests/1/head", true)]
            [InlineData("refs/pull/1/head", true)]
            [InlineData("refs/pull/1/merge", true)]
            [InlineData("refs/changes/1/head", true)]
            [InlineData("refs/heads/master", false)]
            [InlineData("", true)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                fixture.SetGitBranch(value);
                fixture.SetBuildPropertiesContent(Properties.Resources.TeamCity_Build_Properties_Xml);
                fixture.SetConfigPropertiesContent(Properties.Resources.TeamCity_Config_Properties_Xml);
                var info = fixture.CreatePullRequestInfo();

                // When
                var result = info.IsPullRequest;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Theory]
            [InlineData("refs/pull-requests/1/merge", 1)]
            [InlineData("refs/pull/2/merge", 2)]
            [InlineData("refs/changes/3/merge", 3)]
            [InlineData("refs/merge-requests/4/merge", 4)]
            [InlineData("refs/heads/master", null)]
            [InlineData("", 5)]
            public void Should_Return_Correct_Value(string value, int? expected)
            {
                // Given
                var fixture = new TeamCityInfoFixture();
                fixture.SetGitBranch(value);
                fixture.SetBuildPropertiesContent(Properties.Resources.TeamCity_Build_Properties_Xml);
                fixture.SetConfigPropertiesContent(Properties.Resources.TeamCity_Config_Properties_Xml);
                var info = fixture.CreatePullRequestInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}