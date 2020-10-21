using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Security
{
    /// <summary>
    /// Class for calculating a hash for a directory.
    /// </summary>
    public class DirectoryHashCalculator
    {
        private readonly ICakeContext _context;
        private readonly IHashAlgorithmBuilder _hashAlgorithmBuilder;
        private readonly FileHashCalculator _fileHashCalculator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryHashCalculator"/> class.
        /// </summary>
        /// <param name="context">The cake context.</param>
        /// <param name="hashAlgorithmBuilder">The hash algorithm builder.</param>
        public DirectoryHashCalculator(ICakeContext context, IHashAlgorithmBuilder hashAlgorithmBuilder)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (hashAlgorithmBuilder == null)
            {
                throw new ArgumentNullException(nameof(hashAlgorithmBuilder));
            }

            _context = context;
            _hashAlgorithmBuilder = hashAlgorithmBuilder;
            _fileHashCalculator = new FileHashCalculator(_context.FileSystem, _hashAlgorithmBuilder);
        }

        /// <summary>
        /// Calculates the hash for a given directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>A <see cref="DirectoryHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake It calculates the hashes from all cs files in all subdirectories using a MD5 hash: {0}",
        ///     CalculateDirectoryHash("C:\directoryToHash", "./**/*.cs", HashAlgorithm.MD5).ToHex());
        /// </code>
        /// </example>
        public DirectoryHash Calculate(
            DirectoryPath directoryPath,
            IEnumerable<GlobPattern> pattern,
            HashAlgorithm hashAlgorithm)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            using (var incrementalDirectoryHash = _hashAlgorithmBuilder.CreateHashAlgorithm(hashAlgorithm))
            {
                var fileHashList = new List<FileHash>();
                var files = GetDirectoryFiles(directoryPath, pattern).OrderBy(file => file.FullPath);
                if (files.Any())
                {
                    var lastFile = files.LastOrDefault();
                    foreach (var file in GetDirectoryFiles(directoryPath, pattern))
                    {
                        var fileContentAndNameHash = CalculateFileContentAndNameHash(file, directoryPath, hashAlgorithm);
                        fileHashList.Add(fileContentAndNameHash);
                        if (file == lastFile)
                        {
                            incrementalDirectoryHash.TransformFinalBlock(
                                fileContentAndNameHash.ComputedHash, 0,
                                fileContentAndNameHash.ComputedHash.Length);
                        }
                        else
                        {
                            incrementalDirectoryHash.TransformBlock(
                                fileContentAndNameHash.ComputedHash, 0,
                                fileContentAndNameHash.ComputedHash.Length,
                                fileContentAndNameHash.ComputedHash, 0);
                        }
                    }

                    var directoryHash = incrementalDirectoryHash.Hash;
                    return new DirectoryHash(directoryPath, directoryHash, hashAlgorithm, fileHashList);
                }

                // No files found.
                return null;
            }
        }

        /// <summary>
        /// Calculates the hash for a given directory.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="pattern">The glob pattern to match.</param>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <returns>A <see cref="DirectoryHash"/> instance representing the calculated hash.</returns>
        /// <example>
        /// <code>
        /// Information(
        ///     "Cake It calculates the hashes from all cs files in all subdirectories using a MD5 hash: {0}",
        ///     CalculateDirectoryHash("C:\directoryToHash", "./**/*.cs", HashAlgorithm.MD5).ToHex());
        /// </code>
        /// </example>
        public DirectoryHash Calculate(
            DirectoryPath directoryPath,
            IEnumerable<string> pattern,
            HashAlgorithm hashAlgorithm)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return Calculate(
                directoryPath,
                pattern.Select(GlobPattern.FromString),
                hashAlgorithm);
        }

        private FilePathCollection GetDirectoryFiles(
            DirectoryPath directoryPath,
            IEnumerable<GlobPattern> pattern)
        {
            var directory = new Directory(directoryPath);

            if (!directory.Exists)
            {
                const string format = "Directory '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, directoryPath.FullPath);
                throw new CakeException(message);
            }

            var filePathCollection = new FilePathCollection();
            var fullGlobs = pattern.Select(x => directoryPath + x.Pattern).ToArray();

            foreach (var glob in fullGlobs)
            {
                foreach (var file in _context.GetFiles(glob))
                {
                    filePathCollection.Add(file);
                }
            }

            return filePathCollection;
        }

        private FileHash CalculateFileContentAndNameHash(
            FilePath filePath,
            DirectoryPath directoryPath,
            HashAlgorithm hashAlgorithm)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }

            using (var incrementalFileHash = _hashAlgorithmBuilder.CreateHashAlgorithm(hashAlgorithm))
            {
                // Calculate file content hash
                var fileContentHash = _fileHashCalculator.Calculate(filePath, hashAlgorithm);

                // Combine file content hash
                incrementalFileHash.TransformBlock(
                    fileContentHash.ComputedHash, 0,
                    fileContentHash.ComputedHash.Length,
                    fileContentHash.ComputedHash, 0);

                // combine filename hash
                var relativeFilePath = filePath.GetRelativePath(directoryPath);
                var fileFullNameArray = Encoding.ASCII.GetBytes(filePath.GetFilename().FullPath);
                using (var hashAlgorithmInstance = _hashAlgorithmBuilder.CreateHashAlgorithm(hashAlgorithm))
                {
                    var filenameHash = hashAlgorithmInstance.ComputeHash(fileFullNameArray);
                    incrementalFileHash.TransformFinalBlock(
                        filenameHash, 0,
                        filenameHash.Length);
                }
                var fileContentAndNameHash = incrementalFileHash.Hash;

                return new FileHash(filePath, fileContentAndNameHash, hashAlgorithm);
            }
        }
    }
}
