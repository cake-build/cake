// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.Composition;
using Cake.Core.Configuration;

namespace Cake.Infrastructure
{
    /// <summary>
    /// Responsible for registering all dependencies
    /// that is required for executing a script.
    /// </summary>
    public interface IContainerConfigurator
    {
        /// <summary>
        /// Configures the container with the specified registrar, configuration, and arguments.
        /// </summary>
        /// <param name="registrar">The container registrar.</param>
        /// <param name="configuration">The Cake configuration.</param>
        /// <param name="arguments">The Cake arguments.</param>
        void Configure(
            ICakeContainerRegistrar registrar,
            ICakeConfiguration configuration,
            ICakeArguments arguments);
    }
}
