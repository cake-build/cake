using System;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.DotNet.Module;

[assembly: CakeModule(typeof(DotNetModule))]
namespace Cake.DotNet.Module
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.DotNet.Module assembly.
    /// </summary>
    public sealed class DotNetModule : ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registrar.
        /// </summary>
        /// <param name="registrar">The container registrar.</param>
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            registrar.RegisterType<DotNetPackageInstaller>().As<IPackageInstaller>().Singleton();
            registrar.RegisterType<DotNetContentResolver>().As<IDotNetContentResolver>().Singleton();
        }
    }
}
