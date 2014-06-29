using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tests.Fakes;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ReleaseNotesExtensionsTests
    {
        public sealed class TheParseReleaseNotesMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => context.ParseReleaseNotes(null));

                // Then
                Assert.IsType<ArgumentNullException>(result);
                Assert.Equal("filePath", ((ArgumentNullException)result).ParamName);
            }

            [Fact]
            public void Should_Throw_If_File_Do_Not_Exist()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = Substitute.For<ICakeEnvironment>();
                environment.WorkingDirectory = "/Working";
                context.FileSystem.Returns(Substitute.For<IFileSystem>());
                context.Environment.Returns(environment);

                // When
                var result = Record.Exception(() => context.ParseReleaseNotes("ReleaseNotes.md"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Release notes file '/Working/ReleaseNotes.md' do not exist.", result.Message);
            }

            [Fact]
            public void Should_Read_Content_Of_File_And_Parse_It()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = Substitute.For<ICakeEnvironment>();
                environment.WorkingDirectory = "/Working";
                var fileSystem = new FakeFileSystem(true);
                fileSystem.GetCreatedFile("/Working/ReleaseNotes.md", "### New in 1.2.3");
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = context.ParseReleaseNotes("ReleaseNotes.md");

                // Then
                Assert.Equal("1.2.3", result.Version);
            }
        }
    }
}
