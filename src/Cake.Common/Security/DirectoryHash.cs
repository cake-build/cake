// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.IO;

namespace Cake.Common.Security
{
    /// <summary>
    /// Represents a calculated directory hash.
    /// </summary>
    public sealed class DirectoryHash
    {
        private readonly byte[] _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryHash"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="hash">The computed hash.</param>
        /// <param name="hashAlgorithm">The algorithm used.</param>
        /// <param name="fileHashList">List of all computed <see cref="FileHash"/>.</param>
        public DirectoryHash(
            DirectoryPath directoryPath,
            byte[] hash,
            HashAlgorithm hashAlgorithm,
            IEnumerable<FileHash> fileHashList)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            if (fileHashList == null)
            {
                throw new ArgumentNullException(nameof(fileHashList));
            }

            Path = directoryPath;
            _hash = (byte[])hash.Clone();
            Algorithm = hashAlgorithm;
            FileHashList.AddRange(fileHashList);
        }

        /// <summary>
        /// Gets the algorithm used for the hash computation.
        /// </summary>
        public HashAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the <see cref="DirectoryPath"/> for the directory.
        /// </summary>
        public DirectoryPath Path { get; }

        /// <summary>
        /// Gets the list of <see cref="FileHash"/> for all files of the directory.
        /// </summary>
        public List<FileHash> FileHashList { get; } = new List<FileHash>();

        /// <summary>
        /// Gets the raw computed hash.
        /// </summary>
        public byte[] ComputedHash => (byte[])_hash.Clone();

        /// <summary>
        /// Convert the directory hash to a hexadecimal string.
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
