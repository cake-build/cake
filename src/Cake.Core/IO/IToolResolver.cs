namespace Cake.Core.IO
{
    /// <summary>
    /// Generic Tool Resolver Interface
    /// </summary>
    public interface IToolResolver
    {
        /// <summary>
        /// Gets the tool name.
        /// </summary>
        /// <value>The tool name.</value>
        string Name { get; }

        /// <summary>
        /// Resolves the tool path.
        /// </summary>
        /// <returns>The tool path.</returns>
        FilePath ResolveToolPath();
    }
}