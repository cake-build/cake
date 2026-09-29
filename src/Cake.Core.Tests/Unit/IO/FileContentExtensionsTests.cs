// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core.IO;
using Cake.Testing;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.IO;

public sealed class FileContentExtensionsTests
{
    private static FakeFileSystem CreateFileSystem()
        => new FakeFileSystem(FakeEnvironment.CreateUnixEnvironment());

    private static FakeFile CreateExistingFile(FakeFileSystem fileSystem, string path, string content = "Hello World")
        => fileSystem.CreateFile(path).SetContent(content);

    public sealed class TheWriteAllTextMethod
    {
        [Fact]
        public void Should_Throw_If_File_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => ((IFile)null).WriteAllText("content"));

            // Then
            AssertEx.IsArgumentNullException(result, "file");
        }

        [Fact]
        public void Should_Create_File_And_Write_Text()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("out.txt");

            // When
            file.WriteAllText("Hello World");

            // Then
            Assert.True(file.Exists);
            Assert.Equal("Hello World", file.GetTextContent());
            Assert.False(file.HasUTF8BOM());
        }

        [Fact]
        public void Should_Replace_Existing_Content()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "out.txt", "old");

            // When
            file.WriteAllText("new");

            // Then
            Assert.Equal("new", file.GetTextContent());
        }

        [Fact]
        public void Should_Throw_If_Encoding_Is_Null()
        {
            // Given
            var file = CreateFileSystem().GetFile("out.txt");

            // When
            var result = Record.Exception(() => file.WriteAllText("content", null));

            // Then
            AssertEx.IsArgumentNullException(result, "encoding");
        }

        [Fact]
        public void Should_Treat_Null_Text_As_Empty()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "out.txt", "old");

            // When
            file.WriteAllText((string)null);

            // Then
            Assert.Equal(string.Empty, file.GetTextContent());
        }

        [Fact]
        public void Should_Write_Using_Specified_Encoding()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("out.txt");
            const string contents = "Åäö";

            // When
            file.WriteAllText(contents, Encoding.Unicode);

            // Then
            Assert.Equal(contents, file.GetTextContent(Encoding.Unicode));
        }
    }

    public sealed class TheAppendAllTextMethod
    {
        [Fact]
        public void Should_Throw_If_File_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => ((IFile)null).AppendAllText("content"));

            // Then
            AssertEx.IsArgumentNullException(result, "file");
        }

        [Fact]
        public void Should_Create_File_When_Missing()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("out.txt");

            // When
            file.AppendAllText("Hello");

            // Then
            Assert.True(file.Exists);
            Assert.Equal("Hello", file.GetTextContent());
        }

        [Fact]
        public void Should_Append_To_Existing_Content()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "out.txt", "Hello");

            // When
            file.AppendAllText(" World");

            // Then
            Assert.Equal("Hello World", file.GetTextContent());
        }
    }

    public sealed class TheReadAllTextMethod
    {
        [Fact]
        public void Should_Throw_If_File_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => ((IFile)null).ReadAllText());

            // Then
            AssertEx.IsArgumentNullException(result, "file");
        }

        [Fact]
        public void Should_Throw_If_File_Does_Not_Exist()
        {
            // Given
            var file = CreateFileSystem().GetFile("missing.txt");

            // When
            var result = Record.Exception(() => file.ReadAllText());

            // Then
            Assert.IsType<FileNotFoundException>(result);
        }

        [Fact]
        public void Should_Read_Existing_Content()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "text.txt", "Hello World");

            // When
            var result = file.ReadAllText();

            // Then
            Assert.Equal("Hello World", result);
        }
    }

    public sealed class TheWriteAndReadAllBytesMethods
    {
        [Fact]
        public void Should_Throw_If_Bytes_Are_Null()
        {
            // Given
            var file = CreateFileSystem().GetFile("data.bin");

            // When
            var result = Record.Exception(() => file.WriteAllBytes((byte[])null));

            // Then
            AssertEx.IsArgumentNullException(result, "bytes");
        }

        [Fact]
        public void Should_Roundtrip_Bytes()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("data.bin");
            byte[] bytes = [1, 2, 3, 4];

            // When
            file.WriteAllBytes(bytes);
            var result = file.ReadAllBytes();

            // Then
            Assert.Equal(bytes, result);
        }

        [Fact]
        public void Should_Return_Empty_Array_For_Empty_File()
        {
            // Given
            var file = CreateFileSystem().CreateFile("empty.bin");

            // When
            var result = file.ReadAllBytes();

            // Then
            Assert.Empty(result);
        }

        [Fact]
        public void Should_Append_Bytes()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("data.bin");

            // When
            file.WriteAllBytes([1, 2]);
            file.AppendAllBytes([3, 4]);

            // Then
            Assert.Equal(new byte[] { 1, 2, 3, 4 }, file.ReadAllBytes());
        }
    }

    public sealed class TheWriteAndReadAllLinesMethods
    {
        [Fact]
        public void Should_Throw_If_Lines_Are_Null()
        {
            // Given
            var file = CreateFileSystem().GetFile("lines.txt");

            // When
            var result = Record.Exception(() => file.WriteAllLines(null));

            // Then
            AssertEx.IsArgumentNullException(result, "contents");
        }

        [Fact]
        public void Should_Roundtrip_Lines()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("lines.txt");
            string[] lines = ["one", "two", "three"];

            // When
            file.WriteAllLines(lines);
            var result = file.ReadAllLines();

            // Then
            Assert.Equal(lines, result);
        }

        [Fact]
        public void Should_Append_Lines()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("lines.txt");

            // When
            file.WriteAllLines(["one"]);
            file.AppendAllLines(["two"]);

            // Then
            Assert.Equal(["one", "two"], file.ReadAllLines());
        }
    }

    public sealed class TheReadLinesMethod
    {
        [Fact]
        public void Should_Throw_If_File_Is_Null()
        {
            // Given, When
            var result = Record.Exception(() => ((IFile)null).ReadLines());

            // Then
            AssertEx.IsArgumentNullException(result, "file");
        }

        [Fact]
        public void Should_Not_Open_File_Until_Enumerated()
        {
            // Given
            var file = Substitute.For<IFile>();

            // When
            var lines = file.ReadLines();

            // Then
            file.DidNotReceive().Open(Arg.Any<FileMode>(), Arg.Any<FileAccess>(), Arg.Any<FileShare>());
            _ = lines;
        }

        [Fact]
        public void Should_Enumerate_Lines_Lazily()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "text.txt", "1\n2\n3");

            // When
            var result = file.ReadLines().ToList();

            // Then
            Assert.Equal(["1", "2", "3"], result);
        }
    }

    public sealed class TheCreateTextOpenTextAndAppendTextMethods
    {
        [Fact]
        public void CreateText_Should_Replace_Existing_Content_Without_Bom()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "out.txt", "old");

            // When
            using (var writer = file.CreateText())
            {
                writer.Write("new");
            }

            // Then
            Assert.Equal("new", file.GetTextContent());
            Assert.False(file.HasUTF8BOM());
        }

        [Fact]
        public void OpenText_Should_Read_Existing_Content_And_Detect_Bom()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var preamble = Encoding.UTF8.GetPreamble();
            var payload = Encoding.UTF8.GetBytes("Hello");
            var content = new byte[preamble.Length + payload.Length];
            Buffer.BlockCopy(preamble, 0, content, 0, preamble.Length);
            Buffer.BlockCopy(payload, 0, content, preamble.Length, payload.Length);
            var file = fileSystem.CreateFile("text.txt", content);

            // When
            using var reader = file.OpenText();
            var result = reader.ReadToEnd();

            // Then
            Assert.Equal("Hello", result);
            Assert.True(file.HasUTF8BOM());
        }

        [Fact]
        public void AppendText_Should_Append_To_Existing_Content()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "out.txt", "Hello");

            // When
            using (var writer = file.AppendText())
            {
                writer.Write(" World");
            }

            // Then
            Assert.Equal("Hello World", file.GetTextContent());
        }
    }

    public sealed class TheAsyncMethods
    {
        [Fact]
        public async Task Should_Roundtrip_Text_Asynchronously()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("out.txt");

            // When
            await file.WriteAllTextAsync("Hello World", TestContext.Current.CancellationToken);
            var result = await file.ReadAllTextAsync(TestContext.Current.CancellationToken);

            // Then
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public async Task Should_Roundtrip_Bytes_Asynchronously()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("data.bin");
            byte[] bytes = [9, 8, 7];

            // When
            await file.WriteAllBytesAsync(bytes, TestContext.Current.CancellationToken);
            await file.AppendAllBytesAsync([6], TestContext.Current.CancellationToken);
            var result = await file.ReadAllBytesAsync(TestContext.Current.CancellationToken);

            // Then
            Assert.Equal(new byte[] { 9, 8, 7, 6 }, result);
        }

        [Fact]
        public async Task Should_Roundtrip_Lines_Asynchronously()
        {
            // Given
            var fileSystem = CreateFileSystem();
            var file = fileSystem.GetFile("lines.txt");

            // When
            await file.WriteAllLinesAsync(["a", "b"], TestContext.Current.CancellationToken);
            await file.AppendAllLinesAsync(["c"], TestContext.Current.CancellationToken);
            var result = await file.ReadAllLinesAsync(TestContext.Current.CancellationToken);

            // Then
            Assert.Equal(["a", "b", "c"], result);
        }

        [Fact]
        public async Task Should_Enumerate_Lines_Asynchronously()
        {
            // Given
            var file = CreateExistingFile(CreateFileSystem(), "text.txt", "1\n2\n3");
            var result = new List<string>();

            // When
            await foreach (var line in file.ReadLinesAsync(TestContext.Current.CancellationToken))
            {
                result.Add(line);
            }

            // Then
            Assert.Equal(["1", "2", "3"], result);
        }

        [Fact]
        public async Task Should_Throw_If_Cancellation_Token_Is_Already_Canceled()
        {
            // Given
            var file = Substitute.For<IFile>();
            using var cts = new CancellationTokenSource();
            await cts.CancelAsync();

            // When
            var result = await Record.ExceptionAsync(() => file.ReadAllTextAsync(cts.Token));

            // Then
            Assert.IsType<OperationCanceledException>(result);
            file.DidNotReceive().Open(Arg.Any<FileMode>(), Arg.Any<FileAccess>(), Arg.Any<FileShare>());
        }
    }
}
