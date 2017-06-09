// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitLabCI;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitLabCI
{
    public sealed class GitLabCIProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new GitLabCIProvider(null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheIsRunningOnGitLabCIProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GitLabCI()
            {
                // Given
                var fixture = new GitLabCIFixture();
                fixture.IsRunningOnGitLabCI();
                var gitlabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitlabCI.IsRunningOnGitLabCI;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_GitLabCI()
            {
                // Given
                var fixture = new GitLabCIFixture();
                var gitlabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitlabCI.IsRunningOnGitLabCI;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new GitLabCIFixture();
                var gitlabCI = fixture.CreateGitLabCIService();

                // When
                var result = gitlabCI.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
