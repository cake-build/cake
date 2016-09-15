// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.BitbucketPipelines.Data
{
    public sealed class BitbucketPipelinesRepositoryInfoTests
    {
        public sealed class TheCommitProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitbucketPipelinesInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Commit;

                // Then
                Assert.Equal("4efbc1ffb993dfbcf024e6a9202865cc0b6d9c50", result);
            }
        }

        public sealed class TheRepoSlugProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitbucketPipelinesInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoSlug;

                // Then
                Assert.Equal("cake", result);
            }
        }

        public sealed class TheRepoOwnerProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitbucketPipelinesInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoOwner;

                // Then
                Assert.Equal("cakebuild", result);
            }
        }

        public sealed class TheBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitbucketPipelinesInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Branch;

                // Then
                Assert.Equal("develop", result);
            }
        }

        public sealed class TheTagProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitbucketPipelinesInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Tag;

                // Then
                Assert.Equal("BitbucketPiplines", result);
            }
        }
    }
}