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
        /// <inheritdoc/>
        public void Register(ICakeContainerRegistrar registrar)
        {
            ArgumentNullException.ThrowIfNull(registrar);

            registrar.RegisterType<NuGetPackageInstaller>().As<INuGetPackageInstaller>().As<IPackageInstaller>().Singleton();
            registrar.RegisterType<NuGetContentResolver>().As<INuGetContentResolver>().Singleton();
            registrar.RegisterType<NuGetLoadDirectiveProvider>().As<ILoadDirectiveProvider>().Singleton();

            registrar.RegisterType<InProcessInstaller>().Singleton();
            registrar.RegisterType<OutOfProcessInstaller>().Singleton();
        }
    }
}