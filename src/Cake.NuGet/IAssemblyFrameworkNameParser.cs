using System.Runtime.Versioning;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Represents an object that parses the segments of a filepath for a .Net framework name
    /// </summary>
    public interface IAssemblyFrameworkNameParser
    {
        /// <summary>
        /// Parses the framework name from assembly file path.
        /// </summary>
        /// <param name="path">The assembly file path.</param>
        /// <returns>the parsed framework name, or <c>null</c> when path contains no folders.</returns>
        FrameworkName Parse(FilePath path);
    }
}