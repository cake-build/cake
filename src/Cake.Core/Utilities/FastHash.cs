using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cake.Core.Utilities
{
    /// <summary>
    /// Optimized hash generator.
    /// </summary>
    public static class FastHash
    {
        /// <summary>
        /// Generates a hash of the passed byte arrays.
        /// </summary>
        /// <param name="input">The binary data to hash.</param>
        /// <returns>The hash value.</returns>
        public static string GenerateHash(byte[] input)
        {
            using (var md5 = MD5.Create())
            {
                md5.TransformBlock(input, 0, input.Length, input, 0);

                // Just finalize with empty bytes so we don't have to iterate over the enumerable multiple times
                md5.TransformFinalBlock(Encoding.UTF8.GetBytes(String.Empty), 0, 0);
                // Convert to hex string; This method is supposedly faster than the usual StringBuilder approach
                return ConvertBits(md5.Hash);
            }
        }

        /// <summary>
        /// Generates a hash of the passed byte arrays.
        /// </summary>
        /// <param name="inputs">The binary data to hash.</param>
        /// <returns>The hash value.</returns>
        public static string GenerateHash(IEnumerable<byte[]> inputs)
        {
            using (var md5 = MD5.Create())
            {
                foreach (var input in inputs)
                {
                    md5.TransformBlock(input, 0, input.Length, input, 0);
                }

                // Just finalize with empty bytes so we don't have to iterate over the enumerable multiple times
                md5.TransformFinalBlock(Encoding.UTF8.GetBytes(String.Empty), 0, 0);
                // Convert to hex string; This method is supposedly faster than the usual StringBuilder approach
                return ConvertBits(md5.Hash);
            }
        }

        private static string ConvertBits(byte[] hash)
        {
            return BitConverter.ToString(hash)
                    // without dashes
                    .Replace("-", String.Empty)
                    // make lowercase
                    .ToLower();
        }
    }
}