// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.WoodpeckerCI.Data;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core.IO;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.WoodpeckerCI.Data
{
    public sealed class WoodpeckerCIEnvironmentInfoTests
    {
        public sealed class TheCIProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.CI;

                // Then
                Assert.Equal("woodpecker", result);
            }
        }

        public sealed class TheWorkspaceProperty
        {
            [Fact]
            public void Should_Return_Correct_DirectoryPath_For_Valid_Path()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Workspace;

                // Then
                Assert.NotNull(result);
                Assert.Equal("/woodpecker/src/git.example.com/john-doe/my-repo", result.FullPath);
            }

            [Fact]
            public void Should_Return_Null_For_Empty_Path()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_WORKSPACE", "");
                var info = fixture.CreateEnvironmentInfo();

                // When
                var result = info.Workspace;

                // Then
                Assert.Null(result);
            }

            [Fact]
            public void Should_Return_Null_For_Missing_Path()
            {
                // Given
                var fixture = new WoodpeckerCIInfoFixture();
                fixture.SetEnvironmentVariable("CI_WORKSPACE", null);
                var info = fixture.CreateEnvironmentInfo();

                // When
                var result = info.Workspace;

                // Then
                Assert.Null(result);
            }
        }

        public sealed class TheRepositoryProperty
        {
            [Fact]
            public void Should_Return_Repository_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Repository;

                // Then
                Assert.NotNull(result);
                Assert.Equal("john-doe/my-repo", result.Repo);
            }
        }

        public sealed class TheCommitProperty
        {
            [Fact]
            public void Should_Return_Commit_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Commit;

                // Then
                Assert.NotNull(result);
                Assert.Equal("eba09b46064473a1d345da7abf28b477468e8dbd", result.Sha);
            }
        }

        public sealed class ThePipelineProperty
        {
            [Fact]
            public void Should_Return_Pipeline_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Pipeline;

                // Then
                Assert.NotNull(result);
                Assert.Equal(8, result.Number);
            }
        }

        public sealed class TheWorkflowProperty
        {
            [Fact]
            public void Should_Return_Workflow_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Workflow;

                // Then
                Assert.NotNull(result);
                Assert.Equal("release", result.Name);
            }
        }

        public sealed class TheStepProperty
        {
            [Fact]
            public void Should_Return_Step_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Step;

                // Then
                Assert.NotNull(result);
                Assert.Equal("build package", result.Name);
            }
        }

        public sealed class TheSystemProperty
        {
            [Fact]
            public void Should_Return_System_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.System;

                // Then
                Assert.NotNull(result);
                Assert.Equal("woodpecker", result.Name);
            }
        }

        public sealed class TheForgeProperty
        {
            [Fact]
            public void Should_Return_Forge_Info()
            {
                // Given
                var info = new WoodpeckerCIInfoFixture().CreateEnvironmentInfo();

                // When
                var result = info.Forge;

                // Then
                Assert.NotNull(result);
                Assert.Equal(WoodpeckerCIForgeType.GitHub, result.Type);
            }
        }
    }
}
