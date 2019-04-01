// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.BitbucketPipelines.Data
{
    public sealed class BitbucketPipelinesPullRequestInfoTests
    {
        public sealed class TheIsPullRequestProperty
        {
            [Theory]
            [InlineData("1", true)]
            [InlineData("0", false)]
            public void Should_Return_Correct_Value(string value, bool expected)
            {
                // Given
                var fixture = new BitbucketPipelinesInfoFixture();
                fixture.Environment.GetEnvironmentVariable("BITBUCKET_PR_ID").Returns(value);
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
                var info = new BitbucketPipelinesInfoFixture().CreatePullRequestInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(1, result);
            }
        }
    }
}