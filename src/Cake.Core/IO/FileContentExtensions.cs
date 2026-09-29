// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cake.Core.IO;

/// <summary>
/// Contains content read and write extension methods for <see cref="IFile"/>.
/// </summary>
public static class FileContentExtensions
{
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    extension(IFile file)
    {
        /// <summary>
        /// Appends the specified bytes to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="bytes">The bytes to append.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// file.AppendAllBytes(new byte[] { 1, 2, 3 });
        /// </code>
        /// </example>
        public void AppendAllBytes(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(bytes);
            AppendAllBytesCore(file, bytes.AsSpan());
        }

        /// <summary>
        /// Appends the specified bytes to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="bytes">The bytes to append.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// file.AppendAllBytes([1, 2, 3]);
        /// </code>
        /// </example>
        public void AppendAllBytes(ReadOnlySpan<byte> bytes)
        {
            ArgumentNullException.ThrowIfNull(file);
            AppendAllBytesCore(file, bytes);
        }

        /// <summary>
        /// Asynchronously appends the specified bytes to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="bytes">The bytes to append.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.AppendAllBytesAsync(new byte[] { 1, 2, 3 });
        /// </code>
        /// </example>
        public Task AppendAllBytesAsync(byte[] bytes)
            => file.AppendAllBytesAsync(bytes, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified bytes to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="bytes">The bytes to append.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.AppendAllBytesAsync(new byte[] { 1, 2, 3 }, cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllBytesAsync(byte[] bytes, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(bytes);
            cancellationToken.ThrowIfCancellationRequested();
            return AppendAllBytesAsyncCore(file, bytes.AsMemory(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously appends the specified bytes to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="bytes">The bytes to append.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.AppendAllBytesAsync(new ReadOnlyMemory&lt;byte&gt;(new byte[] { 1, 2, 3 }));
        /// </code>
        /// </example>
        public Task AppendAllBytesAsync(ReadOnlyMemory<byte> bytes)
            => file.AppendAllBytesAsync(bytes, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified bytes to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="bytes">The bytes to append.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.AppendAllBytesAsync(new ReadOnlyMemory&lt;byte&gt;(new byte[] { 1, 2, 3 }), cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllBytesAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            cancellationToken.ThrowIfCancellationRequested();
            return AppendAllBytesAsyncCore(file, bytes, cancellationToken);
        }

        /// <summary>
        /// Appends the specified lines to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The lines to append.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// file.AppendAllLines(new[] { "first", "second" });
        /// </code>
        /// </example>
        public void AppendAllLines(IEnumerable<string> contents)
            => file.AppendAllLines(contents, Utf8NoBom);

        /// <summary>
        /// Appends the specified lines to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The lines to append.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// file.AppendAllLines(new[] { "first", "second" }, System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public void AppendAllLines(IEnumerable<string> contents, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(contents);
            ArgumentNullException.ThrowIfNull(encoding);
            AppendAllLinesCore(file, contents, encoding);
        }

        /// <summary>
        /// Asynchronously appends the specified lines to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The lines to append.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// await file.AppendAllLinesAsync(new[] { "first", "second" });
        /// </code>
        /// </example>
        public Task AppendAllLinesAsync(IEnumerable<string> contents)
            => file.AppendAllLinesAsync(contents, Utf8NoBom, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified lines to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The lines to append.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// await file.AppendAllLinesAsync(new[] { "first", "second" }, cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllLinesAsync(IEnumerable<string> contents, CancellationToken cancellationToken)
            => file.AppendAllLinesAsync(contents, Utf8NoBom, cancellationToken);

        /// <summary>
        /// Asynchronously appends the specified lines to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The lines to append.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// await file.AppendAllLinesAsync(new[] { "first", "second" }, System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public Task AppendAllLinesAsync(IEnumerable<string> contents, Encoding encoding)
            => file.AppendAllLinesAsync(contents, encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified lines to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The lines to append.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// await file.AppendAllLinesAsync(new[] { "first", "second" }, System.Text.Encoding.UTF8, cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllLinesAsync(IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(contents);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return AppendAllLinesAsyncCore(file, contents, encoding, cancellationToken);
        }

        /// <summary>
        /// Appends the specified text to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append. A <c>null</c> value is treated as empty.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.AppendAllText("Hello World");
        /// </code>
        /// </example>
        public void AppendAllText(string contents)
            => file.AppendAllText((contents ?? string.Empty).AsSpan(), Utf8NoBom);

        /// <summary>
        /// Appends the specified text to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append. A <c>null</c> value is treated as empty.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.AppendAllText("Hello World", System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public void AppendAllText(string contents, Encoding encoding)
            => file.AppendAllText((contents ?? string.Empty).AsSpan(), encoding);

        /// <summary>
        /// Appends the specified text to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.AppendAllText("Hello World".AsSpan());
        /// </code>
        /// </example>
        public void AppendAllText(ReadOnlySpan<char> contents)
            => file.AppendAllText(contents, Utf8NoBom);

        /// <summary>
        /// Appends the specified text to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.AppendAllText("Hello World".AsSpan(), System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public void AppendAllText(ReadOnlySpan<char> contents, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            AppendAllTextCore(file, contents, encoding);
        }

        /// <summary>
        /// Asynchronously appends the specified text to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append. A <c>null</c> value is treated as empty.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World");
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(string contents)
            => file.AppendAllTextAsync(contents, Utf8NoBom, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified text to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append. A <c>null</c> value is treated as empty.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World", cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(string contents, CancellationToken cancellationToken)
            => file.AppendAllTextAsync(contents, Utf8NoBom, cancellationToken);

        /// <summary>
        /// Asynchronously appends the specified text to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append. A <c>null</c> value is treated as empty.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World", System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(string contents, Encoding encoding)
            => file.AppendAllTextAsync(contents, encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified text to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append. A <c>null</c> value is treated as empty.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World", System.Text.Encoding.UTF8, cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(string contents, Encoding encoding, CancellationToken cancellationToken)
            => file.AppendAllTextAsync((contents ?? string.Empty).AsMemory(), encoding, cancellationToken);

        /// <summary>
        /// Asynchronously appends the specified text to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World".AsMemory());
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(ReadOnlyMemory<char> contents)
            => file.AppendAllTextAsync(contents, Utf8NoBom, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified text to the file using UTF-8 encoding without a byte order mark.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World".AsMemory(), cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(ReadOnlyMemory<char> contents, CancellationToken cancellationToken)
            => file.AppendAllTextAsync(contents, Utf8NoBom, cancellationToken);

        /// <summary>
        /// Asynchronously appends the specified text to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World".AsMemory(), System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(ReadOnlyMemory<char> contents, Encoding encoding)
            => file.AppendAllTextAsync(contents, encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously appends the specified text to the file using the specified encoding.
        /// The file is created if it does not exist.
        /// </summary>
        /// <param name="contents">The text to append.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous append operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.AppendAllTextAsync("Hello World".AsMemory(), System.Text.Encoding.UTF8, cancellationToken);
        /// </code>
        /// </example>
        public Task AppendAllTextAsync(ReadOnlyMemory<char> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return AppendAllTextAsyncCore(file, contents, encoding, cancellationToken);
        }

        /// <summary>
        /// Creates a <see cref="StreamWriter"/> that appends UTF-8 encoded text to the file.
        /// The file is created if it does not exist.
        /// </summary>
        /// <returns>A <see cref="StreamWriter"/> that writes to the file. The caller owns the writer.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/log.txt");
        /// using (var writer = file.AppendText())
        /// {
        ///     writer.WriteLine("Hello World");
        /// }
        /// </code>
        /// </example>
        public StreamWriter AppendText()
        {
            ArgumentNullException.ThrowIfNull(file);
            return CreateOwnedWriter(file.OpenAppend(), Utf8NoBom);
        }

        /// <summary>
        /// Creates or replaces the file and returns a <see cref="StreamWriter"/> that writes UTF-8 encoded text.
        /// </summary>
        /// <returns>A <see cref="StreamWriter"/> that writes to the file. The caller owns the writer.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// using (var writer = file.CreateText())
        /// {
        ///     writer.WriteLine("Hello World");
        /// }
        /// </code>
        /// </example>
        public StreamWriter CreateText()
        {
            ArgumentNullException.ThrowIfNull(file);
            return CreateOwnedWriter(OpenForWrite(file), Utf8NoBom);
        }

        /// <summary>
        /// Opens an existing UTF-8 encoded text file for reading.
        /// </summary>
        /// <returns>A <see cref="StreamReader"/> that reads from the file. The caller owns the reader.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// using (var reader = file.OpenText())
        /// {
        ///     Information(reader.ReadToEnd());
        /// }
        /// </code>
        /// </example>
        public StreamReader OpenText()
        {
            ArgumentNullException.ThrowIfNull(file);
            return CreateOwnedReader(file.OpenRead());
        }

        /// <summary>
        /// Opens a file, reads all bytes from the file, and then closes the file.
        /// </summary>
        /// <returns>A byte array containing the contents of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// var bytes = file.ReadAllBytes();
        /// Information("Read {0} bytes", bytes.Length);
        /// </code>
        /// </example>
        public byte[] ReadAllBytes()
        {
            ArgumentNullException.ThrowIfNull(file);
            return ReadAllBytesCore(file);
        }

        /// <summary>
        /// Asynchronously opens a file, reads all bytes from the file, and then closes the file.
        /// </summary>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// var bytes = await file.ReadAllBytesAsync();
        /// Information("Read {0} bytes", bytes.Length);
        /// </code>
        /// </example>
        public Task<byte[]> ReadAllBytesAsync()
            => file.ReadAllBytesAsync(CancellationToken.None);

        /// <summary>
        /// Asynchronously opens a file, reads all bytes from the file, and then closes the file.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// var bytes = await file.ReadAllBytesAsync(cancellationToken);
        /// Information("Read {0} bytes", bytes.Length);
        /// </code>
        /// </example>
        public Task<byte[]> ReadAllBytesAsync(CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            cancellationToken.ThrowIfCancellationRequested();
            return ReadAllBytesAsyncCore(file, cancellationToken);
        }

        /// <summary>
        /// Opens a file, reads all lines with UTF-8 encoding, and then closes the file.
        /// </summary>
        /// <returns>A string array containing all lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in file.ReadAllLines())
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public string[] ReadAllLines()
            => file.ReadAllLines(Encoding.UTF8);

        /// <summary>
        /// Opens a file, reads all lines with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A string array containing all lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in file.ReadAllLines(System.Text.Encoding.UTF8))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public string[] ReadAllLines(Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            return ReadAllLinesCore(file, encoding);
        }

        /// <summary>
        /// Asynchronously opens a file, reads all lines with UTF-8 encoding, and then closes the file.
        /// </summary>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in await file.ReadAllLinesAsync())
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public Task<string[]> ReadAllLinesAsync()
            => file.ReadAllLinesAsync(Encoding.UTF8, CancellationToken.None);

        /// <summary>
        /// Asynchronously opens a file, reads all lines with UTF-8 encoding, and then closes the file.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in await file.ReadAllLinesAsync(cancellationToken))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public Task<string[]> ReadAllLinesAsync(CancellationToken cancellationToken)
            => file.ReadAllLinesAsync(Encoding.UTF8, cancellationToken);

        /// <summary>
        /// Asynchronously opens a file, reads all lines with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in await file.ReadAllLinesAsync(System.Text.Encoding.UTF8))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public Task<string[]> ReadAllLinesAsync(Encoding encoding)
            => file.ReadAllLinesAsync(encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously opens a file, reads all lines with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in await file.ReadAllLinesAsync(System.Text.Encoding.UTF8, cancellationToken))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public Task<string[]> ReadAllLinesAsync(Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return ReadAllLinesAsyncCore(file, encoding, cancellationToken);
        }

        /// <summary>
        /// Opens a file, reads all text with UTF-8 encoding, and then closes the file.
        /// </summary>
        /// <returns>A string containing all text in the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// Information(file.ReadAllText());
        /// </code>
        /// </example>
        public string ReadAllText()
            => file.ReadAllText(Encoding.UTF8);

        /// <summary>
        /// Opens a file, reads all text with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A string containing all text in the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// Information(file.ReadAllText(System.Text.Encoding.UTF8));
        /// </code>
        /// </example>
        public string ReadAllText(Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            return ReadAllTextCore(file, encoding);
        }

        /// <summary>
        /// Asynchronously opens a file, reads all text with UTF-8 encoding, and then closes the file.
        /// </summary>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// Information(await file.ReadAllTextAsync());
        /// </code>
        /// </example>
        public Task<string> ReadAllTextAsync()
            => file.ReadAllTextAsync(Encoding.UTF8, CancellationToken.None);

        /// <summary>
        /// Asynchronously opens a file, reads all text with UTF-8 encoding, and then closes the file.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// Information(await file.ReadAllTextAsync(cancellationToken));
        /// </code>
        /// </example>
        public Task<string> ReadAllTextAsync(CancellationToken cancellationToken)
            => file.ReadAllTextAsync(Encoding.UTF8, cancellationToken);

        /// <summary>
        /// Asynchronously opens a file, reads all text with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// Information(await file.ReadAllTextAsync(System.Text.Encoding.UTF8));
        /// </code>
        /// </example>
        public Task<string> ReadAllTextAsync(Encoding encoding)
            => file.ReadAllTextAsync(encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously opens a file, reads all text with the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous read operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// Information(await file.ReadAllTextAsync(System.Text.Encoding.UTF8, cancellationToken));
        /// </code>
        /// </example>
        public Task<string> ReadAllTextAsync(Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return ReadAllTextAsyncCore(file, encoding, cancellationToken);
        }

        /// <summary>
        /// Lazily enumerates the lines of a file using UTF-8 encoding.
        /// The file is opened when enumeration starts and closed when enumeration ends.
        /// </summary>
        /// <returns>The lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// foreach (var line in file.ReadLines())
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public IEnumerable<string> ReadLines()
        {
            ArgumentNullException.ThrowIfNull(file);
            return ReadLinesCore(file, Encoding.UTF8);
        }

        /// <summary>
        /// Lazily enumerates the lines of a file using UTF-8 encoding.
        /// The file is opened when enumeration starts and closed when enumeration ends.
        /// </summary>
        /// <returns>The lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// await foreach (var line in file.ReadLinesAsync())
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public IAsyncEnumerable<string> ReadLinesAsync()
            => file.ReadLinesAsync(Encoding.UTF8, CancellationToken.None);

        /// <summary>
        /// Lazily enumerates the lines of a file using UTF-8 encoding.
        /// The file is opened when enumeration starts and closed when enumeration ends.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// await foreach (var line in file.ReadLinesAsync(cancellationToken))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public IAsyncEnumerable<string> ReadLinesAsync(CancellationToken cancellationToken)
            => file.ReadLinesAsync(Encoding.UTF8, cancellationToken);

        /// <summary>
        /// Lazily enumerates the lines of a file using the specified encoding.
        /// The file is opened when enumeration starts and closed when enumeration ends.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>The lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// await foreach (var line in file.ReadLinesAsync(System.Text.Encoding.UTF8))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public IAsyncEnumerable<string> ReadLinesAsync(Encoding encoding)
            => file.ReadLinesAsync(encoding, CancellationToken.None);

        /// <summary>
        /// Lazily enumerates the lines of a file using the specified encoding.
        /// The file is opened when enumeration starts and closed when enumeration ends.
        /// </summary>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The lines of the file.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./README.md");
        /// await foreach (var line in file.ReadLinesAsync(System.Text.Encoding.UTF8, cancellationToken))
        /// {
        ///     Information(line);
        /// }
        /// </code>
        /// </example>
        public IAsyncEnumerable<string> ReadLinesAsync(Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return ReadLinesAsyncCore(file, encoding, cancellationToken);
        }

        /// <summary>
        /// Creates a new file, writes the specified bytes to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// file.WriteAllBytes(new byte[] { 1, 2, 3 });
        /// </code>
        /// </example>
        public void WriteAllBytes(byte[] bytes)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(bytes);
            WriteAllBytesCore(file, bytes.AsSpan());
        }

        /// <summary>
        /// Creates a new file, writes the specified bytes to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// file.WriteAllBytes([1, 2, 3]);
        /// </code>
        /// </example>
        public void WriteAllBytes(ReadOnlySpan<byte> bytes)
        {
            ArgumentNullException.ThrowIfNull(file);
            WriteAllBytesCore(file, bytes);
        }

        /// <summary>
        /// Asynchronously creates a new file, writes the specified bytes to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.WriteAllBytesAsync(new byte[] { 1, 2, 3 });
        /// </code>
        /// </example>
        public Task WriteAllBytesAsync(byte[] bytes)
            => file.WriteAllBytesAsync(bytes, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified bytes to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.WriteAllBytesAsync(new byte[] { 1, 2, 3 }, cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllBytesAsync(byte[] bytes, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(bytes);
            cancellationToken.ThrowIfCancellationRequested();
            return WriteAllBytesAsyncCore(file, bytes.AsMemory(), cancellationToken);
        }

        /// <summary>
        /// Asynchronously creates a new file, writes the specified bytes to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.WriteAllBytesAsync(new ReadOnlyMemory&lt;byte&gt;(new byte[] { 1, 2, 3 }));
        /// </code>
        /// </example>
        public Task WriteAllBytesAsync(ReadOnlyMemory<byte> bytes)
            => file.WriteAllBytesAsync(bytes, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified bytes to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="bytes">The bytes to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/data.bin");
        /// await file.WriteAllBytesAsync(new ReadOnlyMemory&lt;byte&gt;(new byte[] { 1, 2, 3 }), cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllBytesAsync(ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            cancellationToken.ThrowIfCancellationRequested();
            return WriteAllBytesAsyncCore(file, bytes, cancellationToken);
        }

        /// <summary>
        /// Creates a new file, writes the specified lines to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The lines to write.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.WriteAllLines(new[] { "first", "second" });
        /// </code>
        /// </example>
        public void WriteAllLines(IEnumerable<string> contents)
            => file.WriteAllLines(contents, Utf8NoBom);

        /// <summary>
        /// Creates a new file, writes the specified lines to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The lines to write.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.WriteAllLines(new[] { "first", "second" }, System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public void WriteAllLines(IEnumerable<string> contents, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(contents);
            ArgumentNullException.ThrowIfNull(encoding);
            WriteAllLinesCore(file, contents, encoding);
        }

        /// <summary>
        /// Asynchronously creates a new file, writes the specified lines to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The lines to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllLinesAsync(new[] { "first", "second" });
        /// </code>
        /// </example>
        public Task WriteAllLinesAsync(IEnumerable<string> contents)
            => file.WriteAllLinesAsync(contents, Utf8NoBom, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified lines to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The lines to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllLinesAsync(new[] { "first", "second" }, cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllLinesAsync(IEnumerable<string> contents, CancellationToken cancellationToken)
            => file.WriteAllLinesAsync(contents, Utf8NoBom, cancellationToken);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified lines to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The lines to write.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllLinesAsync(new[] { "first", "second" }, System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public Task WriteAllLinesAsync(IEnumerable<string> contents, Encoding encoding)
            => file.WriteAllLinesAsync(contents, encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified lines to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The lines to write.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllLinesAsync(new[] { "first", "second" }, System.Text.Encoding.UTF8, cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllLinesAsync(IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(contents);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return WriteAllLinesAsyncCore(file, contents, encoding, cancellationToken);
        }

        /// <summary>
        /// Creates a new file, writes the specified text to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write. A <c>null</c> value is treated as empty.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.WriteAllText("Hello World");
        /// </code>
        /// </example>
        public void WriteAllText(string contents)
            => file.WriteAllText((contents ?? string.Empty).AsSpan(), Utf8NoBom);

        /// <summary>
        /// Creates a new file, writes the specified text to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write. A <c>null</c> value is treated as empty.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.WriteAllText("Hello World", System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public void WriteAllText(string contents, Encoding encoding)
            => file.WriteAllText((contents ?? string.Empty).AsSpan(), encoding);

        /// <summary>
        /// Creates a new file, writes the specified text to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.WriteAllText("Hello World".AsSpan());
        /// </code>
        /// </example>
        public void WriteAllText(ReadOnlySpan<char> contents)
            => file.WriteAllText(contents, Utf8NoBom);

        /// <summary>
        /// Creates a new file, writes the specified text to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// file.WriteAllText("Hello World".AsSpan(), System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public void WriteAllText(ReadOnlySpan<char> contents, Encoding encoding)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            WriteAllTextCore(file, contents, encoding);
        }

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write. A <c>null</c> value is treated as empty.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World");
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(string contents)
            => file.WriteAllTextAsync(contents, Utf8NoBom, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write. A <c>null</c> value is treated as empty.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World", cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(string contents, CancellationToken cancellationToken)
            => file.WriteAllTextAsync(contents, Utf8NoBom, cancellationToken);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write. A <c>null</c> value is treated as empty.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World", System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(string contents, Encoding encoding)
            => file.WriteAllTextAsync(contents, encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write. A <c>null</c> value is treated as empty.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World", System.Text.Encoding.UTF8, cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(string contents, Encoding encoding, CancellationToken cancellationToken)
            => file.WriteAllTextAsync((contents ?? string.Empty).AsMemory(), encoding, cancellationToken);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World".AsMemory());
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(ReadOnlyMemory<char> contents)
            => file.WriteAllTextAsync(contents, Utf8NoBom, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using UTF-8 encoding without a byte order mark, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World".AsMemory(), cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(ReadOnlyMemory<char> contents, CancellationToken cancellationToken)
            => file.WriteAllTextAsync(contents, Utf8NoBom, cancellationToken);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World".AsMemory(), System.Text.Encoding.UTF8);
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(ReadOnlyMemory<char> contents, Encoding encoding)
            => file.WriteAllTextAsync(contents, encoding, CancellationToken.None);

        /// <summary>
        /// Asynchronously creates a new file, writes the specified text to the file using the specified encoding, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="contents">The text to write.</param>
        /// <param name="encoding">The encoding applied to the file.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        /// <example>
        /// <code>
        /// var file = context.FileSystem.GetFile("./artifacts/out.txt");
        /// await file.WriteAllTextAsync("Hello World".AsMemory(), System.Text.Encoding.UTF8, cancellationToken);
        /// </code>
        /// </example>
        public Task WriteAllTextAsync(ReadOnlyMemory<char> contents, Encoding encoding, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(file);
            ArgumentNullException.ThrowIfNull(encoding);
            cancellationToken.ThrowIfCancellationRequested();
            return WriteAllTextAsyncCore(file, contents, encoding, cancellationToken);
        }
    }

    private static Stream OpenForWrite(IFile file)
        => file.Open(FileMode.Create, FileAccess.Write, FileShare.Read);

    private static StreamWriter CreateOwnedWriter(Stream stream, Encoding encoding)
    {
        try
        {
            return new StreamWriter(stream, encoding);
        }
        catch
        {
            stream.Dispose();
            throw;
        }
    }

    private static StreamReader CreateOwnedReader(Stream stream)
    {
        try
        {
            return new StreamReader(stream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        }
        catch
        {
            stream.Dispose();
            throw;
        }
    }

    private static void AppendAllBytesCore(IFile file, ReadOnlySpan<byte> bytes)
    {
        using var stream = file.OpenAppend();
        stream.Write(bytes);
    }

    private static async Task AppendAllBytesAsyncCore(IFile file, ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenAppend();
        await stream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
    }

    private static void AppendAllLinesCore(IFile file, IEnumerable<string> contents, Encoding encoding)
    {
        using var stream = file.OpenAppend();
        using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        foreach (var line in contents)
        {
            writer.WriteLine(line);
        }
    }

    private static async Task AppendAllLinesAsyncCore(IFile file, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenAppend();
        await using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        foreach (var line in contents)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteLineAsync((line ?? string.Empty).AsMemory(), cancellationToken).ConfigureAwait(false);
        }
    }

    private static void AppendAllTextCore(IFile file, ReadOnlySpan<char> contents, Encoding encoding)
    {
        using var stream = file.OpenAppend();
        using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        writer.Write(contents);
    }

    private static async Task AppendAllTextAsyncCore(IFile file, ReadOnlyMemory<char> contents, Encoding encoding, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenAppend();
        await using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        await writer.WriteAsync(contents, cancellationToken).ConfigureAwait(false);
    }

    private static byte[] ReadAllBytesCore(IFile file)
    {
        using var stream = file.OpenRead();
        if (stream.CanSeek)
        {
            var length = stream.Length - stream.Position;
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, int.MaxValue);
            if (length == 0)
            {
                return [];
            }

            var buffer = new byte[(int)length];
            stream.ReadExactly(buffer);
            return buffer;
        }

        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    private static async Task<byte[]> ReadAllBytesAsyncCore(IFile file, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenRead();
        if (stream.CanSeek)
        {
            var length = stream.Length - stream.Position;
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, int.MaxValue);
            if (length == 0)
            {
                return [];
            }

            var buffer = new byte[(int)length];
            await stream.ReadExactlyAsync(buffer, cancellationToken).ConfigureAwait(false);
            return buffer;
        }

        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, cancellationToken).ConfigureAwait(false);
        return memory.ToArray();
    }

    private static string[] ReadAllLinesCore(IFile file, Encoding encoding)
    {
        using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
        var lines = new List<string>();
        while (reader.ReadLine() is { } line)
        {
            lines.Add(line);
        }

        return [.. lines];
    }

    private static async Task<string[]> ReadAllLinesAsyncCore(IFile file, Encoding encoding, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
        var lines = new List<string>();
        while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
        {
            lines.Add(line);
        }

        return [.. lines];
    }

    private static string ReadAllTextCore(IFile file, Encoding encoding)
    {
        using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
        return reader.ReadToEnd();
    }

    private static async Task<string> ReadAllTextAsyncCore(IFile file, Encoding encoding, CancellationToken cancellationToken)
    {
        await using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
        return await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
    }

    private static IEnumerable<string> ReadLinesCore(IFile file, Encoding encoding)
    {
        using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
        while (reader.ReadLine() is { } line)
        {
            yield return line;
        }
    }

    private static async IAsyncEnumerable<string> ReadLinesAsyncCore(
        IFile file,
        Encoding encoding,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var stream = file.OpenRead();
        using var reader = new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen: true);
        while (await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false) is { } line)
        {
            yield return line;
        }
    }

    private static void WriteAllBytesCore(IFile file, ReadOnlySpan<byte> bytes)
    {
        using var stream = OpenForWrite(file);
        stream.Write(bytes);
    }

    private static async Task WriteAllBytesAsyncCore(IFile file, ReadOnlyMemory<byte> bytes, CancellationToken cancellationToken)
    {
        await using var stream = OpenForWrite(file);
        await stream.WriteAsync(bytes, cancellationToken).ConfigureAwait(false);
    }

    private static void WriteAllLinesCore(IFile file, IEnumerable<string> contents, Encoding encoding)
    {
        using var stream = OpenForWrite(file);
        using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        foreach (var line in contents)
        {
            writer.WriteLine(line);
        }
    }

    private static async Task WriteAllLinesAsyncCore(IFile file, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken)
    {
        await using var stream = OpenForWrite(file);
        await using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        foreach (var line in contents)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await writer.WriteLineAsync((line ?? string.Empty).AsMemory(), cancellationToken).ConfigureAwait(false);
        }
    }

    private static void WriteAllTextCore(IFile file, ReadOnlySpan<char> contents, Encoding encoding)
    {
        using var stream = OpenForWrite(file);
        using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        writer.Write(contents);
    }

    private static async Task WriteAllTextAsyncCore(IFile file, ReadOnlyMemory<char> contents, Encoding encoding, CancellationToken cancellationToken)
    {
        await using var stream = OpenForWrite(file);
        await using var writer = new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen: true);
        await writer.WriteAsync(contents, cancellationToken).ConfigureAwait(false);
    }
}
