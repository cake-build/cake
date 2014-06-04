using System;
using Cake.Core.IO;
using Xunit;
using Xunit.Extensions;

namespace Cake.Core.Tests.Unit.IO.Paths
{
    public sealed class DirectoryPathTests
    {
        public sealed class TheGetFilePathMethod
        {
            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple.frag", "c:/assets/shaders/simple.frag")]
            [InlineData("c:/", "simple.frag", "c:/simple.frag")]
#endif
            [InlineData("assets/shaders", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("assets/shaders/", "simple.frag", "assets/shaders/simple.frag")]
            [InlineData("/assets/shaders/", "simple.frag", "/assets/shaders/simple.frag")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.GetFilePath(new FilePath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.GetFilePath(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Not_Combine_Directory_Path_With_Absolute_File_Path()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.GetFilePath(new FilePath("/other/asset.txt")));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Cannot combine a directory path with an absolute file path.", result.Message);
            }
        }

        public sealed class TheCombineWithDirectoryPathMethod
        {
            [Theory]
#if !UNIX
            [InlineData("c:/assets/shaders/", "simple", "c:/assets/shaders/simple")]
            [InlineData("c:/", "simple", "c:/simple")]
#endif
            [InlineData("assets/shaders", "simple", "assets/shaders/simple")]
            [InlineData("assets/shaders/", "simple", "assets/shaders/simple")]
            [InlineData("/assets/shaders/", "simple", "/assets/shaders/simple")]
            public void Should_Combine_Paths(string first, string second, string expected)
            {
                // Given
                var path = new DirectoryPath(first);

                // When
                var result = path.Combine(new DirectoryPath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("path", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Not_Combine_Directory_Path_With_Absolute_Directory_Path()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.Combine(new DirectoryPath("/other/assets")));

                // Then
                Assert.IsType<InvalidOperationException>(result);
                Assert.Equal("Cannot combine a directory path with an absolute directory path.", result.Message);
            }
        }
    }
}