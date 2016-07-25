using System.Collections.Generic;
using System.Reflection;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Represents a script alias generator.
    /// </summary>
    public interface IProcessorExtensionFinder
    {
        /// <summary>
        /// Finds script aliases in the provided assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to find script aliases in.</param>
        /// <returns>The script aliases that were found.</returns>
        IReadOnlyList<IProcessorExtension> FindProcessorExtensions(IEnumerable<Assembly> assemblies);
    }
}