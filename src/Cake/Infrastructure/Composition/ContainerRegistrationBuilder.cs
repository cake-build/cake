// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Autofac.Builder;
using Cake.Core.Composition;

namespace Cake.Infrastructure.Composition
{
    /// <summary>
    /// Represents a container registration builder.
    /// </summary>
    /// <typeparam name="T">The type being registered.</typeparam>
    /// <typeparam name="TActivator">The activator data type.</typeparam>
    public class ContainerRegistrationBuilder<T, TActivator> : ICakeRegistrationBuilder
            where TActivator : IConcreteActivatorData
    {
        private IRegistrationBuilder<T, TActivator, SingleRegistrationStyle> _registration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerRegistrationBuilder{T, TActivator}"/> class.
        /// </summary>
        /// <param name="registration">The registration builder.</param>
        public ContainerRegistrationBuilder(IRegistrationBuilder<T, TActivator, SingleRegistrationStyle> registration)
        {
            _registration = registration;
        }

        /// <summary>
        /// Registers the type as the specified service type.
        /// </summary>
        /// <param name="type">The service type.</param>
        /// <returns>The registration builder.</returns>
        public ICakeRegistrationBuilder As(Type type)
        {
            _registration = _registration.As(type);
            return this;
        }

        /// <summary>
        /// Registers the type as itself.
        /// </summary>
        /// <returns>The registration builder.</returns>
        public ICakeRegistrationBuilder AsSelf()
        {
            _registration = _registration.AsSelf();
            return this;
        }

        /// <summary>
        /// Registers the type as a singleton.
        /// </summary>
        /// <returns>The registration builder.</returns>
        public ICakeRegistrationBuilder Singleton()
        {
            _registration = _registration.SingleInstance();
            return this;
        }

        /// <summary>
        /// Registers the type as transient.
        /// </summary>
        /// <returns>The registration builder.</returns>
        public ICakeRegistrationBuilder Transient()
        {
            _registration = _registration.InstancePerDependency();
            return this;
        }
    }
}
