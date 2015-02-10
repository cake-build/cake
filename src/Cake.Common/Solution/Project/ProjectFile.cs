using Cake.Core.IO;

namespace Cake.Common.Solution.Project
{
    /// <summary>
    /// Represents a MSBuild project file.
    /// </summary>
    public sealed class ProjectFile
    {
        /// <summary>
        /// Gets or sets the project file path.
        /// </summary>
        /// <value>The project file path.</value>
        public FilePath FilePath { get; set; }

        /// <summary>
        /// Gets or sets the relative path to the project file.
        /// </summary>
        /// <value>
        /// The relative path to the project file.
        /// </value>
        public string RelativePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ProjectFile"/> is compiled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if compiled; otherwise, <c>false</c>.
        /// </value>
        public bool Compile { get; set; }
    }
}