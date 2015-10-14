namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Represents a NuGet package to be installed.
    /// </summary>
    public sealed class NuGetPackage
    {
        private readonly string _packageId;

        /// <summary>
        /// Gets the package identifier.
        /// </summary>
        /// <value>The package identifier.</value>
        public string PackageId
        {
            get { return _packageId; }
        }

        /// <summary>
        /// Gets or sets the NuGet source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackage"/> class.
        /// </summary>
        /// <param name="packageId">The package identifier.</param>
        public NuGetPackage(string packageId)
        {
            _packageId = packageId;
        }
    }
}
