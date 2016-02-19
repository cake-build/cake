using System.Collections.Generic;
using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Filters assemblies for .Net target framework compatibility
    /// </summary>
    public interface INuGetAssemblyCompatibilityFilter
    {
        /// <summary>
        /// Filters a collection of assembly files for .Net target framework compatibility.
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        /// <param name="assemblyPaths">The assembly file paths, relative to their package folder.</param>
        /// <returns>a subset of the provided assemblyFiles that match the provided targetFramework.</returns>
        IEnumerable<FilePath> FilterCompatibleAssemblies(FrameworkName targetFramework, IEnumerable<FilePath> assemblyPaths);
    }
}