// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Rwx.Data
{
    public sealed class RwxGitInfoTests
    {
        public sealed class TheRepositoryUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.RepositoryUrl;

                // Then
                Assert.Equal("https://github.com/cake-build/cake.git", result);
            }
        }

        public sealed class TheRepositoryNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.RepositoryName;

                // Then
                Assert.Equal("cake-build/cake", result);
            }
        }

        public sealed class TheCommitShaProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.CommitSha;

                // Then
                Assert.Equal("0123456789abcdef0123456789abcdef01234567", result);
            }
        }

        public sealed class TheCommitMessageProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.CommitMessage;

                // Then
                Assert.Equal("Add RWX build provider\n\nMore details here.", result);
            }
        }

        public sealed class TheCommitSummaryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.CommitSummary;

                // Then
                Assert.Equal("Add RWX build provider", result);
            }
        }

        public sealed class TheCommitterNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.CommitterName;

                // Then
                Assert.Equal("Octo Cat", result);
            }
        }

        public sealed class TheCommitterEmailProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.CommitterEmail;

                // Then
                Assert.Equal("octocat@example.com", result);
            }
        }

        public sealed class TheRefProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.Ref;

                // Then
                Assert.Equal("refs/heads/main", result);
            }
        }

        public sealed class TheRefNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new RwxInfoFixture().CreateGitInfo();

                // When
                var result = info.RefName;

                // Then
                Assert.Equal("main", result);
            }
        }
    }
}
