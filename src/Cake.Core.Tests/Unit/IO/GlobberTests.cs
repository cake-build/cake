using System;
using System.Linq;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

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
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("fileSystem", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var fileSystem = Substitute.For<IFileSystem>();
                var result = Record.Exception(() => new Globber(fileSystem, null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("environment", ((ArgumentNullException)result).ParamName);
            }
        }

        public sealed class TheMatchMethod
        {
            [Fact]
            public void Should_Throw_If_Pattern_Is_Empty()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = new Globber(fixture.FileSystem, fixture.Environment);

                // When
                var result = Record.Exception(() => globber.Match(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("pattern", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Can_Traverse_Recursivly()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = new Globber(fixture.FileSystem, fixture.Environment);

                // When
                var result = globber.Match("/Temp/**/*.txt").ToArray();

                // Then
                Assert.Equal(2, result.Length);
                Assert.Equal("/Temp/Hello/World/Text.txt", result[0].FullPath);
                Assert.Equal("/Temp/Goodbye/OtherText.txt", result[1].FullPath);
            }

            [Fact]
            public void Will_Append_Relative_Root_With_Implicit_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture();
                var globber = new Globber(fixture.FileSystem, fixture.Environment);

                // When
                var result = globber.Match("Hello/World/Text.txt").ToArray();

                // Then
                Assert.Equal(1, result.Length);
                Assert.Equal("/Working/Hello/World/Text.txt", result[0].FullPath);
            }

#if !UNIX
            [Fact]
            public void Will_Fix_Root_If_Drive_Is_Missing_By_Using_The_Drive_From_The_Working_Directory()
            {
                // Given
                var fixture = new GlobberFixture(isUnix: false);
                fixture.Environment.WorkingDirectory.Returns("C:/Working/");
                var globber = new Globber(fixture.FileSystem, fixture.Environment);

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
                var fixture = new GlobberFixture(isUnix: false);
                var globber = new Globber(fixture.FileSystem, fixture.Environment);

                // When
                var result = Record.Exception(() => globber.Match("//Hello/World/Text.txt"));

                // Then
                Assert.IsType<NotSupportedException>(result);
                Assert.Equal("UNC paths are not supported.", result.Message);
            }
#endif

            [Theory]
            [InlineData(true, false)]
            [InlineData(false, true)]
            public void Should_Ignore_Case_Sensitivity_On_Case_Insensitive_Operative_System(bool isUnix, bool shouldFindFile)
            {
                // Given
                var fixture = new GlobberFixture(isUnix);
                var globber = new Globber(fixture.FileSystem, fixture.Environment);

                // When
                var result = globber.Match("/Temp/**/text.txt").ToArray();

                // Then
                Assert.Equal(shouldFindFile, result.Length == 1);
            }
        }
    }
}
