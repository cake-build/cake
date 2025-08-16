// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI.Data
{
    public sealed class WoodpeckerCIRepositoryInfoTests
    {
        public sealed class TheRepoProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Repo;

                // Then
                Assert.Equal("john-doe/my-repo", result);
            }
        }

        public sealed class TheRepoOwnerProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoOwner;

                // Then
                Assert.Equal("john-doe", result);
            }
        }

        public sealed class TheRepoNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoName;

                // Then
                Assert.Equal("my-repo", result);
            }
        }

        public sealed class TheRepoUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Uri_For_Valid_Url()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoUrl;

                // Then
                Assert.NotNull(result);
                Assert.Equal("https://git.example.com/john-doe/my-repo", result.ToString());
            }

            [Fact]
            public void Should_Return_Null_For_Invalid_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_REPO_URL", "not-a-valid-url");
                var info = fixture.CreateRepositoryInfo();

                // When
                var result = info.RepoUrl;

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheRepoCloneUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Uri_For_Valid_Url()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoCloneUrl;

                // Then
                Assert.NotNull(result);
                Assert.Equal("https://git.example.com/john-doe/my-repo.git", result.ToString());
            }

            [Fact]
            public void Should_Return_Null_For_Invalid_Url()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_REPO_CLONE_URL", "not-a-valid-url");
                var info = fixture.CreateRepositoryInfo();

                // When
                var result = info.RepoCloneUrl;

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheRepoCloneSshUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Uri_For_Valid_Url()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoCloneSshUrl;

                // Then
                Assert.NotNull(result);
                Assert.Equal("git@git.example.com:john-doe/my-repo.git", result.ToString());
            }
        }

        public sealed class TheRepoPrivateProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoPrivate;

                // Then
                Assert.True(result);
            }
        }
    }
}
