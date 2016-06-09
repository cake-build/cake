// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using System.Security.Cryptography;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Security
{
    /// <summary>
    /// Represents a file hash operation.
    /// </summary>
    public sealed class FileHashCalculator
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileHashCalculator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public FileHashCalculator(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }

            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Calculates the hash for a file using the given algorithm.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="hashAlgorithm">The algorithm to use.</param>
        /// <returns>A <see cref="FileHash"/> instance representing the calculated hash.</returns>
        public FileHash Calculate(FilePath filePath, HashAlgorithm hashAlgorithm)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            var file = _fileSystem.GetFile(filePath);

            if (!file.Exists)
            {
                const string format = "File '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, filePath.FullPath);
                throw new CakeException(message);
            }

            using (var hashAlgo = GetHashAlgorithm(hashAlgorithm))
            using (var readStream = file.OpenRead())
            {
                var hash = hashAlgo.ComputeHash(readStream);
                return new FileHash(filePath, hash, hashAlgorithm);
            }
        }

        private System.Security.Cryptography.HashAlgorithm GetHashAlgorithm(HashAlgorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case HashAlgorithm.MD5:
                    return MD5.Create();

                case HashAlgorithm.SHA256:
                    return SHA256.Create();

                case HashAlgorithm.SHA512:
                    return SHA512.Create();
            }

            throw new NotSupportedException(hashAlgorithm.ToString());
        }
    }
}
