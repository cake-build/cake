// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Testing.Xunit;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class PathTests
    {
        #region Private Test Classes

        private sealed class TestingPath : Path
        {
            public TestingPath(string path)
                : base(path)
            {
            }
        }

        #endregion

        public sealed class TheConstructor
        {
            [Fact]
            public void Current_Directory_Returns_Empty_Path()
            {
                // Given, When
                var path = new TestingPath("./");

                // Then
                Assert.Equal(string.Empty, path.FullPath);
            }

            [Fact]
            public void Will_Normalize_Path_Separators()
            {
                // Given, When
                var path = new TestingPath("shaders\\basic");

                // Then
                Assert.Equal("shaders/basic", path.FullPath);
            }

            [Fact]
            public void Will_Normalize_UNC_Path_Separators()
            {
                // Given, When
                var path = new TestingPath(@"\\foo/bar\qux");

                // Then
                Assert.Equal(@"\\foo\bar\qux", path.FullPath);
            }

            [Theory]
            [InlineData(" foo/bar ", "foo/bar")]
            [InlineData(@" \\foo\bar ", @"\\foo\bar")]
            public void Will_Trim_WhiteSpace_From_Path(string input, string expected)
            {
                // Given, When
                var path = new TestingPath(input);

                // Then
                Assert.Equal(expected, path.FullPath);
            }

            [Theory]
            [InlineData("foo bar/qux")]
            [InlineData(@"\\foo bar\qux")]
            public void Will_Not_Remove_WhiteSpace_Within_Path(string fullPath)
            {
                // Given, When
                var path = new TestingPath(fullPath);

                // Then
                Assert.Equal(fullPath, path.FullPath);
            }

            [Theory]
            [InlineData("/Hello/World/", "/Hello/World")]
            [InlineData("\\Hello\\World\\", "/Hello/World")]
            [InlineData("file.txt/", "file.txt")]
            [InlineData("file.txt\\", "file.txt")]
            [InlineData("Temp/file.txt/", "Temp/file.txt")]
            [InlineData("Temp\\file.txt\\", "Temp/file.txt")]
            [InlineData(@"\\foo\bar\", @"\\foo\bar")]
            [InlineData(@"\\foo\bar/", @"\\foo\bar")]
            public void Should_Remove_Trailing_Slashes(string value, string expected)
            {
                // Given, When
                var path = new TestingPath(value);

                // Then
                Assert.Equal(expected, path.FullPath);
            }

            [Fact]
            public void Should_Not_Remove_Trailing_Slash_For_Root()
            {
                // Given, When
                var path = new TestingPath("/");

                // Then
                Assert.Equal("/", path.FullPath);
            }

            [Fact]
            public void Should_Not_Remove_Trailing_Slash_For_UNC_Root()
            {
                // Given, When
                var path = new TestingPath(@"\\");

                // Then
                Assert.Equal(@"\\", path.FullPath);
            }
        }

        public sealed class TheIsUNCProperty
        {
            [Theory]
            [InlineData(@"\\Hello\World", true)]
            [InlineData("Hello/World", false)]
            [InlineData("./Hello/World", false)]
            public void Should_Return_Whether_Or_Not_A_Path_Is_An_UNC_Path(string pathName, bool expected)
            {
                // Given
                var path = new TestingPath(pathName);

                // When
                var result = path.IsUNC;

                // Then
                Assert.Equal(expected, result);
            }
        }

        public sealed class TheSegmentsProperty
        {
            [Theory]
            [InlineData("Hello/World")]
            [InlineData("./Hello/World/")]
            public void Should_Return_Segments_Of_Path(string pathName)
            {
                // Given
                var path = new TestingPath(pathName);

                // When, Then
                Assert.Equal(2, path.Segments.Length);
                Assert.Equal("Hello", path.Segments[0]);
                Assert.Equal("World", path.Segments[1]);
            }

            [Theory]
            [InlineData(@"\\Hello\World")]
            public void Should_Return_Segments_Of_UNC_Path(string pathName)
            {
                // Given
                var path = new TestingPath(pathName);

                // When, Then
                Assert.Equal(3, path.Segments.Length);
                Assert.Equal(@"\\", path.Segments[0]);
                Assert.Equal("Hello", path.Segments[1]);
                Assert.Equal("World", path.Segments[2]);
            }

            [Theory]
            [InlineData("/Hello/World")]
            [InlineData("/Hello/World/")]
            public void Should_Return_Segments_Of_Path_And_Leave_Absolute_Directory_Separator_Intact(string pathName)
            {
                // Given
                var path = new TestingPath(pathName);

                // When, Then
                Assert.Equal(2, path.Segments.Length);
                Assert.Equal("/Hello", path.Segments[0]);
                Assert.Equal("World", path.Segments[1]);
            }
        }

        public sealed class TheFullPathProperty
        {
            [Fact]
            public void Should_Return_Full_Path()
            {
                // Given, When
                var path = new TestingPath("shaders/basic");

                // Then
                Assert.Equal("shaders/basic", path.FullPath);
            }
        }

        public sealed class TheIsRelativeProperty
        {
            [Theory]
            [InlineData("assets/shaders", true)]
            [InlineData("assets/shaders/basic.frag", true)]
            [InlineData("/assets/shaders", false)]
            [InlineData("/assets/shaders/basic.frag", false)]
            public void Should_Return_Whether_Or_Not_A_Path_Is_Relative(string fullPath, bool expected)
            {
                // Given, When
                var path = new TestingPath(fullPath);

                // Then
                Assert.Equal(expected, path.IsRelative);
            }

            [Fact]
            public void An_UNC_Path_Is_Always_Considered_To_Be_Absolute()
            {
                // Given, When
                var path = new TestingPath(@"\\foo\bar");

                // Then
                Assert.False(path.IsRelative);
            }

            [WindowsTheory]
            [InlineData("c:/assets/shaders", false)]
            [InlineData("c:/assets/shaders/basic.frag", false)]
            [InlineData("c:/", false)]
            [InlineData("c:", false)]
            public void Should_Return_Whether_Or_Not_A_Path_Is_Relative_On_Windows(string fullPath, bool expected)
            {
                // Given, When
                var path = new TestingPath(fullPath);

                // Then
                Assert.Equal(expected, path.IsRelative);
            }
        }

        public sealed class TheToStringMethod
        {
            [Theory]
            [InlineData("foo/bar")]
            [InlineData(@"\\foo\bar")]
            public void Should_Return_The_Full_Path(string fullPath)
            {
                // Given, When
                var path = new TestingPath(fullPath);

                // Then
                Assert.Equal(fullPath, path.ToString());
            }
        }
    }
}