// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Composition
{
    /// <summary>
    /// Represents a container registry used to register types and instances with Cake.
    /// </summary>
    public interface ICakeContainerRegistrar
    {
        /// <summary>
        /// Registers a type with the container registry.
        /// </summary>
        /// <param name="type">The implementation type to register.</param>
        /// <returns>A registration builder used to configure the registration.</returns>
        ICakeRegistrationBuilder RegisterType(Type type);

        /// <summary>
        /// Registers an instance with the container registry.
        /// </summary>
        /// <typeparam name="TImplementation">The instance's implementation type to register.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <returns>A registration builder used to configure the registration.</returns>
        ICakeRegistrationBuilder RegisterInstance<TImplementation>(TImplementation instance) where TImplementation : class;
    }
}