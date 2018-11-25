namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Specifies the package's source code location, allowing IDEs to download and debug the code.
    /// </summary>
    public class NuGetRepository
    {
        /// <summary>
        /// Gets or sets the type of repository e.g. Git.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the repository's URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the name of the branch within the repository.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the corresponding commit ID for the specified version of the package.
        /// </summary>
        public string Commit { get; set; }
    }
}
