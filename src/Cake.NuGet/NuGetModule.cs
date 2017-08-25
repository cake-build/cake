﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
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

            // NuGet content resolver.
            registrar.RegisterType<NuGetContentResolver>()
                .As<INuGetContentResolver>()
                .Singleton();

#if !NETCORE
            // Load directive not available for .NET Core
            registrar.RegisterType<NuGetLoadDirectiveProvider>()
                .As<ILoadDirectiveProvider>()
                .Singleton();
#endif

            // URI resource support.
            registrar.RegisterType<NuGetPackageInstaller>()
                .As<INuGetPackageInstaller>()
                .As<IPackageInstaller>()
                .Singleton();
        }
    }
}