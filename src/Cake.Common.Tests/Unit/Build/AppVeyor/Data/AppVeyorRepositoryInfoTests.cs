// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor.Data
{
    public sealed class AppVeyorRepositoryInfoTests
    {
        public sealed class TheProviderProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Provider;

                // Then
                Assert.Equal("github", result);
            }
        }

        public sealed class TheScmProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Scm;

                // Then
                Assert.Equal("git", result);
            }
        }

        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("cake-build/cake", result);
            }
        }

        public sealed class TheBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateRepositoryInfo();

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
                var info = new AppVeyorInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Tag;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheCommitProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new AppVeyorInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Commit;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
