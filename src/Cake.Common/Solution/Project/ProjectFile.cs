using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// MSBuild Project file content
    /// </summary>
    public sealed class ProjectFile
    {
        /// <summary>
        /// Path to file
        /// </summary>
        public FilePath FilePath { get; set; }
        /// <summary>
        /// Project releative path to file
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// If file is compiled
        /// </summary>
        public bool Compile { get; set; }
    }
}