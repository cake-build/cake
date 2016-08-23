// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Composition
{
    /// <summary>
    /// Represents a registration builder for a container.
    /// </summary>
    public interface ICakeRegistrationBuilder
    {
        /// <summary>
        /// Adds a registration type to the configuration.
        /// </summary>
        /// <param name="type">The registration type.</param>
        /// <returns>The same <see cref="ICakeRegistrationBuilder"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder As(Type type);

        /// <summary>
        /// Adds a registration type that matches the implementation type.
        /// </summary>
        /// <returns>The same <see cref="ICakeRegistrationBuilder"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder AsSelf();

        /// <summary>
        /// Configure the component so that every dependent component
        /// gets the same, shared instance. This is the default lifetime scope.
        /// </summary>
        /// <returns>The same <see cref="ICakeRegistrationBuilder"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder Singleton();

        /// <summary>
        /// Configure the component so that every dependent component
        /// gets a new, unique instance.
        /// </summary>
        /// <returns>The same <see cref="ICakeRegistrationBuilder"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder Transient();
    }
}