using System.Runtime.Versioning;

namespace Cake.Core.IO.NuGet.Parsing
{
    /// <summary>
    /// Represents an object that parses the segments of a filepath for a .Net framework name
    /// </summary>
    public interface IAssemblyFilePathFrameworkNameParser
    {
        /// <summary>
        /// Parses the framework name from assembly file path.
        /// </summary>
        /// <param name="path">The assembly file path.</param>
        /// <returns>the parsed framework name, or <c>null</c> when path contains no folders.</returns>
        FrameworkName Parse(FilePath path);
    }
}