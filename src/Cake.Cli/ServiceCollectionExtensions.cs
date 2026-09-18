// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Modules;
using Cake.Core.Composition;
using Cake.Core.Modules;
using Cake.DotNetTool.Module;
using Cake.NuGet;
using Microsoft.Extensions.DependencyInjection;

namespace Cake.Cli
{
    /// <summary>
    /// Contains extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the specified Cake module.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="module">The module to register.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseModule(this IServiceCollection services, ICakeModule module)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(module);

            var registrar = new ContainerRegistrar(services);
            module.Register(registrar);
            registrar.Transfer();
            return services;
        }

        /// <summary>
        /// Registers the specified Cake module.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseModule<TModule>(this IServiceCollection services)
            where TModule : ICakeModule, new()
        {
            return services.UseModule(new TModule());
        }

        /// <summary>
        /// Registers the default Cake diagnostics services.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection AddCakeDiagnostics(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var registrar = new ContainerRegistrar(services);
            registrar.AddCakeDiagnostics();
            registrar.Transfer();
            return services;
        }

        /// <summary>
        /// Registers the default Cake modules.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseCakeDefaultModules(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            return services
                .UseModule<CoreModule>()
                .UseModule<CommonModule>()
                .UseModule<NuGetModule>()
                .UseModule<DotNetToolModule>();
        }
    }
}
