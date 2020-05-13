// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins.Data
{
    public sealed class JenkinsChangeInfoTests
    {
        public sealed class TheChangeIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Id;

                // Then
                Assert.Equal("42178", result);
            }
        }

        public sealed class TheChangeUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Url;

                // Then
                Assert.Equal("http://changeurl", result);
            }
        }

        public sealed class TheChangeTitleProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Title;

                // Then
                Assert.Equal("Modified x", result);
            }
        }

        public sealed class TheChangeAuthorProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Author;

                // Then
                Assert.Equal("cu", result);
            }
        }

        public sealed class TheChangeAuthorDisplayNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.AuthorDisplayName;

                // Then
                Assert.Equal("Cake User", result);
            }
        }

        public sealed class TheChangeAuthorEmailroperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.AuthorEmail;

                // Then
                Assert.Equal("cake@cakebuild.net", result);
            }
        }

        public sealed class TheChangeTargetProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Target;

                // Then
                Assert.Equal("develop", result);
            }
        }

        public sealed class TheChangeBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Branch;

                // Then
                Assert.Equal("feature/feature1", result);
            }
        }

        public sealed class TheChangeForkProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.Fork;

                // Then
                Assert.Equal("fork1", result);
            }
        }

        public sealed class TheIsPullRequestProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Change.IsPullRequest;

                // Then
                Assert.Equal(true, result);
            }
        }
    }
}