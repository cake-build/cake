// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    /// <summary>
    /// The module responsible for registering
    /// default types in the Cake.NuGet assembly.
    /// </summary>
    public sealed class NuGetModule : ICakeModule
    {
        /// <summary>
        /// Performs custom registrations in the provided registry.
        /// </summary>
        /// <param name="registry">The container registry.</param>
        public void Register(ICakeContainerRegistry registry)
        {
            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

#if NETCORE
            // NuGet V3
            registry.RegisterType<V3.NuGetV3ContentResolver>().As<INuGetContentResolver>().Singleton();
#else
            // NuGet V2
            registry.RegisterType<V2.NuGetV2ContentResolver>().As<INuGetContentResolver>().Singleton();
#endif

            // URI resource support.
            registry.RegisterType<NuGetPackageInstaller>().As<IPackageInstaller>().Singleton();
        }
    }
}