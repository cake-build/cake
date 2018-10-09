namespace Cake.Common.NuSpec
{
    /// <summary>
    /// Represents a NuGet nuspec Reference
    /// </summary>
    public class NuSpecReference
    {
        /// <summary>
        /// Gets or sets the reference's file.
        /// </summary>
        /// <value>The reference's file.</value>
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the target framework for the reference.
        /// </summary>
        /// <value>The target framework for the reference.</value>
        public string TargetFramework { get; set; }
    }
}