// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI.Data
{
    public sealed class GitLabCIRunnerInfoTests
    {
        public sealed class TheDescriptionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateRunnerInfo();

                // When
                var result = info.Description;

                // Then
                Assert.Equal("my runner", result);
            }
        }

        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateRunnerInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(10, result);
            }
        }

        public sealed class TheTagsProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitLabCIInfoFixture().CreateRunnerInfo();

                // When
                var result = info.Tags;

                // Then
                Assert.Equal(new[] { "docker", "linux" }, result);
            }
        }
    }
}
