namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Represents a NuGet path resolver.
    /// </summary>
    public interface INuGetToolResolver
    {
        /// <summary>
        /// Resolves the path to nuget.exe.
        /// </summary>
        /// <returns>The path to nuget.exe.</returns>
        FilePath ResolvePath();
    }
}