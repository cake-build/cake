// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Bitrise.Data
{
    public sealed class BitriseRepositoryInfoTests
    {
        public sealed class TheGitRepositoryUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.GitRepositoryUrl;

                //Then
                Assert.Equal("git@github.com:/cake-build/cake.git", result);
            }
        }

        public sealed class TheGitBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.GitBranch;

                //Then
                Assert.Equal("cake-branch", result);
            }
        }

        public sealed class TheGitTagProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.GitTag;

                //Then
                Assert.Equal("v0.0.1", result);
            }
        }

        public sealed class TheGitCommitProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.GitCommit;

                //Then
                Assert.Equal("63dd7b", result);
            }
        }

        public sealed class ThePullRequestProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new BitriseInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.PullRequest;

                //Then
                Assert.Equal("[WIP] Bitrise cake support #000", result);
            }
        }
    }
}
