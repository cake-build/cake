namespace Cake.Core.IO
{
    /// <summary>
    /// Generic Tool Resolver Interface
    /// </summary>
    public interface IToolResolver
    {
        /// <summary>
        /// Name of tool
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Generic resolve path to tool
        /// </summary>
        /// <returns>nuget.exe path</returns>
        FilePath ResolveToolPath();
    }
}