using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.DotNetTool.Module
{
    /// <summary>
    /// Represents a file locator for dotnet Tool packages that returns relevant
    /// files given the resource type.
    /// </summary>
    public interface IDotNetToolContentResolver
    {
        /// <summary>
        /// Gets the relevant files for a dotnet Tool package
        /// given a resource type.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="type">The resource type.</param>
        /// <returns>A collection of files.</returns>
        IReadOnlyCollection<IFile> GetFiles(PackageReference package, PackageType type);
    }
}
