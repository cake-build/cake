namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Represents a NuGet nuspec file
    /// </summary>
    public class NuSpecContent
    {
        /// <summary>
        /// The location of the file or files to include. The path is relative to the NuSpec file unless an absolute path is specified. The wildcard character, *, is allowed. Using a double wildcard, **, implies a recursive directory search.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// This is a relative path to the directory within the package where the source files will be placed.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The file or files to exclude. This is usually combined with a wildcard value in the `src` attribute. The `exclude` attribute can contain a semi-colon delimited list of files or a file pattern. Using a double wildcard, **, implies a recursive exclude pattern.
        /// </summary>
        public string Exclude { get; set; }
    }
}