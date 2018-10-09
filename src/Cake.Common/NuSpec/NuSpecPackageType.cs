namespace Cake.Common.NuSpec
{
    /// <summary>
    /// Represents a NuGet nuspec PackageType
    /// </summary>
    public class NuSpecPackageType
    {
        /// <summary>
        /// Gets or sets the packageType's name.
        /// </summary>
        /// <value>The packageType's name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the packageType's version.
        /// </summary>
        /// <value>The packageType's version.</value>
        public string Version { get; set; }
    }
}