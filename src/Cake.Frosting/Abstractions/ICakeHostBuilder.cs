// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents a builder for <see cref="ICakeHost"/>.
    /// </summary>
    public interface ICakeHostBuilder
    {
        /// <summary>
        /// Adds a delegate for configuring additional services for the host.
        /// </summary>
        /// <param name="configureServices">A delegate for configuring the <see cref="ICakeServices"/>.</param>
        /// <returns>The same <see cref="ICakeHostBuilder"/> instance so that multiple calls can be chained.</returns>
        ICakeHostBuilder ConfigureServices(Action<ICakeServices> configureServices);

        /// <summary>
        /// Builds the required services and an <see cref="ICakeHost"/> using the specified options.
        /// </summary>
        /// <returns>The built <see cref="ICakeHost"/>.</returns>
        ICakeHost Build();
    }
}