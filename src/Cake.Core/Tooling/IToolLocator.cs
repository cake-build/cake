using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Represents a tool locator.
    /// </summary>
    public interface IToolLocator
    {
        /// <summary>
        /// Registers the specified tool file path.
        /// </summary>
        /// <param name="path">The tool path.</param>
        void RegisterFile(FilePath path);

        /// <summary>
        /// Resolves the path to the specified tool.
        /// </summary>
        /// <param name="tool">The tool.</param>
        /// <returns>A path if the tool was found; otherwise <c>null</c>.</returns>
        FilePath Resolve(string tool);
    }
}