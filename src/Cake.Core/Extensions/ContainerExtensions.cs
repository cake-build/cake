// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// ReSharper disable once CheckNamespace
namespace Cake.Core.Composition
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeContainerRegistrar"/>.
    /// </summary>
    public static class ContainerRegistrarExtensions
    {
        /// <summary>
        /// Registers a type with the container registrar.
        /// </summary>
        /// <typeparam name="TImplementation">The implementation type to register.</typeparam>
        /// <param name="registrar">The container registrar.</param>
        /// <returns>A registration builder used to configure the registration.</returns>
        public static ICakeRegistrationBuilder RegisterType<TImplementation>(this ICakeContainerRegistrar registrar)
        {
            return registrar.RegisterType(typeof(TImplementation));
        }

        /// <summary>
        /// Adds a registration type to an existing registration builder.
        /// </summary>
        /// <typeparam name="TRegistration">The registration type.</typeparam>
        /// <param name="builder">The registration builder.</param>
        /// <returns>The same <see cref="ICakeRegistrationBuilder"/> instance so that multiple calls can be chained.</returns>
        public static ICakeRegistrationBuilder As<TRegistration>(this ICakeRegistrationBuilder builder)
        {
            return builder.As(typeof(TRegistration));
        }
    }
}
