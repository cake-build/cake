using Cake.Core.Composition;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.NuGet assembly.
    /// </summary>
    public sealed class NuGetModule : ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registry.
        /// </summary>
        /// <param name="registry">The container registry.</param>
        public void Register(ICakeContainerRegistry registry)
        {
            // NuGet addins support
            registry.RegisterType<NuGetVersionUtilityAdapter>().As<INuGetFrameworkCompatibilityFilter>().As<IFrameworkNameParser>().Singleton();
            registry.RegisterType<NuGetPackageAssembliesLocator>().As<INuGetPackageAssembliesLocator>().Singleton();
            registry.RegisterType<NuGetPackageReferenceBundler>().As<INuGetPackageReferenceBundler>().Singleton();
            registry.RegisterType<NuGetAssemblyCompatibilityFilter>().As<INuGetAssemblyCompatibilityFilter>().Singleton();
            registry.RegisterType<AssemblyFrameworkNameParser>().As<IAssemblyFrameworkNameParser>().Singleton();

            // URI resource support.
            registry.RegisterType<NuGetPackageInstaller>().As<IPackageInstaller>().Singleton();
            registry.RegisterType<NuGetPackageContentResolver>().As<INuGetPackageContentResolver>().Singleton();
        }
    }
}
