using System;
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
        [CakeMethodAlias]
        public static FileHash CalculateFileHash(this ICakeContext context, FilePath filePath, HashAlgorithm hashAlgorithm)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var fileHashCalulator = new FileHashCalculator(context.FileSystem);
            return fileHashCalulator.Calculate(filePath, hashAlgorithm);
        }
    }
}
