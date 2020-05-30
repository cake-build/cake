// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AzurePipelines.Data
{
    public sealed class AzurePipelinesPullRequestInfoTests
    {
        public sealed class TheIsPullRequestProperty
        {
            [Theory]
            [InlineData("1", true)]
            [InlineData("0", false)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new AzurePipelinesInfoFixture();
                fixture.Environment.GetEnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID").Returns(value);
                var info = fixture.CreatePullRequestInfo();

                // When
                var result = info.IsPullRequest;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(1, result);
            }
        }

        public sealed class TheNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.Number;

                // Then
                Assert.Equal(1, result);
            }
        }

        public sealed class ThePullRequestIsForkProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.IsFork;

                // Then
                Assert.Equal(false, result);
            }
        }

        public sealed class ThePullRequestSourceBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.SourceBranch;

                // Then
                Assert.Equal(@"refs/heads/FeatureBranch", result);
            }
        }

        public sealed class ThePullRequestSourceRepositoryUriProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.SourceRepositoryUri;

                // Then
                Assert.Equal(new Uri(@"https://fabrikamfiber.visualstudio.com/Project/_git/ProjectRepo"), result);
            }
        }

        public sealed class ThePullRequestTargetBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AzurePipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.TargetBranch;

                // Then
                Assert.Equal(@"refs/heads/master", result);
            }
        }
    }
}
