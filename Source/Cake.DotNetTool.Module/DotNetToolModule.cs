using System;
using Cake.Core.Annotations;
using Cake.Core.Composition;
using Cake.Core.Packaging;
using Cake.DotNetTool.Module;

[assembly: CakeModule(typeof(DotNetToolModule))]
namespace Cake.DotNetTool.Module
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.DotNetTool.Module assembly.
    /// </summary>
    public sealed class DotNetToolModule : ICakeModule
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

            registrar.RegisterType<DotNetToolPackageInstaller>().As<IPackageInstaller>().Singleton();
            registrar.RegisterType<DotNetToolContentResolver>().As<IDotNetToolContentResolver>().Singleton();
        }
    }
}
