using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetPacker"/>.
    /// </summary>
    public sealed class NuGetPackSettings
    {
        /// <summary>
        /// Gets or sets the base path.
        /// </summary>
        /// <value>The base path.</value>
        public DirectoryPath BasePath { get; set; }

        /// <summary>
        /// Gets or sets the output directory.
        /// </summary>
        /// <value>The output directory.</value>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the Nuspec version.
        /// </summary>
        /// <value>The Nuspec version.</value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether package analysis should be performed.
        /// Defaults to <c>true</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if package analysis should be performed; otherwise, <c>false</c>.
        /// </value>
        public bool NoPackageAnalysis { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a symbol package should be created.
        /// Defaults to <c>false</c>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a symbol package should be created; otherwise, <c>false</c>.
        /// </value>
        public bool Symbols { get; set; }

        /// <summary>
        /// Gets or sets the tool path.
        /// </summary>
        /// <value>The tool path.</value>
        public FilePath ToolPath { get; set; }
    }
}
