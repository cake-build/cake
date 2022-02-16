// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitHubActions.Data;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Data
{
    public sealed class GitHubActionsWorkflowInfoTests
    {
        public sealed class TheActionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Action;

                // Then
                Assert.Equal("run1", result);
            }
        }

        public sealed class TheActionPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.ActionPath.FullPath;

                // Then
                Assert.Equal("/path/to/action", result);
            }
        }

        public sealed class TheActorProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Actor;

                // Then
                Assert.Equal("dependabot", result);
            }
        }

        public sealed class TheApiUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.ApiUrl;

                // Then
                Assert.Equal("https://api.github.com", result);
            }
        }

        public sealed class TheBaseRefProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.BaseRef;

                // Then
                Assert.Equal("master", result);
            }
        }

        public sealed class TheEventNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.EventName;

                // Then
                Assert.Equal("pull_request", result);
            }
        }

        public sealed class TheEventPathProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.EventPath.FullPath;

                // Then
                Assert.Equal("/home/runner/work/_temp/_github_workflow/event.json", result);
            }
        }

        public sealed class TheGraphQLUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.GraphQLUrl;

                // Then
                Assert.Equal("https://api.github.com/graphql", result);
            }
        }

        public sealed class TheHeadRefProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.HeadRef;

                // Then
                Assert.Equal("dependabot/nuget/Microsoft.SourceLink.GitHub-1.0.0", result);
            }
        }

        public sealed class TheJobProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Job;

                // Then
                Assert.Equal("job", result);
            }
        }

        public sealed class TheRefProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Ref;

                // Then
                Assert.Equal("refs/pull/1/merge", result);
            }
        }

        public sealed class TheRepositoryProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Repository;

                // Then
                Assert.Equal("cake-build/cake", result);
            }
        }

        public sealed class TheRepositoryOwnerProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.RepositoryOwner;

                // Then
                Assert.Equal("cake-build", result);
            }
        }

        public sealed class TheRunIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.RunId;

                // Then
                Assert.Equal("34058136", result);
            }
        }

        public sealed class TheRunNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.RunNumber;

                // Then
                Assert.Equal(60, result);
            }
        }

        public sealed class TheServerUrlProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.ServerUrl;

                // Then
                Assert.Equal("https://github.com", result);
            }
        }

        public sealed class TheShaProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Sha;

                // Then
                Assert.Equal("d1e4f990f57349334368c8253382abc63be02d73", result);
            }
        }

        public sealed class TheWorkflowProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Workflow;

                // Then
                Assert.Equal("Build", result);
            }
        }

        public sealed class TheWorkspaceProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Workspace.FullPath;

                // Then
                Assert.Equal("/home/runner/work/cake/cake", result);
            }
        }

        public sealed class TheAttemptProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.Attempt;

                // Then
                Assert.Equal(2, result);
            }
        }

        public sealed class TheRefProtectedProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.RefProtected;

                // Then
                Assert.True(result);
            }
        }

        public sealed class TheRefNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo();

                // When
                var result = info.RefName;

                // Then
                Assert.Equal("main", result);
            }
        }

        public sealed class TheRefTypeProperty
        {
            [Theory]
            [InlineData("branch", GitHubActionsRefType.Branch)]
            [InlineData("tag", GitHubActionsRefType.Tag)]
            [InlineData("", GitHubActionsRefType.Unknown)]
            public void Should_Return_Correct_Value(string value, GitHubActionsRefType expected)
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateWorkflowInfo(refType: value);

                // When
                var result = info.RefType;

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
