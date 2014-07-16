using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a file system globber.
    /// </summary>
    public interface IGlobber
    {
        /// <summary>
        /// Returns <see cref="Path"/> instances matching the specified pattern.
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <returns><see cref="Path"/> instances matching the specified pattern.</returns>
        IEnumerable<Path> Match(string pattern);
    }
}
