namespace Cake.Common.Security
{
    /// <summary>
    /// The hash algorithm to use for a specific operation.
    /// </summary>
    public enum HashAlgorithm
    {
        /// <summary>
        /// The MD5 hash algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MD5,

        /// <summary>
        /// The SHA256 hash algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA256,

        /// <summary>
        /// The SHA512 hash algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA512
    }
}