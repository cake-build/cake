namespace Cake.Common.Tools.Chocolatey.Pack
{
    /// <summary>
    /// Represents a Chocolatey NuGet nuspec dependency.
    /// </summary>
    public class ChocolateyNuSpecDependency
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