// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Cake.Core.Composition;
using Cake.Frosting.Internal;

// ReSharper disable once CheckNamespace
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
        /// <typeparam name="T">The type of the module</typeparam>
        /// <param name="services">The service registration collection.</param>
        /// <returns>The same <see cref="ICakeServices"/> instance so that multiple calls can be chained.</returns>
        public static ICakeServices UseModule<T>(this ICakeServices services)
            where T : ICakeModule, new()
        {
            Guard.ArgumentNotNull(services, nameof(services));

            var module = new T();
            module.Register(services);
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
            var info = new ConfigurationSetting(key, value);
            services.RegisterInstance(info).AsSelf().Singleton();
            return services;
        }
    }
}