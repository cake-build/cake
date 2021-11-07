// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Data
{
    public sealed class GitHubActionsEnvironmentInfoTests
    {
        public sealed class TheHomeProperty
        {
            [Fact]
            public void Should_Return_Correct_Values()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Home.FullPath;

                // Then
                Assert.Equal("/home/runner", result);
            }
        }

        public sealed class TheWorkflowProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Workflow;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class ThePullRequestProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.PullRequest;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
