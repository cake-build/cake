namespace Cake.Common
{
    /// <summary>
    /// Represents the content in an assembly info file.
    /// </summary>
    public sealed class AssemblyInfoParseResult
    {
        private readonly string _assemblyVersion;
        private readonly string _assemblyFileVersion;
        private readonly string _assemblyInformationalVersion;

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>The assembly version.</value>
        public string AssemblyVersion
        {
            get { return _assemblyVersion; }
        }

        /// <summary>
        /// Gets the assembly file version.
        /// </summary>
        /// <value>The assembly file version.</value>
        public string AssemblyFileVersion
        {
            get { return _assemblyFileVersion; }
        }

        /// <summary>
        /// Gets the assembly informational version.
        /// </summary>
        /// <value>The assembly informational version.</value>
        public string AssemblyInformationalVersion
        {
            get { return _assemblyInformationalVersion; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfoParseResult"/> class.
        /// </summary>
        /// <param name="assemblyVersion">The assembly version.</param>
        /// <param name="assemblyFileVersion">The assembly file version.</param>
        /// <param name="assemblyInformationalVersion">The assembly informational version.</param>
        public AssemblyInfoParseResult(string assemblyVersion, string assemblyFileVersion, string assemblyInformationalVersion)
        {
            _assemblyVersion = assemblyVersion;
            _assemblyFileVersion = assemblyFileVersion;
            _assemblyInformationalVersion = assemblyInformationalVersion;
        }
    }
}