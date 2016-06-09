// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.Jenkins.Data
{
    public sealed class JenkinsRepositoryInfoTests
    {
        public sealed class TheGitBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.GitBranch;

                // Then
                Assert.Equal("CAKE-BRANCH", result);
            }
        }

        public sealed class TheGitCommitShaProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.GitCommitSha;

                // Then
                Assert.Equal("67d423d36dd15b191a53ab3ddb613dc4b95be8b3", result);
            }
        }

        public sealed class TheSvnRevisionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.SvnRevision;

                // Then
                Assert.Equal("REVISION-NUMBER", result);
            }
        }

        public sealed class TheSvnUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.SvnUrl;

                // Then
                Assert.Equal("svn://127.0.0.1/cake-build", result);
            }
        }

        public sealed class TheCvsBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new JenkinsInfoFixture().CreateRepositoryInfo();

                // When
                var result = info.CvsBranch;

                // Then
                Assert.Equal("DEVBRANCH", result);
            }
        }
    }
}
