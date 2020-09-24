namespace Cake.Common.Security
{
    /// <summary>
    /// Creates a <see cref="System.Security.Cryptography.HashAlgorithm"/> instance by
    /// <see cref="HashAlgorithm"/>.
    /// </summary>
    public interface IHashAlgorithmBuilder
    {
        /// <summary>
        /// Return a instance of <see cref="System.Security.Cryptography.HashAlgorithm"/> by
        /// <see cref="HashAlgorithm"/> enum value.
        /// </summary>
        /// <param name="hashAlgorithm">Algorithm enum value.</param>
        /// <returns>A <see cref="System.Security.Cryptography.HashAlgorithm"/> instance.</returns>
        System.Security.Cryptography.HashAlgorithm CreateHashAlgorithm(HashAlgorithm hashAlgorithm);
    }
}