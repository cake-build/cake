using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class GlobberTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given, When
                var environment = Substitute.For<ICakeEnvironment>();
                var result = Record.Exception(() => new Globber(null, environment));

                // Then
                Assert.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new Globber(fileSystem, null));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheMatchMethod
        {
            public sealed class WindowsSpecific
            {
                [Fact]
                public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("/Working/Foo/Bar/Qux.c");

                    // Then
                    Assert.Equal(1, result.Length);
                    Assert.ContainsFilePath(result, "C:/Working/Foo/Bar/Qux.c");
                }

                [Fact]
                public void Should_Throw_If_Unc_Root_Was_Encountered()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = Record.Exception(() => fixture.Match("//Foo/Bar/Qux.c"));

                    // Then
                    Assert.IsType<NotSupportedException>(result);
                    Assert.Equal("UNC paths are not supported.", result.Message);
                }

                [Fact]
                public void Should_Ignore_Case_Sensitivity_On_Case_Insensitive_Operative_System()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Working/**/qux.c");

                    // Then
                    Assert.Equal(1, result.Length);
                    Assert.IsType<FilePath>(result[0]);
                    Assert.ContainsFilePath(result, "C:/Working/Foo/Bar/Qux.c");
                }
            }

            public sealed class WithPredicate
            {
                [Fact]
                public void Should_Return_Single_Path_Glob_Pattern_With_Predicate()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i => i.Path.FullPath == "/Working/Foo/Bar/Qux.c");

                    // When
                    var result = fixture.Match("./**/*.c", predicate);

                    // Then
                    Assert.Equal(1, result.Length);
                    Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                }
            }

            [Fact]
            public void Should_Throw_If_Pattern_Is_Null()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = Record.Exception(() => fixture.Match(null));

                // Then
                Assert.IsArgumentNullException(result, "pattern");
            }

            [Fact]
            public void Should_Return_Empty_Result_If_Pattern_Is_Empty()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match(string.Empty);

                // Then
                Assert.Equal(0, result.Count());
            }

            [Fact]
            public void Can_Traverse_Recursivly()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*.c");

                // Then
                Assert.Equal(5, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Baz/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Bar/Qux.c");
            }

            [Fact]
            public void Will_Append_Relative_Root_With_Implicit_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("Foo/Bar/Qux.c");

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Absolute_File_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Bar/Qux.c");

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<FilePath>(result[0]);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Absolute_Directory_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Bar");

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Bar");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Relative_File_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("/Working/Foo");

                // When
                var result = fixture.Match("./Bar/Qux.c");

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Relative_Directory_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("/Working/Foo");

                // When
                var result = fixture.Match("./Bar");

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Bar");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*");

                // Then
                Assert.Equal(12, result.Length);
                Assert.ContainsDirectoryPath(result, "/Working/Foo");
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Bar");
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Baz");
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Bar/Baz");
                Assert.ContainsDirectoryPath(result, "/Working/Bar");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.h");
                Assert.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Baz/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Bar/Qux.h");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Containing_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/*/Qux.c");

                // Then
                Assert.Equal(2, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Character_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Bar/Q?x.c");

                // Then
                Assert.Equal(2, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Containing_Character_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/Ba?/Qux.c");

                // Then
                Assert.Equal(2, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
            }

            [Fact]
            public void Should_Return_File_For_Recursive_Wildcard_Pattern_Ending_With_Wildcard_Regex()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*.c");

                // Then
                Assert.Equal(5, result.Length);
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Foo/Bar/Baz/Qux.c");
                Assert.ContainsFilePath(result, "/Working/Bar/Qux.c");
            }

            [Fact]
            public void Should_Return_Only_Folders_For_Pattern_Ending_With_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**");

                // Then
                Assert.Equal(6, result.Length);
                Assert.ContainsDirectoryPath(result, "/Working");
                Assert.ContainsDirectoryPath(result, "/Working/Foo");
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Bar");
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Baz");
                Assert.ContainsDirectoryPath(result, "/Working/Foo/Bar/Baz");
                Assert.ContainsDirectoryPath(result, "/Working/Bar");
            }

            [Fact]
            public void Should_Include_Files_In_Root_Folder_When_Using_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo/**/Bar.baz");

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Foo/Bar.baz");
            }

            [Fact]
            public void Should_Include_Folder_In_Root_Folder_When_Using_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo/**/Bar");

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsDirectoryPath(result, "/Foo/Bar");
            }
        }
    }
}
