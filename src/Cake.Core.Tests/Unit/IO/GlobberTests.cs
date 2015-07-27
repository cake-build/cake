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
            public void Should_Throw_If_Pattern_Is_Empty()
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
            public void Can_Traverse_Recursivly()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**/*.txt").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.True(result.Any(p => p.FullPath == "/Temp/Hello/World/Text.txt"));
                Assert.True(result.Any(p => p.FullPath == "/Temp/Goodbye/OtherText.txt"));
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
                Assert.Equal("/Temp/Hello/World/Text.txt", result[0].FullPath);
            }

            [Fact]
            public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture();
                fixture.SetWorkingDirectory("C:/Working/");
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("C:/Temp/Hello/World/Text.txt", result[0].FullPath);
            }

            [Fact]
            public void Should_Throw_If_Unc_Root_Was_Encountered()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = fixture.CreateGlobber();

                // When
                var result = Record.Exception(() => globber.Match("//Hello/World/Text.txt"));

                // Then
                Assert.IsType<NotSupportedException>(result);
                Assert.Equal("UNC paths are not supported.", result.Message);
            }

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Should_Ignore_Case_Sensitivity_On_Case_Insensitive_Operative_System(bool isFileSystemCaseSensitive, bool shouldFindFile)
            {
                // Given
                var fixture = new GlobberFixture(isFileSystemCaseSensitive);
                var globber = fixture.CreateGlobber();

                // When
                var result = globber.Match("/Temp/**/text.txt").ToArray();

                // Then
                Assert.Equal(shouldFindFile, result.Length == 1);
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
                Assert.Equal("/Temp/Hello/World/Text.txt", result[0].FullPath);
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
                Assert.IsType<DirectoryPath>(result[0]);
                Assert.Equal("/Temp/Hello/World", result[0].FullPath);
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
                Assert.IsType<FilePath>(result[0]);
                Assert.Equal("/Temp/Hello/World/Text.txt", result[0].FullPath);
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
                Assert.IsType<DirectoryPath>(result[0]);
                Assert.Equal("/Temp/Hello/World", result[0].FullPath);
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
                    predicate=>predicate.Path is DirectoryPath || predicate.Path.FullPath == "/Temp/Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.IsType<FilePath>(result[0]);
                Assert.Equal("/Temp/Hello/World/Text.txt", result[0].FullPath);
            }
        }
    }
}
