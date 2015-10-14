using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseManager
{
    /// <summary>
    /// Represents a GitReleaseManager path resolver.
    /// </summary>
    public interface IGitReleaseManagerToolResolver
    {
        /// <summary>
        /// Resolves the path to GitReleaseManager.exe.
        /// </summary>
        /// <returns>The path to nuget.exe.</returns>
        FilePath ResolvePath();
    }
}