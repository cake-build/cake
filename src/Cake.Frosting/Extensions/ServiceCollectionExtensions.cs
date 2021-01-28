// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Frosting.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Cake.Frosting
{
    /// <summary>
    /// Contains extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the specified context type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TContext">The type of the context to register.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseContext<TContext>(this IServiceCollection services)
            where TContext : class, IFrostingContext
        {
            services.AddSingleton<IFrostingContext, TContext>();
            return services;
        }

        /// <summary>
        /// Registers the specified lifetime type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TLifetime">The type of the lifetime.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseLifetime<TLifetime>(this IServiceCollection services)
            where TLifetime : class, IFrostingLifetime
        {
            services.AddSingleton<IFrostingLifetime, TLifetime>();
            services.AddSingleton<IFrostingSetup>(s => s.GetRequiredService<IFrostingLifetime>());
            services.AddSingleton<IFrostingTeardown>(s => s.GetRequiredService<IFrostingLifetime>());
            return services;
        }

        /// <summary>
        /// Registers a setup action.
        /// </summary>
        /// <typeparam name="TSetup">The setup action.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseSetup<TSetup>(this IServiceCollection services)
            where TSetup : class, IFrostingSetup
        {
            return services.AddSingleton<IFrostingSetup, TSetup>();
        }

        /// <summary>
        /// Registers a teardown action.
        /// </summary>
        /// <typeparam name="TTeardown">The teardown action.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseTeardown<TTeardown>(this IServiceCollection services)
            where TTeardown : class, IFrostingTeardown
        {
            return services.AddSingleton<IFrostingTeardown, TTeardown>();
        }

        /// <summary>
        /// Registers the specified task lifetime type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TLifetime">The type of the lifetime.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseTaskLifetime<TLifetime>(this IServiceCollection services)
            where TLifetime : class, IFrostingTaskLifetime
        {
            services.AddSingleton<IFrostingTaskLifetime, TLifetime>();
            services.AddSingleton<IFrostingTaskSetup>(s => s.GetRequiredService<IFrostingTaskLifetime>());
            services.AddSingleton<IFrostingTaskTeardown>(s => s.GetRequiredService<IFrostingTaskLifetime>());
            return services;
        }

        /// <summary>
        /// Registers a task setup action.
        /// </summary>
        /// <typeparam name="TSetup">The task setup action.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseTaskSetup<TSetup>(this IServiceCollection services)
            where TSetup : class, IFrostingTaskSetup
        {
            return services.AddSingleton<IFrostingTaskSetup, TSetup>();
        }

        /// <summary>
        /// Registers a task teardown action.
        /// </summary>
        /// <typeparam name="TTeardown">The task teardown action.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseTaskTeardown<TTeardown>(this IServiceCollection services)
            where TTeardown : class, IFrostingTaskTeardown
        {
            return services.AddSingleton<IFrostingTaskTeardown, TTeardown>();
        }

        /// <summary>
        /// Registers the specified module type.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseModule<TModule>(this IServiceCollection services)
            where TModule : ICakeModule, new()
        {
            var module = new TModule();

            var adapter = new ServiceCollectionAdapter();
            module.Register(adapter);
            adapter.Transfer(services);

            return services;
        }

        /// <summary>
        /// Registers a package installer.
        /// </summary>
        /// <typeparam name="TPackageInstaller">The type of the package installer.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UsePackageInstaller<TPackageInstaller>(this IServiceCollection services)
            where TPackageInstaller : class, IPackageInstaller
        {
            services.AddSingleton<IPackageInstaller, TPackageInstaller>();
            return services;
        }

        /// <summary>
        /// Registers a specific tool for installation.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="uri">The tool URI.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseTool(this IServiceCollection services, Uri uri)
        {
            var package = new PackageReference(uri.OriginalString);
            services.AddSingleton(package);
            return services;
        }

        /// <summary>
        /// Sets the relative working directory to be used when running the build.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="path">The working directory path.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseWorkingDirectory(this IServiceCollection services, DirectoryPath path)
        {
            services.AddSingleton(new WorkingDirectory(path));
            return services;
        }

        /// <summary>
        /// Sets the specified Cake setting to the specified value.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="key">The setting key.</param>
        /// <param name="value">The setting value.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseCakeSetting(this IServiceCollection services, string key, string value)
        {
            services.AddSingleton(new FrostingConfigurationValue(key, value));
            return services;
        }

        /// <summary>
        /// Sets the tool path configuration to the specified value.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="toolPath">The tool path value.</param>
        /// <returns>The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.</returns>
        public static IServiceCollection UseToolPath(this IServiceCollection services, DirectoryPath toolPath)
        {
            return services.UseCakeSetting("paths_tools", toolPath.FullPath);
        }
    }
}
