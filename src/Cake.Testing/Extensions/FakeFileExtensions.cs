// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;
using Cake.Core;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Testing
{
    /// <summary>
    /// Contains extensions for <see cref="FakeFile"/>.
    /// </summary>
    public static class FakeFileExtensions
    {
        /// <summary>
        /// Sets the content of the provided file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="content">The content.</param>
        /// <returns>The same <see cref="FakeFile"/> instance so that multiple calls can be chained.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static FakeFile SetContent(this FakeFile file, string content)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }
            using (var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(content);
                file.LastWriteTime = DateTime.Now;
                return file;
            }
        }

        /// <summary>
        /// Gets the binary content of the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The binary content of the specified file.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static byte[] GetBinaryContent(this FakeFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (!file.Exists)
            {
                throw new FileNotFoundException("File could not be found.", file.Path.FullPath);
            }
            using (var stream = file.OpenRead())
            using (var reader = new BinaryReader(stream))
            using (var memory = new MemoryStream())
            {
                reader.BaseStream.CopyTo(memory);
                return memory.ToArray();
            }
        }

        /// <summary>
        /// Gets the text content of the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The text content of the file.</returns>
        public static string GetTextContent(this FakeFile file)
        {
            return file?.GetTextContent(Encoding.UTF8);
        }

        /// <summary>
        /// Gets the text content of the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="encoding">The text encoding.</param>
        /// <returns>The text content of the file.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string GetTextContent(this FakeFile file, Encoding encoding)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (!file.Exists)
            {
                throw new FileNotFoundException("File could not be found.", file.Path.FullPath);
            }
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Determines if a specified file has a UTF-8 BOM.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>Whether or not the specified file has a UTF-8 BOM.</returns>
        // ReSharper disable once InconsistentNaming
        public static bool HasUTF8BOM(this FakeFile file)
        {
            var content = GetBinaryContent(file);
            var preamble = Encoding.UTF8.GetPreamble();
            return content.StartsWith(preamble);
        }

        /// <summary>
        /// Hides the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The same <see cref="FakeFile"/> instance so that multiple calls can be chained.</returns>
        public static FakeFile Hide(this FakeFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            file.Hidden = true;
            return file;
        }
    }
}