// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
                var result = info.TagNames.ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.Equal("the tag", result[0]);
                Assert.Equal("the other tag", result[1]);
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
                var result = info.IssueNames.ToArray();

                // Then
                Assert.Equal(3, result.Length);
                Assert.Equal("an important issue", result[0]);
                Assert.Equal("a not so important issue", result[2]);
            }
        }
    }
}
