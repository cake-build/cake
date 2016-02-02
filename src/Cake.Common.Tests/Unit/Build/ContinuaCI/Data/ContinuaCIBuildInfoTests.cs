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
                var result = info.Repositories;

                // Then
                Assert.Equal(result.Count(), 3);
                Assert.Equal(result.First(), "Repo1");
                Assert.Equal(result.Last(), "Repo3");
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
                var result = info.RepositoryBranches;

                // Then
                Assert.Equal(result.Count(), 3);
                Assert.Equal(result.First(), "Branch1");
                Assert.Equal(result.Last(), "Branch3");
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
                var result = info.ChangesetRevisions;

                // Then
                Assert.Equal(result.Count(), 3);
                Assert.Equal(result.First(), "6");
                Assert.Equal(result.Last(), "65");
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
                var result = info.ChangesetUserNames;

                // Then
                Assert.Equal(result.Count(), 2);
                Assert.Equal(result.First(), "george");
                Assert.Equal(result.Last(), "bill");
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
                var result = info.ChangesetTagNames;

                // Then
                Assert.Equal(result.Count(), 3);
                Assert.Equal(result.First(), "tag1");
                Assert.Equal(result.Last(), "tag 3");
            }
        }
    }
}