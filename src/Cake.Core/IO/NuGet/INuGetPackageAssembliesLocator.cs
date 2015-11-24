using System.Collections.Generic;
using System.Runtime.Versioning;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Finds assemblies included in a nuget package.
    /// </summary>
    public interface INuGetPackageAssembliesLocator
    {
        /// <summary>
        /// Finds assemblies (DLLs) included in a nuget package.
        /// </summary>
        /// <param name="packageDirectory">The package directory.</param>
        /// <returns>
        /// the DLLs.
        /// </returns>
        IReadOnlyList<IFile> FindAssemblies(DirectoryPath packageDirectory);
    }
}