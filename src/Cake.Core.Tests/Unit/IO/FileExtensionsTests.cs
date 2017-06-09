// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Text;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO
{
    public sealed class FileExtensionsTests
    {
        public sealed class TheOpenMethod
        {
            public sealed class WithFileMode
            {
                [Fact]
                public void Should_Throw_If_File_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() => FileExtensions.Open(null, FileMode.Create));

                    // Then
                    AssertEx.IsArgumentNullException(result, "file");
                }

                [Theory]
                [InlineData(FileMode.Append, FileAccess.Write)]
                [InlineData(FileMode.Create, FileAccess.ReadWrite)]
                [InlineData(FileMode.CreateNew, FileAccess.ReadWrite)]
                [InlineData(FileMode.Open, FileAccess.ReadWrite)]
                [InlineData(FileMode.OpenOrCreate, FileAccess.ReadWrite)]
                [InlineData(FileMode.Truncate, FileAccess.ReadWrite)]
                public void Should_Open_With_Specified_File_Mode_And_Infer_File_Access(FileMode mode, FileAccess access)
                {
                    // Given
                    var file = Substitute.For<IFile>();

                    // When
                    file.Open(mode);

                    // Then
                    file.Received(1).Open(mode, access, FileShare.None);
                }
            }

            public sealed class WithFileModeAndFileAccess
            {
                [Fact]
                public void Should_Throw_If_File_Is_Null()
                {
                    // Given, When
                    var result = Record.Exception(() => FileExtensions.Open(null, FileMode.Create, FileAccess.Write));

                    // Then
                    AssertEx.IsArgumentNullException(result, "file");
                }

                [Theory]
                [InlineData(FileMode.Append, FileAccess.Write)]
                [InlineData(FileMode.Create, FileAccess.ReadWrite)]
                [InlineData(FileMode.CreateNew, FileAccess.ReadWrite)]
                [InlineData(FileMode.Open, FileAccess.ReadWrite)]
                [InlineData(FileMode.OpenOrCreate, FileAccess.ReadWrite)]
                [InlineData(FileMode.Truncate, FileAccess.ReadWrite)]
                public void Should_Open_With_Specified_File_Mode_And_Infer_File_Access(FileMode mode, FileAccess access)
                {
                    // Given
                    var file = Substitute.For<IFile>();

                    // When
                    file.Open(mode, access);

                    // Then
                    file.Received(1).Open(mode, access, FileShare.None);
                }
            }
        }

        public sealed class TheOpenReadMethod
        {
            [Fact]
            public void Should_Throw_If_File_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => FileExtensions.OpenRead(null));

                // Then
                AssertEx.IsArgumentNullException(result, "file");
            }

            [Fact]
            public void Should_Open_Stream_With_Expected_FileMode_And_FileAccess()
            {
                // Given
                var file = Substitute.For<IFile>();

                // When
                file.OpenRead();

                // Then
                file.Received(1).Open(FileMode.Open, FileAccess.Read, FileShare.Read);
            }
        }

        public sealed class TheOpenWriteMethod
        {
            [Fact]
            public void Should_Throw_If_File_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => FileExtensions.OpenWrite(null));

                // Then
                AssertEx.IsArgumentNullException(result, "file");
            }

            [Fact]
            public void Should_Open_Stream_With_Expected_FileMode_And_FileAccess()
            {
                // Given
                var file = Substitute.For<IFile>();

                // When
                file.OpenWrite();

                // Then
                file.Received(1).Open(FileMode.Create, FileAccess.Write, FileShare.None);
            }
        }

        public sealed class TheReadLinesMethod
        {
            [Fact]
            public void Should_Throw_If_File_Is_Null()
            {
                // Given, When
                var result = Record.Exception(() => FileExtensions.ReadLines(null, Encoding.UTF8));

                // Then
                AssertEx.IsArgumentNullException(result, "file");
            }

            [Fact]
            public void Should_Return_Empty_List_If_File_Contains_No_Lines()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var file = fileSystem.CreateFile("text.txt");

                // When
                var result = file.ReadLines(Encoding.UTF8).ToList();

                // Then
                Assert.Equal(0, result.Count);
            }

            [Fact]
            public void Should_Read_File_With_Single_Line_Correctly()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var file = fileSystem.CreateFile("text.txt").SetContent("Hello World");

                // When
                var result = file.ReadLines(Encoding.UTF8).ToList();

                // Then
                Assert.Equal(1, result.Count);
            }

            [Fact]
            public void Should_Read_File_With_Multiple_Lines_Correctly()
            {
                // Given
                var environment = FakeEnvironment.CreateUnixEnvironment();
                var fileSystem = new FakeFileSystem(environment);
                var content = new StringBuilder();
                content.AppendLine("1");
                content.AppendLine("2");
                content.AppendLine("3");
                var file = fileSystem.CreateFile("text.txt").SetContent(content.ToString());

                // When
                var result = file.ReadLines(Encoding.UTF8).ToList();

                // Then
                Assert.Equal(3, result.Count);
                Assert.Equal("1", result[0]);
                Assert.Equal("2", result[1]);
                Assert.Equal("3", result[2]);
            }
        }
    }
}