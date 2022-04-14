// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class PathCollapserTests
    {
        public sealed class TheCollapseMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => PathCollapser.Collapse(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            public sealed class WithPathsInRelativeFormat
            {
                [Fact]
                public void Should_Collapse_Relative_Path()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("hello/temp/test/../../world"));

                    // Then
                    Assert.Equal("hello/world", path);
                }

                [Fact]
                public void Should_Collapse_Path_With_Separated_Ellipsis()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("hello/temp/../temp2/../world"));

                    // Then
                    Assert.Equal("hello/world", path);
                }

                [Theory]
                [InlineData("./foo/..", ".")]
                [InlineData("foo/..", ".")]
                public void Should_Collapse_To_Dot_When_Only_One_Folder_Is_Followed_By_Ellipsis(string input,
                    string expected)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(input));

                    // Then
                    Assert.Equal(expected, path);
                }

                [Theory]
                [InlineData(".")]
                [InlineData("./")]
                [InlineData("")]
                public void Should_Collapse_Single_Dot_To_Single_Dot(string uncollapsedPath)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(uncollapsedPath));

                    // Then
                    Assert.Equal(".", path);
                }

                [Fact]
                public void Should_Collapse_Single_Dot_With_Ellipsis()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("./.."));

                    // Then
                    Assert.Equal(".", path);
                }

                [Theory]
                [InlineData("./a", "a")]
                [InlineData("a/./b", "a/b")]
                [InlineData("a/b/.", "a/b")]
                public void Should_Collapse_Single_Dot(string uncollapsedPath, string collapsedPath)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(uncollapsedPath));

                    // Then
                    Assert.Equal(collapsedPath, path);
                }
            }

            public sealed class WithPathsInUncFormat
            {
                [Theory]
                [InlineData(@"\\server\share\folder\..", @"\\server\share")]
                [InlineData(@"\\server\share\folder\..\..\..\..", @"\\server")]
                [InlineData(@"\\server\share\folder\..\..\..\..\foo", @"\\server\foo")]
                public void Should_Collapse_Ellipsis(string input,
                    string expected)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(input));

                    // Then
                    Assert.Equal(expected, path);
                }
            }

            public sealed class WithPathsInNonWindowsFormat
            {
                [Fact]
                public void Should_Collapse_Path_With_Non_Windows_Root()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("/hello/temp/test/../../world"));

                    // Then
                    Assert.Equal("/hello/world", path);
                }

                [NonWindowsFact]
                public void Should_Stop_Collapsing_When_Root_Is_Reached()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("/hello/../../../../../../temp"));

                    // Then
                    Assert.Equal("/temp", path);
                }

                [NonWindowsTheory]
                [InlineData("/foo/..", "/")]
                [InlineData("/..", "/")]
                public void Should_Collapse_To_Root_When_Only_One_Folder_Is_Followed_By_Ellipsis(string input,
                    string expected)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(input));

                    // Then
                    Assert.Equal(expected, path);
                }

                [Theory]
                [InlineData("/a/./b", "/a/b")]
                [InlineData("/a/b/.", "/a/b")]
                [InlineData("/./a/b", "/a/b")]
                public void Should_Collapse_Single_Dot(string uncollapsedPath, string collapsedPath)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(uncollapsedPath));

                    // Then
                    Assert.Equal(collapsedPath, path);
                }
            }

            public sealed class WithPathsInWindowsFormat
            {
                [Fact]
                public void Should_Collapse_Path_With_Windows_Root()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("c:/hello/temp/test/../../world"));

                    // Then
                    Assert.Equal("c:/hello/world", path);
                }

                [WindowsFact]
                public void Should_Stop_Collapsing_When_Windows_Root_Is_Reached()
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath("c:/../../../../../../temp"));

                    // Then
                    Assert.Equal("c:/temp", path);
                }

                [Theory]
                [InlineData("C:/foo/..", "C:")]
                public void Should_Collapse_To_Root_When_Only_One_Folder_Is_Followed_By_Ellipsis(string input,
                    string expected)
                {
                    // Given, When
                    var path = PathCollapser.Collapse(new DirectoryPath(input));

                    // Then
                    Assert.Equal(expected, path);
                }
            }
        }
    }
}