// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Testing.Xunit;
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
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fileSystem = Substitute.For<IFileSystem>();

                // When
                var result = Record.Exception(() => new Globber(fileSystem, null));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }
        }

        public sealed class TheMatchMethod
        {
            public sealed class WindowsSpecific
            {
                [WindowsFact]
                public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("/Working/Foo/Bar/Qux.c");

                    // Then
                    Assert.Equal(1, result.Length);
                    AssertEx.ContainsFilePath(result, "C:/Working/Foo/Bar/Qux.c");
                }

                [WindowsFact]
                public void Should_Throw_If_Unc_Root_Was_Encountered()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = Record.Exception(() => fixture.Match("//Foo/Bar/Qux.c"));

                    // Then
                    Assert.IsType<NotSupportedException>(result);
                    Assert.Equal("UNC paths are not supported.", result?.Message);
                }

                [WindowsFact]
                public void Should_Ignore_Case_Sensitivity_On_Case_Insensitive_Operative_System()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Working/**/qux.c");

                    // Then
                    Assert.Equal(1, result.Length);
                    Assert.IsType<FilePath>(result[0]);
                    AssertEx.ContainsFilePath(result, "C:/Working/Foo/Bar/Qux.c");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Parenthesis_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Program Files (x86)/Foo.*");

                    // Then
                    Assert.Equal(1, result.Length);
                    AssertEx.ContainsFilePath(result, "C:/Program Files (x86)/Foo.c");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Ampersand_In_Them()
                {
                  // Given
                  var fixture = new GlobberFixture(windows: true);

                  // When
                  var result = fixture.Match("C:/Tools & Services/*.dll");

                  // Then
                  Assert.Equal(1, result.Length);
                  AssertEx.ContainsFilePath(result, "C:/Tools & Services/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Plus_In_Them()
                {
                  // Given
                  var fixture = new GlobberFixture(windows: true);

                  // When
                  var result = fixture.Match("C:/Tools + Services/*.dll");

                  // Then
                  Assert.Equal(1, result.Length);
                  AssertEx.ContainsFilePath(result, "C:/Tools + Services/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Percent_In_Them()
                {
                  // Given
                  var fixture = new GlobberFixture(windows: true);

                  // When
                  var result = fixture.Match("C:/Some %2F Directory/*.dll");

                  // Then
                  Assert.Equal(1, result.Length);
                  AssertEx.ContainsFilePath(result, "C:/Some %2F Directory/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_Exclamation_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Some ! Directory/*.dll");

                    // Then
                    Assert.Equal(1, result.Length);
                    AssertEx.ContainsFilePath(result, "C:/Some ! Directory/MyTool.dll");
                }

                [WindowsFact]
                public void Should_Parse_Glob_Expressions_With_AtSign_In_Them()
                {
                    // Given
                    var fixture = new GlobberFixture(windows: true);

                    // When
                    var result = fixture.Match("C:/Some@Directory/*.dll");

                    // Then
                    Assert.Equal(1, result.Length);
                    AssertEx.ContainsFilePath(result, "C:/Some@Directory/MyTool.dll");
                }
            }

            public sealed class WithPredicate
            {
                [Fact]
                public void Should_Return_Paths_Not_Affected_By_Walker_Hints()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i =>
                        i.Path.FullPath != "/Working/Bar");

                    // When
                    var result = fixture.Match("./**/Qux.h", predicate);

                    // Then
                    Assert.Equal(1, result.Length);
                    AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.h");
                }

                [Fact]
                public void Should_Not_Return_Path_If_Walker_Hint_Matches_Part_Of_Pattern()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i =>
                        i.Path.FullPath != "/Working/Bar");

                    // When
                    var result = fixture.Match("/Working/Bar/Qux.h", predicate);

                    // Then
                    Assert.Equal(0, result.Length);
                }

                [Fact]
                public void Should_Not_Return_Path_If_Walker_Hint_Exactly_Match_Pattern()
                {
                    // Given
                    var fixture = new GlobberFixture();
                    var predicate = new Func<IFileSystemInfo, bool>(i =>
                        i.Path.FullPath != "/Working/Bar");

                    // When
                    var result = fixture.Match("/Working/Bar", predicate);

                    // Then
                    Assert.Equal(0, result.Length);
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
                AssertEx.IsArgumentNullException(result, "pattern");
            }

            [Fact]
            public void Should_Return_Empty_Result_If_Pattern_Is_Empty()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match(string.Empty);

                // Then
                Assert.Equal(0, result.Length);
            }

            [Fact]
            public void Can_Traverse_Recursively()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*.c");

                // Then
                Assert.Equal(5, result.Length);
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Baz/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Bar/Qux.c");
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Be_Able_To_Visit_Parent_Using_Double_Dots()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/Foo/../Foo/Bar/Qux.c");

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<FilePath>(result[0]);
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
            }

            [Fact]
            public void Should_Throw_If_Visiting_Parent_That_Is_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = Record.Exception(() => fixture.Match("/Working/Foo/**/../Foo/Bar/Qux.c"));

                // Then
                Assert.NotNull(result);
                Assert.IsType<NotSupportedException>(result);
                Assert.Equal("Visiting a parent that is a recursive wildcard is not supported.", result?.Message);
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
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
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Bar");
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
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
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Bar");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/**/*");

                // Then
                Assert.Equal(15, result.Length);
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Bar");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Baz");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Bar/Baz");
                AssertEx.ContainsDirectoryPath(result, "/Working/Bar");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.h");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Baz/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo.Bar.Test.dll");
                AssertEx.ContainsFilePath(result, "/Working/Bar.Qux.Test.dll");
                AssertEx.ContainsFilePath(result, "/Working/Quz.FooTest.dll");
                AssertEx.ContainsFilePath(result, "/Working/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Bar/Qux.h");
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
            }

            [Fact]
            public void Should_Return_Files_For_Pattern_Ending_With_Character_Wildcard_And_Dot()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/*.Test.dll");

                // Then
                Assert.Equal(2, result.Length);
                AssertEx.ContainsFilePath(result, "/Working/Foo.Bar.Test.dll");
                AssertEx.ContainsFilePath(result, "/Working/Bar.Qux.Test.dll");
            }

            [WindowsFact]
            public void Should_Return_Files_For_Pattern_Ending_With_Character_Wildcard_And_Dot_On_Windows()
            {
                // Given
                var fixture = new GlobberFixture(true);

                // When
                var result = fixture.Match("C:/Working/*.Test.dll");

                // Then
                Assert.Equal(2, result.Length);
                AssertEx.ContainsFilePath(result, "C:/Working/Project.A.Test.dll");
                AssertEx.ContainsFilePath(result, "C:/Working/Project.B.Test.dll");
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
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Qex.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Baz/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Foo/Bar/Baz/Qux.c");
                AssertEx.ContainsFilePath(result, "/Working/Bar/Qux.c");
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
                AssertEx.ContainsDirectoryPath(result, "/Working");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Bar");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Baz");
                AssertEx.ContainsDirectoryPath(result, "/Working/Foo/Bar/Baz");
                AssertEx.ContainsDirectoryPath(result, "/Working/Bar");
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
                AssertEx.ContainsFilePath(result, "/Foo/Bar.baz");
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
                AssertEx.ContainsDirectoryPath(result, "/Foo/Bar");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_Parenthesis_In_Them()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo (Bar)/Baz.*");

                // Then
                Assert.Equal(1, result.Length);
                AssertEx.ContainsFilePath(result, "/Foo (Bar)/Baz.c");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_AtSign_In_Them()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Foo@Bar/Baz.*");

                // Then
                Assert.Equal(1, result.Length);
                AssertEx.ContainsFilePath(result, "/Foo@Bar/Baz.c");
            }

            [Fact]
            public void Should_Parse_Glob_Expressions_With_Relative_Directory_Not_At_The_Beginning()
            {
                // Given
                var fixture = new GlobberFixture();

                // When
                var result = fixture.Match("/Working/./*.Test.dll");

                // Then
                Assert.Equal(2, result.Length);
                AssertEx.ContainsFilePath(result, "/Working/Foo.Bar.Test.dll");
                AssertEx.ContainsFilePath(result, "/Working/Bar.Qux.Test.dll");
            }
        }
    }
}