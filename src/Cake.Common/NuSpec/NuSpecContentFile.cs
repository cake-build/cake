namespace Cake.Common.NuSpec
{
    /// <summary>
    /// Represents a NuGet nuspec Content File Entry
    /// </summary>
    public class NuSpecContentFile
    {
        /// <summary>
        /// Gets or sets the content file's include pattern.
        /// </summary>
        /// <value>The dependency's include pattern.</value>
        public string Include { get; set; }

        /// <summary>
        /// Gets or sets the content file's exclude pattern.
        /// </summary>
        /// <value>The dependency's exclude pattern.</value>
        public string Exclude { get; set; }

        /// <summary>
        /// Gets or sets the content file's build action.
        /// </summary>
        /// <value>The dependency's exclude pattern.</value>
        public string BuildAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content file should be copied to the output.
        /// </summary>
        /// <value>
        ///   <c>true</c> if should be copied; otherwise, <c>false</c>.
        /// </value>
        public bool CopyToOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the content files should be flatten.
        /// </summary>
        /// <value>
        ///   <c>true</c> if should be flatten; otherwise, <c>false</c>.
        /// </value>
        public bool Flatten { get; set; }
    }
}