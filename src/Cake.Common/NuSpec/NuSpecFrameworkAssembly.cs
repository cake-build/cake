namespace Cake.Common.NuSpec
{
    /// <summary>
    /// Represents a NuGet nuspec FrameworkAssembly
    /// </summary>
    public class NuSpecFrameworkAssembly
    {
        /// <summary>
        /// Gets or sets the assembly name.
        /// </summary>
        /// <value>The assembly name.</value>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the framework assembly target framework.
        /// </summary>
        /// <value>The framework assembly target framework.</value>
        public string TargetFramework { get; set; }
    }
}