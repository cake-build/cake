// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tests.Fixtures.Build;
using System;
using System.Linq;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI.Data
{
    public sealed class ContinuaCIBuildInfoTests
    {
        public sealed class TheIdProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Id;

                // Then
                Assert.Equal(99, result);
            }
        }

        public sealed class TheVersionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Version;

                // Then
                Assert.Equal("v1.2.3", result);
            }
        }

        public sealed class TheBuildKeyProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.StartedBy;

                // Then
                Assert.Equal("TestTrigger", result);
            }
        }

        public sealed class TheIsFeatureBranchBuildProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.IsFeatureBranchBuild;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheBuildNumberProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.BuildNumber;

                // Then
                Assert.Equal(999, result);
            }
        }

        public sealed class TheStartedProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Started;

                // Then
                Assert.Equal(DateTime.Parse("2015-12-15T22:53:37.847+01:00"), result);
            }
        }

        public sealed class TheUsesDefaultBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.UsesDefaultBranch;

                // Then
                Assert.Equal(false, result);
            }
        }

        public sealed class TheHasNewChangesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.HasNewChanges;

                // Then
                Assert.Equal(true, result);
            }
        }

        public sealed class TheChangesetCountProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.ChangesetCount;

                // Then
                Assert.Equal(6, result);
            }
        }

        public sealed class TheIssueCountProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.IssueCount;

                // Then
                Assert.Equal(3, result);
            }
        }

        public sealed class TheElapsedProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Elapsed;

                // Then
                Assert.Equal(TimeSpan.FromSeconds(300), result);
            }
        }

        public sealed class TheTimeOnQueueProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.TimeOnQueue;

                // Then
                Assert.Equal(7777, result);
            }
        }

        public sealed class TheRepositoriesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.Repositories.ToArray();

                // Then
                Assert.Equal(result.Length, 3);
                Assert.Equal(result[0], "Repo1");
                Assert.Equal(result[2], "Repo3");
            }
        }

        public sealed class TheRepositoryBranchesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.RepositoryBranches.ToArray();

                // Then
                Assert.Equal(result.Length, 3);
                Assert.Equal(result[0], "Branch1");
                Assert.Equal(result[2], "Branch3");
            }
        }

        public sealed class TheTriggeringBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.TriggeringBranch;

                // Then
                Assert.Equal(result, "Branch2");
            }
        }

        public sealed class TheChangesetRevisionsProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.ChangesetRevisions.ToArray();

                // Then
                Assert.Equal(result.Length, 3);
                Assert.Equal(result[0], "6");
                Assert.Equal(result[2], "65");
            }
        }

        public sealed class TheChangesetUserNamesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.ChangesetUserNames.ToArray();

                // Then
                Assert.Equal(result.Length, 2);
                Assert.Equal(result[0], "george");
                Assert.Equal(result[1], "bill");
            }
        }

        public sealed class TheChangesetTagNamesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateBuildInfo();

                // When
                var result = info.ChangesetTagNames.ToArray();

                // Then
                Assert.Equal(result.Length, 3);
                Assert.Equal(result[0], "tag1");
                Assert.Equal(result[2], "tag 3");
            }
        }
    }
}
