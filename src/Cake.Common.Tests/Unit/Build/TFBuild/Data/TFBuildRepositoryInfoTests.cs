// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TFBuild.Data;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.TFBuild.Data
{
    public sealed class TFBuildRepositoryInfoTests
    {
        public sealed class TheSourceBranchNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.SourceBranchName;

                // Then
                Assert.Equal("develop", result);
            }
        }

        public sealed class TheSourceVersionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.SourceVersion;

                // Then
                Assert.Equal("4efbc1ffb993dfbcf024e6a9202865cc0b6d9c50", result);
            }
        }

        public sealed class TheShelvesetProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.Shelveset;

                // Then
                Assert.Equal("Shelveset1", result);
            }
        }

        public sealed class TheRepoNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new TFBuildInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.RepoName;

                // Then
                Assert.Equal("cake", result);
            }
        }

        public sealed class TheProviderProperty
        {
            [Theory]
            [InlineData("Git", TFRepositoryType.Git)]
            [InlineData("GitHub", TFRepositoryType.GitHub)]
            [InlineData("Svn", TFRepositoryType.Svn)]
            [InlineData("TfsGit", TFRepositoryType.TfsGit)]
            [InlineData("TfsVersionControl", TFRepositoryType.TfsVersionControl)]
            public void Should_Return_Correct_Value(string type, TFRepositoryType provider)
            {
                // Given
                var info = new TFBuildInfoFixture().CreateRepositoryInfo(type);

                // When
                var result = info.Provider;

                // Then
                Assert.Equal(provider, result);
            }
        }
    }
}
