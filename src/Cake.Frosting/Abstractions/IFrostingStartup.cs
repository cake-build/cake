// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents a startup configuration.
    /// </summary>
    public interface IFrostingStartup
    {
        /// <summary>
        /// Configures services used by Cake.
        /// </summary>
        /// <param name="services">The services to configure.</param>
        void Configure(ICakeServices services);
    }
}