// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitHubActions;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions
{
    public sealed class GitHubActionsProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => new GitHubActionsProvider(null, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_FileSystem_Is_Null()
            {
                // Given
                var environment = Substitute.For<ICakeEnvironment>();

                // When
                var result = Record.Exception(() => new GitHubActionsProvider(environment, null));

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }
        }

        public sealed class TheIsRunningOnGitHubActionsProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_GitHubActions()
            {
                // Given
                var fixture = new GitHubActionsFixture();
                fixture.IsRunningOnGitHubActions();
                var gitHubActions = fixture.CreateGitHubActionsService();

                // When
                var result = gitHubActions.IsRunningOnGitHubActions;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_GitHubActions()
            {
                // Given
                var fixture = new GitHubActionsFixture();
                var gitHubActions = fixture.CreateGitHubActionsService();

                // When
                var result = gitHubActions.IsRunningOnGitHubActions;

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
                var fixture = new GitHubActionsFixture();
                var gitHubActions = fixture.CreateGitHubActionsService();

                // When
                var result = gitHubActions.Environment;

                // Then
                Assert.NotNull(result);
            }
        }
    }
}
