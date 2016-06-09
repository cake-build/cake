// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TravicCI.Data
{
    public sealed class TravisRepositoryInfoTests
    {
        public sealed class TheCommitProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Commit;

                // Then
                Assert.Equal("6cbdbe8", result);
            }
        }

        public sealed class TheCommitRangeProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.CommitRange;

                // Then
                Assert.Equal("6cb4d6...5ba6dbe8", result);
            }
        }

        public sealed class ThePullRequestProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.PullRequest;

                // Then
                Assert.Equal("#786 (GH742) Added TravisCI build system support", result);
            }
        }

        public sealed class TheSlugProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TravisCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Slug;

                // Then
                Assert.Equal("4d65ba6", result);
            }
        }
    }
}
