using System;
using System.Security.Cryptography;

namespace Cake.Common.Security
{
    /// <summary>
    /// Creates a <see cref="System.Security.Cryptography.HashAlgorithm"/> instance by
    /// <see cref="HashAlgorithm"/>.
    /// </summary>
    public sealed class HashAlgorithmBuilder : IHashAlgorithmBuilder
    {
        /// <summary>
        /// Return a instance of <see cref="System.Security.Cryptography.HashAlgorithm"/> by
        /// <see cref="HashAlgorithm"/> enum value.
        /// </summary>
        /// <param name="hashAlgorithm">Algorithm enum value.</param>
        /// <returns>A <see cref="System.Security.Cryptography.HashAlgorithm"/> instance.</returns>
        public System.Security.Cryptography.HashAlgorithm CreateHashAlgorithm(HashAlgorithm hashAlgorithm)
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
