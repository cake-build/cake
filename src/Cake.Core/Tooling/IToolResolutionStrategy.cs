using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Represents a tool resolution strategy.
    /// </summary>
    public interface IToolResolutionStrategy
    {
        /// <summary>
        /// Resolves the specified tool using the specified tool repository.
        /// </summary>
        /// <param name="repository">The tool repository.</param>
        /// <param name="tool">The tool.</param>
        /// <returns>The path to the tool; otherwise <c>null</c>.</returns>
        FilePath Resolve(IToolRepository repository, string tool);
    }
}