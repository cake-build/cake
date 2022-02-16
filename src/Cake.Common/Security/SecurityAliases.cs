// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Security
{
    /// <summary>
    /// Contains security related functionality, such as calculating file
    /// hashes.
    /// </summary>
    [CakeAliasCategory("Security")]
    public static class SecurityAliases
    {
        /// <summary>
        /// Calculates the hash for a given file using the default (SHA256) algorithm.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns>A <see cref="FileHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///    "Cake executable file SHA256 hash: {0}",
        ///    CalculateFileHash("Cake.exe").ToHex());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static FileHash CalculateFileHash(this ICakeContext context, FilePath filePath)
        {
            return CalculateFileHash(context, filePath, HashAlgorithm.SHA256);
        }

        /// <summary>
        /// Calculates the hash for a given file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>A <see cref="FileHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake executable file MD5 hash: {0}",
        ///     CalculateFileHash("Cake.exe", HashAlgorithm.MD5).ToHex());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static FileHash CalculateFileHash(this ICakeContext context, FilePath filePath, HashAlgorithm hashAlgorithm)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var fileHashCalculator = new FileHashCalculator(context.FileSystem, new HashAlgorithmBuilder());
            return fileHashCalculator.Calculate(filePath, hashAlgorithm);
        }

        /// <summary>
        /// Calculates the hash for a given directory using the default (SHA256) algorithm.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The file path.</param>
        /// <param name="globs">The glob pattern to match.</param>
        /// <returns>A <see cref="DirectoryHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake It calculates the hashes from all cs files in all subdirectories using a SHA256 hash: {0}",
        ///     CalculateDirectoryHash("C:\directoryToHash", "./**/*.cs").ToHex());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static DirectoryHash CalculateDirectoryHash(this ICakeContext context,
            IEnumerable<GlobPattern> globs, DirectoryPath directoryPath)
        {
            return CalculateDirectoryHash(context, directoryPath, globs, HashAlgorithm.SHA256);
        }

        /// <summary>
        /// Calculates the hash for a given directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The file path.</param>
        /// <param name="globs">The glob pattern to match.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>A <see cref="DirectoryHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake It calculates the hashes from all cs files in all subdirectories using a MD5 hash: {0}",
        ///     CalculateDirectoryHash("C:\directoryToHash", "./**/*.cs", HashAlgorithm.MD5).ToHex());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static DirectoryHash CalculateDirectoryHash(this ICakeContext context,
            DirectoryPath directoryPath, IEnumerable<GlobPattern> globs, HashAlgorithm hashAlgorithm)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var directoryHashCalculator = new DirectoryHashCalculator(context, new HashAlgorithmBuilder());
            return directoryHashCalculator.Calculate(directoryPath, globs, hashAlgorithm);
        }

        /// <summary>
        /// Calculates the hash for a given directory using the default (SHA256) algorithm.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The file path.</param>
        /// <param name="globs">The glob pattern to match.</param>
        /// <returns>A <see cref="DirectoryHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake It calculates the hashes from all cs files in all subdirectories using a SHA256 hash: {0}",
        ///     CalculateDirectoryHash("C:\directoryToHash", "./**/*.cs").ToHex());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static DirectoryHash CalculateDirectoryHash(this ICakeContext context,
            IEnumerable<string> globs, DirectoryPath directoryPath)
        {
            return CalculateDirectoryHash(context, directoryPath, globs, HashAlgorithm.SHA256);
        }

        /// <summary>
        /// Calculates the hash for a given directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="directoryPath">The file path.</param>
        /// <param name="globs">The glob pattern to match.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>A <see cref="DirectoryHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake It calculates the hashes from all cs files in all subdirectories using a MD5 hash: {0}",
        ///     CalculateDirectoryHash("C:\directoryToHash", "./**/*.cs", HashAlgorithm.MD5).ToHex());
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static DirectoryHash CalculateDirectoryHash(this ICakeContext context,
            DirectoryPath directoryPath, IEnumerable<string> globs, HashAlgorithm hashAlgorithm)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var directoryHashCalculator = new DirectoryHashCalculator(context, new HashAlgorithmBuilder());
            return directoryHashCalculator.Calculate(directoryPath, globs, hashAlgorithm);
        }
    }
}