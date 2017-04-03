// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ReleaseNotesAliasesTests
    {
        public sealed class TheParseAllReleaseNotesMethod
        {
            [Fact]
            public void Should_Throw_If_File_Path_Is_Null()
            {
                // Given
                var context = Substitute.For<ICakeContext>();

                // When
                var result = Record.Exception(() => context.ParseAllReleaseNotes(null));

                // Then
                AssertEx.IsArgumentNullException(result, "filePath");
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
                var result = Record.Exception(() => context.ParseAllReleaseNotes("ReleaseNotes.md"));

                // Then
                Assert.IsType<CakeException>(result);
                Assert.Equal("Release notes file '/Working/ReleaseNotes.md' does not exist.", result?.Message);
            }

            [Fact]
            public void Should_Read_Content_Of_File_And_Parse_It()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/ReleaseNotes.md").SetContent("### New in 1.2.3");
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = context.ParseAllReleaseNotes("ReleaseNotes.md");

                // Then
                Assert.Equal("1.2.3", result[0].Version.ToString());
            }
        }

        public sealed class TheParseReleaseNotesMethod
        {
            [Fact]
            public void Should_Return_The_Latest_Release_Notes()
            {
                // Given
                var context = Substitute.For<ICakeContext>();
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                fileSystem.CreateFile("/Working/ReleaseNotes.md").SetContent("* 1.2.3 - Line 1\n* 1.2.5 Line 2\n* 1.2.4 Line 3");
                context.FileSystem.Returns(fileSystem);
                context.Environment.Returns(environment);

                // When
                var result = context.ParseReleaseNotes("ReleaseNotes.md");

                // Then
                Assert.Equal("1.2.5", result.Version.ToString());
            }
        }
    }
}