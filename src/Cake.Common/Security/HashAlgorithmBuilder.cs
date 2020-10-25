using System;
using System.Security.Cryptography;

namespace Cake.Common.Security
{
    /// <inheritdoc/>
    public sealed class HashAlgorithmBuilder : IHashAlgorithmBuilder
    {
        /// <inheritdoc/>
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
