// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using Cake.Core.IO;

namespace Cake.Common.Security
{
    /// <summary>
    /// Represents a calculated file hash.
    /// </summary>
    public sealed class FileHash
    {
        private readonly byte[] _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHash"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="hash">The computed hash.</param>
        /// <param name="hashAlgorithm">The algorithm used.</param>
        public FileHash(FilePath filePath, byte[] hash, HashAlgorithm hashAlgorithm)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            Path = filePath;
            _hash = (byte[])hash.Clone();
            Algorithm = hashAlgorithm;
        }

        /// <summary>
        /// Gets the algorithm used for the hash computation.
        /// </summary>
        public HashAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the <see cref="FilePath"/> for the file.
        /// </summary>
        public FilePath Path { get; }

        /// <summary>
        /// Gets the raw computed hash.
        /// </summary>
        public byte[] ComputedHash => (byte[])_hash.Clone();

        /// <summary>
        /// Convert the file hash to a hexadecimal string.
        /// </summary>
        /// <returns>A hexadecimal string representing the computed hash.</returns>
        public string ToHex()
        {
            // Each byte becomes two characters. Prepare the StringBuilder accordingly.
            var builder = new StringBuilder(_hash.Length * 2);

            foreach (var b in _hash)
            {
                builder.AppendFormat("{0:x2}", b);
            }

            return builder.ToString();
        }
    }
}