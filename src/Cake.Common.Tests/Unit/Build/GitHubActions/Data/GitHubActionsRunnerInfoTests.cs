// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GitHubActions.Data;
using Cake.Common.Tests.Fixtures.Build;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.GitHubActions.Data
{
    public sealed class GitHubActionsRunnerInfoTests
    {
        public sealed class TheNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.Name;

                // Then
                Assert.Equal("RunnerName", result);
            }
        }

        // ReSharper disable once InconsistentNaming
        public sealed class TheOSProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.OS;

                // Then
                Assert.Equal("Linux", result);
            }
        }

        public sealed class TheTempProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.Temp.FullPath;

                // Then
                Assert.Equal("/home/runner/work/_temp", result);
            }
        }

        public sealed class TheToolCacheProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.ToolCache.FullPath;

                // Then
                Assert.Equal("/opt/hostedtoolcache", result);
            }
        }

        public sealed class TheWorkspaceProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.Workspace.FullPath;

                // Then
                Assert.Equal("/home/runner/work/cake", result);
            }
        }

        public sealed class TheImageOSProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.ImageOS;

                // Then
                Assert.Equal("ubuntu20", result);
            }
        }

        public sealed class TheImageVersionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.ImageVersion;

                // Then
                Assert.Equal("20211209.3", result);
            }
        }

        public sealed class TheUserProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo();

                // When
                var result = info.User;

                // Then
                Assert.Equal("runner", result);
            }
        }

        public sealed class TheArchitectureProperty
        {
            [Theory]
            [InlineData("X86", GitHubActionsArchitecture.X86)]
            [InlineData("X64", GitHubActionsArchitecture.X64)]
            [InlineData("ARM", GitHubActionsArchitecture.ARM)]
            [InlineData("ARM64", GitHubActionsArchitecture.ARM64)]
            [InlineData("", GitHubActionsArchitecture.Unknown)]
            public void Should_Return_Correct_Value(string value, GitHubActionsArchitecture expected)
            {
                // Given
                var info = new GitHubActionsInfoFixture().CreateRunnerInfo(architecture: value);

                // When
                var result = info.Architecture;

                // Then
                Assert.Equal(expected, result);
            }
        }
    }
}
