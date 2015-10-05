using Cake.Core.IO;

namespace Cake.Common.Tools.Chocolatey
{
    /// <summary>
    /// Represents a Chocolatey path resolver.
    /// </summary>
    public interface IChocolateyToolResolver
    {
        /// <summary>
        /// Resolves the path to choco.exe.
        /// </summary>
        /// <returns>The path to choco.exe.</returns>
        FilePath ResolvePath();
    }
}