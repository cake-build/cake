// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.IO;
using Cake.Core.Packaging;
using CoreConstants = Cake.Core.Constants;

namespace Cake.Frosting
{
    /// <summary>
    /// Contains extension methods for <see cref="CakeHost"/>.
    /// </summary>
    public static class CakeHostExtensions
    {
        /// <summary>
        /// Specify the startup type to be used by the Cake host.
        /// </summary>
        /// <typeparam name="TStartup">The startup type.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseStartup<TStartup>(this CakeHost host)
            where TStartup : IFrostingStartup, new()
        {
            if (host is null)
            {
                throw new ArgumentNullException(nameof(host));
            }

            return host.ConfigureServices(services =>
            {
                var startup = new TStartup();
                startup.Configure(services);
            });
        }

        /// <summary>
        /// Registers the specified context type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TContext">The type of the context to register.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseContext<TContext>(this CakeHost host)
            where TContext : class, IFrostingContext
        {
            return host.ConfigureServices(services => services.UseContext<TContext>());
        }

        /// <summary>
        /// Registers the specified lifetime type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TLifetime">The type of the lifetime.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseLifetime<TLifetime>(this CakeHost host)
            where TLifetime : class, IFrostingLifetime
        {
            return host.ConfigureServices(services => services.UseLifetime<TLifetime>());
        }

        /// <summary>
        /// Registers a setup action.
        /// </summary>
        /// <typeparam name="TSetup">The setup action.</typeparam>
        /// <param name="host">The service collection.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseSetup<TSetup>(this CakeHost host)
            where TSetup : class, IFrostingSetup
        {
            return host.ConfigureServices(services => services.UseSetup<TSetup>());
        }

        /// <summary>
        /// Registers a teardown action.
        /// </summary>
        /// <typeparam name="TTeardown">The teardown action.</typeparam>
        /// <param name="host">The service collection.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseTeardown<TTeardown>(this CakeHost host)
            where TTeardown : class, IFrostingTeardown
        {
            return host.ConfigureServices(services => services.UseTeardown<TTeardown>());
        }

        /// <summary>
        /// Registers the specified task lifetime type.
        /// Only the last registration will be used.
        /// </summary>
        /// <typeparam name="TLifetime">The type of the lifetime.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseTaskLifetime<TLifetime>(this CakeHost host)
            where TLifetime : class, IFrostingTaskLifetime
        {
            return host.ConfigureServices(services => services.UseTaskLifetime<TLifetime>());
        }

        /// <summary>
        /// Registers a task setup action.
        /// </summary>
        /// <typeparam name="TSetup">The task setup action.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseTaskSetup<TSetup>(this CakeHost host)
            where TSetup : class, IFrostingTaskSetup
        {
            return host.ConfigureServices(services => services.UseTaskSetup<TSetup>());
        }

        /// <summary>
        /// Registers a task teardown action.
        /// </summary>
        /// <typeparam name="TTeardown">The task teardown action.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseTaskTeardown<TTeardown>(this CakeHost host)
            where TTeardown : class, IFrostingTaskTeardown
        {
            return host.ConfigureServices(services => services.UseTaskTeardown<TTeardown>());
        }

        /// <summary>
        /// Registers the specified module type.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseModule<TModule>(this CakeHost host)
            where TModule : ICakeModule, new()
        {
            return host.ConfigureServices(services => services.UseModule<TModule>());
        }

        /// <summary>
        /// Registers a package installer.
        /// </summary>
        /// <typeparam name="TPackageInstaller">The type of the package installer.</typeparam>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UsePackageInstaller<TPackageInstaller>(this CakeHost host)
            where TPackageInstaller : class, IPackageInstaller
        {
            return host.ConfigureServices(services => services.UsePackageInstaller<TPackageInstaller>());
        }

        /// <summary>
        /// Registers a specific tool for installation.
        /// </summary>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <param name="uri">The tool URI.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost InstallTool(this CakeHost host, Uri uri)
        {
            return host.ConfigureServices(services => services.UseTool(uri));
        }

        /// <summary>
        /// Sets the relative working directory to be used when running the build.
        /// </summary>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <param name="path">The working directory path.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseWorkingDirectory(this CakeHost host, DirectoryPath path)
        {
            return host.ConfigureServices(services => services.UseWorkingDirectory(path));
        }

        /// <summary>
        /// Sets the specified Cake setting to the specified value.
        /// </summary>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <param name="key">The setting key.</param>
        /// <param name="value">The setting value.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost UseCakeSetting(this CakeHost host, string key, string value)
        {
            return host.ConfigureServices(services => services.UseCakeSetting(key, value));
        }

        /// <summary>
        /// Sets the specified path as the path where tools and addins will be installed.
        /// </summary>
        /// <param name="host">The <see cref="CakeHost"/> to configure.</param>
        /// <param name="path">The tool path.</param>
        /// <returns>The same <see cref="CakeHost"/> instance so that multiple calls can be chained.</returns>
        public static CakeHost SetToolPath(this CakeHost host, DirectoryPath path)
        {
            host.UseCakeSetting(CoreConstants.Paths.Tools, path.FullPath);
            return host;
        }
    }
}
