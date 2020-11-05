// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.Composition;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Frosting.Internal;

namespace Cake.Frosting
{
    /// <summary>
    /// Contains extensions for <see cref="ICakeServices"/>.
    /// </summary>
    public static class CakeServicesExtensions
    {
        /// <summary>
        /// Registers the specified context type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TContext">The type of the context to register.</typeparam>
        /// <param name="services">The service registration collection.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseContext<TContext>(this ICakeServices services)
            where TContext : IFrostingContext
        {
            Guard.ArgumentNotNull(services, nameof(services));

            services.RegisterType<TContext>().AsSelf().As<IFrostingContext>().Singleton();
            return services;
        }

        /// <summary>
        /// Registers the specified lifetime type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TLifetime">The type of the lifetime.</typeparam>
        /// <param name="services">The service registration collection.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseLifetime<TLifetime>(this ICakeServices services)
            where TLifetime : IFrostingLifetime
        {
            Guard.ArgumentNotNull(services, nameof(services));

            services.RegisterType<TLifetime>().As<IFrostingLifetime>().Singleton();
            return services;
        }

        /// <summary>
        /// Registers the specified task lifetime type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TLifetime">The type of the lifetime.</typeparam>
        /// <param name="services">The service registration collection.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseTaskLifetime<TLifetime>(this ICakeServices services)
            where TLifetime : IFrostingTaskLifetime
        {
            Guard.ArgumentNotNull(services, nameof(services));

            services.RegisterType<TLifetime>().As<IFrostingTaskLifetime>().Singleton();
            return services;
        }

        /// <summary>
        /// Registers the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="services">The service registration collection.</param>
        /// <param name="assembly">The assembly to register.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseAssembly(this ICakeServices services, Assembly assembly)
        {
            Guard.ArgumentNotNull(services, nameof(services));

            services.RegisterInstance(assembly).Singleton();
            return services;
        }

        /// <summary>
        /// Registers the specified module type.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="services">The service registration collection.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseModule<TModule>(this ICakeServices services)
            where TModule : ICakeModule, new()
        {
            Guard.ArgumentNotNull(services, nameof(services));

            var module = new TModule();
            module.Register(services);
            return services;
        }

        /// <summary>
        /// Sets the relative working directory to be used when running the build.
        /// </summary>
        /// <param name="services">The service registration collection.</param>
        /// <param name="path">The working directory path.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseWorkingDirectory(this ICakeServices services, DirectoryPath path)
        {
            Guard.ArgumentNotNull(services, nameof(services));

            services.RegisterInstance(new WorkingDirectory(path)).AsSelf().Singleton();
            return services;
        }

        /// <summary>
        ///  Add or replace a setting in the configuration.
        /// </summary>
        /// <param name="services">The service registration collection.</param>
        /// <param name="key">The key of the setting to add or replace.</param>
        /// <param name="value">The value of the setting to add or replace.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseSetting(this ICakeServices services, string key, string value)
        {
            Guard.ArgumentNotNull(services, nameof(services));

            var info = new ConfigurationSetting(key, value);
            services.RegisterInstance(info).AsSelf().Singleton();
            return services;
        }

        /// <summary>
        /// Registers a specific tool for installation.
        /// </summary>
        /// <typeparam name="TPackageInstaller">The type of the package installer.</typeparam>
        /// <param name="services">The service registration collection.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UsePackageInstaller<TPackageInstaller>(this ICakeServices services)
            where TPackageInstaller : IPackageInstaller
        {
            Guard.ArgumentNotNull(services, nameof(services));

            services.RegisterType<TPackageInstaller>().As<IPackageInstaller>().Singleton();
            return services;
        }

        /// <summary>
        /// Registers a specific tool for installation.
        /// </summary>
        /// <param name="services">The service registration collection.</param>
        /// <param name="uri">The tool URI.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseTool(this ICakeServices services, Uri uri)
        {
            Guard.ArgumentNotNull(services, nameof(services));

            var package = new PackageReference(uri.OriginalString);
            services.RegisterInstance(package).Singleton();
            return services;
        }
    }
}