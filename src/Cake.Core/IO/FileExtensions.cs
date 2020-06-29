// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;

namespace Cake.Core.IO
{
    /// <summary>
    /// Contains extension methods for <see cref="IFile"/>.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="mode">The mode.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream Open(this IFile file, FileMode mode)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            return file.Open(mode,
                mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite,
                FileShare.None);
        }

        /// <summary>
        /// Opens the file using the specified options.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="access">The access.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream Open(this IFile file, FileMode mode, FileAccess access)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            return file.Open(mode, access, FileShare.None);
        }

        /// <summary>
        /// Opens the file for reading.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream OpenRead(this IFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            return file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Opens the file for writing.
        /// If the file already exists, it will be overwritten.
        /// </summary>
        /// <param name="file">The file to be opened.</param>
        /// <returns>A <see cref="Stream"/> to the file.</returns>
        public static Stream OpenWrite(this IFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            return file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        }

        /// <summary>
        /// Enumerates line in file.
        /// </summary>
        /// <param name="file">The file to be read from.</param>
        /// <param name="encoding">The encoding that is applied to the content of the file.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of file line content.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static IEnumerable<string> ReadLines(this IFile file, System.Text.Encoding encoding)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            using (var stream = file.OpenRead())
            using (var reader = new StreamReader(stream, encoding))
            {
                var result = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result.Add(line);
                }
                return result;
            }
        }

        /// <summary>
        /// Checks if file has CLR PE Header.
        /// </summary>
        /// <param name="file">The file to be read from.</param>
        /// <returns><c>true</c> if file is CLR assembly; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// See <a href="https://docs.microsoft.com/en-us/windows/desktop/Debug/pe-format">https://docs.microsoft.com/en-us/windows/desktop/Debug/pe-format</a> for more information.
        /// </remarks>
        public static bool IsClrAssembly(this IFile file)
        {
            if (!file.Exists || file.Length < 365)
            {
                return false;
            }

            using (var fs = file.OpenRead())
            {
                using (var reader = new System.IO.BinaryReader(fs))
                {
                    const uint MagicOffset = 0x18;
                    const uint Magic32Bit = 0x10b;
                    const int Offset32Bit = 0x5e;
                    const int Offset64Bit = 0x6e;
                    const int OffsetDictionary = 0x70;

                    // PE Header Start
                    fs.Position = 0x3C;

                    // Go to Magic header
                    fs.Position = reader.ReadUInt32() + MagicOffset;

                    // Check magic to get 32 / 64 bit offset
                    var is32Bit = reader.ReadUInt16() == Magic32Bit;
                    var offset = fs.Position + (is32Bit ? Offset32Bit : Offset64Bit) + OffsetDictionary;

                    if (offset + 4 > fs.Length)
                    {
                        return false;
                    }

                    // Go to dictionary start
                    fs.Position = offset;
                    return reader.ReadUInt32() > 0;
                }
            }
        }
    }
}