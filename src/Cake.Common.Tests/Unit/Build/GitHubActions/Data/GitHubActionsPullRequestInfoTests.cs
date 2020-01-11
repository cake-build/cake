// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Data
{
    public sealed class GitHubActionsPullRequestInfoTests
    {
        public sealed class TheIsPullRequestProperty
        {
            [Theory]
            [InlineData("pull_request", true)]
            [InlineData("push", false)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new GitHubActionsInfoFixture();
                fixture.Environment.GetEnvironmentVariable("GITHUB_EVENT_NAME").Returns(value);
                var info = fixture.CreatePullRequestInfo();

                // When
                var result = info.IsPullRequest;

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
