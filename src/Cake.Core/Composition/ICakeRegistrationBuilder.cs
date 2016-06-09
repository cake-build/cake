// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Composition
{
    /// <summary>
    /// Represents a registration builder for a specific type.
    /// </summary>
    /// <typeparam name="T">The type to configure.</typeparam>
    public interface ICakeRegistrationBuilder<T>
    {
        /// <summary>
        /// Adds a registration type to the configuration.
        /// </summary>
        /// <typeparam name="TRegistrationType">The registration type.</typeparam>
        /// <returns>The same <see cref="ICakeRegistrationBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder<T> As<TRegistrationType>();

        /// <summary>
        /// Adds a registration type that matches the implementation type.
        /// </summary>
        /// <returns>The same <see cref="ICakeRegistrationBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder<T> AsSelf();

        /// <summary>
        /// Configure the component so that every dependent component
        /// gets the same, shared instance. This is the default lifetime scope.
        /// </summary>
        /// <returns>The same <see cref="ICakeRegistrationBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder<T> Singleton();

        /// <summary>
        /// Configure the component so that every dependent component
        /// gets a new, unique instance.
        /// </summary>
        /// <returns>The same <see cref="ICakeRegistrationBuilder{T}"/> instance so that multiple calls can be chained.</returns>
        ICakeRegistrationBuilder<T> Transient();
    }
}
