// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.Configuration;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Processors.Loading;

namespace Cake.NuGet
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.NuGet assembly.
    /// </summary>
    public sealed class NuGetModule : ICakeModule
    {
        private readonly ICakeConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetModule"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public NuGetModule(ICakeConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <inheritdoc/>
        public void Register(ICakeContainerRegistrar registrar)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            // NuGet content resolver.
            registrar.RegisterType<NuGetContentResolver>()
                .As<INuGetContentResolver>()
                .Singleton();

#if !NETCORE
            // Load directive provider.
            registrar.RegisterType<NuGetLoadDirectiveProvider>()
                .As<ILoadDirectiveProvider>()
                .Singleton();
#endif

            // URI resource support.
            if (!bool.TryParse(_config.GetValue(Constants.NuGet.UseInProcessClient) ?? bool.TrueString, out bool useInProcessClient) || useInProcessClient)
            {
#if NETCORE
                // Load directive provider.
                registrar.RegisterType<NuGetLoadDirectiveProvider>()
                    .As<ILoadDirectiveProvider>()
                    .Singleton();
#endif
                registrar.RegisterType<Install.NuGetPackageInstaller>()
                    .As<INuGetPackageInstaller>()
                    .As<IPackageInstaller>()
                    .Singleton();
            }
            else
            {
                registrar.RegisterType<NuGetPackageInstaller>()
                    .As<INuGetPackageInstaller>()
                    .As<IPackageInstaller>()
                    .Singleton();
            }
        }
    }
}