namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// Represents a NuGet nuspec dependency
    /// </summary>
    public class NuSpecDependency
    {
        /// <summary>
        /// Gets or sets the dependency's package ID.
        /// </summary>
        /// <value>The dependency's package ID.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the dependency's version.
        /// </summary>
        /// <value>The dependency's version.</value>
        public string Version { get; set; }
    }
}