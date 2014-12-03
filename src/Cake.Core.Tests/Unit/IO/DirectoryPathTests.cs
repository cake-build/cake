﻿using System;
using Cake.Core.IO;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace Cake.Core.Tests.Unit.IO
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
                var result = path.CombineWithFilePath(new FilePath(second));

                // Then
                Assert.Equal(expected, result.FullPath);
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var path = new DirectoryPath("assets");

                // When
                var result = Record.Exception(() => path.CombineWithFilePath(null));

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
                var result = Record.Exception(() => path.CombineWithFilePath(new FilePath("/other/asset.txt")));

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

        public sealed class TheMakeAbsoluteMethod
        {
            public sealed class ThatTakesAnEnvironment
            {
                [Fact]
                public void Should_Throw_If_Provided_Environment_Is_Null()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = Record.Exception(
                        () => path.MakeAbsolute((ICakeEnvironment) null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_When_Path_Is_Relative()
                {
                    // Given
                    var environment = Substitute.For<ICakeEnvironment>();
                    environment.WorkingDirectory.Returns("/Working");
                    var path = new DirectoryPath("assets");

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal("/Working/assets", result.FullPath);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_Identical_To_The_Path()
                {
                    // Given
                    var environment = Substitute.For<ICakeEnvironment>();
                    var path = new DirectoryPath("/assets");

                    // When
                    var result = path.MakeAbsolute(environment);

                    // Then
                    Assert.Equal("/assets", result.FullPath);
                }
            }

            public sealed class ThatTakesAnotherDirectoryPath
            {
                [Fact]
                public void Should_Throw_If_Provided_Path_Is_Null()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = Record.Exception(
                        () => path.MakeAbsolute((DirectoryPath)null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("path", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Provided_Path_Is_Relative()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = Record.Exception(() => path.MakeAbsolute("Working"));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("The provided path cannot be relative.", result.Message);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_When_Path_Is_Relative()
                {
                    // Given
                    var path = new DirectoryPath("assets");

                    // When
                    var result = path.MakeAbsolute("/absolute");

                    // Then
                    Assert.Equal("/absolute/assets", result.FullPath);
                }

                [Fact]
                public void Should_Create_New_Absolute_Path_Identical_To_The_Path()
                {
                    // Given
                    var path = new DirectoryPath("/assets");

                    // When
                    var result = path.MakeAbsolute("/absolute");

                    // Then
                    Assert.Equal("/assets", result.FullPath);
                }
            }
        }
    }
}