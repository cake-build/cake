namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Represents a NuGet package installer.
    /// </summary>
    public interface INuGetPackageInstaller
    {
        /// <summary>
        /// Installs the specified package.
        /// </summary>
        /// <param name="package">The package definition.</param>
        /// <param name="installationRoot">The installation root path.</param>
        void InstallPackage(NuGetPackage package, DirectoryPath installationRoot);
    }
}
