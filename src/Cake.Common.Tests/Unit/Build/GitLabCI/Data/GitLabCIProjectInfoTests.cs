// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI.Data
{
    public sealed class GitLabCIProjectInfoTests
    {
        public sealed class TheDirectoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Directory;

                // Then
                Assert.Equal("/builds/gitlab-org/gitlab-ce", result);
            }
        }

        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(34, result);
            }
        }

        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("gitlab-ce", result);
            }
        }

        public sealed class TheNamespaceProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Namespace;

                // Then
                Assert.Equal("gitlab-org", result);
            }
        }

        public sealed class ThePathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Path;

                // Then
                Assert.Equal("gitlab-org/gitlab-ce", result);
            }
        }

        public sealed class TheUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateProjectInfo();

                // When
                var result = info.Url;

                // Then
                Assert.Equal("https://gitlab.com/gitlab-org/gitlab-ce", result);
            }
        }
    }
}
