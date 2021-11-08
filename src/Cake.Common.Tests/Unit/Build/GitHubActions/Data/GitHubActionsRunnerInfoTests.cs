// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    }
}
