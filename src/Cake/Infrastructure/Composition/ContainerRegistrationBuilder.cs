// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Autofac.Builder;
using Cake.Core.Composition;

namespace Cake.Infrastructure.Composition
{
    public class ContainerRegistrationBuilder<T, TActivator> : ICakeRegistrationBuilder
            where TActivator : IConcreteActivatorData
    {
        private IRegistrationBuilder<T, TActivator, SingleRegistrationStyle> _registration;

        public ContainerRegistrationBuilder(IRegistrationBuilder<T, TActivator, SingleRegistrationStyle> registration)
        {
            _registration = registration;
        }

        public ICakeRegistrationBuilder As(Type type)
        {
            _registration = _registration.As(type);
            return this;
        }

        public ICakeRegistrationBuilder AsSelf()
        {
            _registration = _registration.AsSelf();
            return this;
        }

        public ICakeRegistrationBuilder Singleton()
        {
            _registration = _registration.SingleInstance();
            return this;
        }

        public ICakeRegistrationBuilder Transient()
        {
            _registration = _registration.InstancePerDependency();
            return this;
        }
    }
}
