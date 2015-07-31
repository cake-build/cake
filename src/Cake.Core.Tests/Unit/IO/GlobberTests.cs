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
            [Fact]
            public void Should_Throw_If_Pattern_Is_Null()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = Record.Exception(() => globber.Match(null));

                // Then
                Assert.IsArgumentNullException(result, "pattern");
            }

            [Fact]
            public void Should_Return_Empty_Result_If_Pattern_Is_Empty()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match(string.Empty);

                // Then
                Assert.Equal(0, result.Count());
            }

            [Fact]
            public void Can_Traverse_Recursivly()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**/*.txt").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
                Assert.ContainsFilePath(result, "/Temp/Goodbye/OtherText.txt");
            }

            [Fact]
            public void Will_Append_Relative_Root_With_Implicit_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture(windows: true);
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "C:/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Throw_If_Unc_Root_Was_Encountered()
            {
                // Given
                var fixture = new GlobberFixture(windows: true);
                var globber = fixture.CreateGlobber();

                // When
                var result = Record.Exception(() => globber.Match("//Hello/World/Text.txt"));

                // Then
                Assert.IsType<NotSupportedException>(result);
                Assert.Equal("UNC paths are not supported.", result.Message);
            }

            [Fact]
            public void Should_Ignore_Case_Sensitivity_On_Case_Insensitive_Operative_System()
            {
                // Given
                var fixture = new GlobberFixture(windows: true);
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("C:/Temp/**/text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<FilePath>(result[0]);
                Assert.ContainsFilePath(result, "C:/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Absolute_File_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<FilePath>(result[0]);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Absolute_Directory_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/Hello/World").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsDirectoryPath(result, "/Temp/Hello/World");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Relative_File_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("/Temp/Hello");
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("./World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_Single_Path_For_Relative_Directory_Path_Without_Glob_Pattern()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("/Temp/Hello");
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("./World").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsDirectoryPath(result, "/Temp/Hello/World");
            }

            [Fact]
            public void Should_Return_Single_Path_Glob_Pattern_With_Predicate()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match(
                    "./**/*.txt",
                    predicate => predicate.Path is DirectoryPath || predicate.Path.FullPath == "/Temp/Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**/*").ToArray();

                // Then
                Assert.Equal(7, result.Length);
                Assert.ContainsDirectoryPath(result, "/Temp/Hello");
                Assert.ContainsDirectoryPath(result, "/Temp/Hello/World");
                Assert.ContainsDirectoryPath(result, "/Temp/Goodbye");
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Picture.png");
                Assert.ContainsFilePath(result, "/Temp/Goodbye/OtherText.txt");
                Assert.ContainsFilePath(result, "/Temp/Goodbye/OtherPicture.png");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Containing_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/Hello/*/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Ending_With_Character_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**/Te?t.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_Files_And_Folders_For_Pattern_Containing_Character_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/Hello/W???d/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
            }

            [Fact]
            public void Should_Return_File_For_Recursive_Wildcard_Pattern_Ending_With_Wildcard_Regex()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**/*.txt").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.ContainsFilePath(result, "/Temp/Hello/World/Text.txt");
                Assert.ContainsFilePath(result, "/Temp/Goodbye/OtherText.txt");
            }

            [Fact]
            public void Should_Return_Only_Folders_For_Pattern_Ending_With_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**").ToArray();

                // Then
                Assert.Equal(4, result.Length);
                Assert.ContainsDirectoryPath(result, "/Temp");
                Assert.ContainsDirectoryPath(result, "/Temp/Hello");
                Assert.ContainsDirectoryPath(result, "/Temp/Hello/World");
                Assert.ContainsDirectoryPath(result, "/Temp/Goodbye");
            }

            [Fact]
            public void Should_Include_Files_In_Root_Folder_When_Using_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Working/**/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsFilePath(result, "/Working/Text.txt");
            }

            [Fact]
            public void Should_Include_Folder_In_Root_Folder_When_Using_Recursive_Wildcard()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Working/**/NotWorking").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.ContainsDirectoryPath(result, "/Working/NotWorking");
            }
        }
    }
}
