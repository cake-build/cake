using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Represents a tool repository.
    /// </summary>
    public interface IToolRepository
    {
        /// <summary>
        /// Registers the specified path with the repository.
        /// </summary>
        /// <param name="path">The path to register.</param>
        void Register(FilePath path);

        /// <summary>
        /// Resolves the specified tool.
        /// </summary>
        /// <param name="tool">The tool to resolve.</param>
        /// <returns>The tool's file paths if any; otherwise <c>null</c>.</returns>
        IEnumerable<FilePath> Resolve(string tool);
    }
}