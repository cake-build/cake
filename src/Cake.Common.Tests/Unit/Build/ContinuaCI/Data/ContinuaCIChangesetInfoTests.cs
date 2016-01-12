using Cake.Common.Tests.Fixtures.Build;
using System;
using System.Linq;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.ContinuaCI.Data
{
    public sealed class ContinuaCIChangesetInfoTests
    {
        public sealed class TheRevisionProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.Revision;

                // Then
                Assert.Equal("55", result);
            }
        }

        public sealed class TheBranchProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.Branch;

                // Then
                Assert.Equal("master", result);
            }
        }

        public sealed class TheCreatedProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.Created;

                // Then
                Assert.Equal(DateTime.Parse("2016-01-02T12:00:16.666+11:00"), result);
            }
        }

        public sealed class TheFileCountProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.FileCount;

                // Then
                Assert.Equal(77, result);
            }
        }

        public sealed class TheUserNameProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.UserName;

                // Then
                Assert.Equal("georgedawes", result);
            }
        }

        public sealed class TheTagCountProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.TagCount;

                // Then
                Assert.Equal(2, result);
            }
        }

        public sealed class TheIssueCountProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.IssueCount;

                // Then
                Assert.Equal(3, result);
            }
        }

        public sealed class TheTagNamesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.TagNames;

                // Then
                Assert.Equal(2, result.Count());
                Assert.Equal("the tag", result.First());
                Assert.Equal("the other tag", result.Last());
            }
        }

        public sealed class TheIssueNamesProperty
        {
            [Fact]
            public void Should_Return_Correct_Value()
            {
                // Given
                var info = new ContinuaCIInfoFixture().CreateChangesetInfo();

                // When
                var result = info.IssueNames;

                // Then
                Assert.Equal(3, result.Count());
                Assert.Equal("an important issue", result.First());
                Assert.Equal("a not so important issue", result.Last());
            }
        }
    }
}