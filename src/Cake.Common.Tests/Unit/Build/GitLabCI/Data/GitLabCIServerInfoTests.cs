// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI.Data
{
    public sealed class GitLabCIServerInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateServerInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("GitLab", result);
            }
        }
    }

    public sealed class TheRevisionProperty
    {
        [Fact]
        public void Should_Return_Correct_Value()
        {
            // Given
            var info = new GitLabCIInfoFixture().CreateServerInfo();

            // When
            var result = info.Revision;

            // Then
            Assert.Equal("70606bf", result);
        }
    }

    public sealed class TheVersionProperty
    {
        [Fact]
        public void Should_Return_Correct_Value()
        {
            // Given
            var info = new GitLabCIInfoFixture().CreateServerInfo();

            // When
            var result = info.Version;

            // Then
            Assert.Equal("8.9.0", result);
        }
    }
}
