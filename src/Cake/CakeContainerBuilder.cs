// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Autofac;
using Autofac.Builder;
using Cake.Composition;
using Cake.Core.Composition;

namespace Cake
{
    internal sealed class CakeContainerBuilder
    {
        private readonly ContainerRegistry _registry;

        public ICakeContainerRegistry Registry
        {
            get { return _registry; }
        }

        public CakeContainerBuilder()
        {
            _registry = new ContainerRegistry();
        }

        public IContainer Build()
        {
            var builder = new ContainerBuilder();
            AddRegistrations(builder);
            return builder.Build();
        }

        public void Update(IContainer container)
        {
            var builder = new ContainerBuilder();
            AddRegistrations(builder);
            builder.Update(container);
        }

        private void AddRegistrations(ContainerBuilder builder)
        {
            foreach (var registration in _registry.Registrations)
            {
                if (registration.Instance != null)
                {
                    // Register instance.
                    var result = builder.RegisterInstance(registration.Instance);
                    SetRegistrationTypes(result, registration);
                    SetLifetime(result, registration);
                }
                else
                {
                    // Register type.
                    var result = builder.RegisterType(registration.ImplementationType);
                    SetRegistrationTypes(result, registration);
                    SetLifetime(result, registration);
                }
            }
        }

        private static void SetRegistrationTypes<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
            ContainerRegistration registration)
        {
            if (registration.RegistrationTypes.Count > 0)
            {
                foreach (var type in registration.RegistrationTypes)
                {
                    builder.As(type);
                }
            }
            else
            {
                builder.As(registration.ImplementationType);
            }
        }

        private static void SetLifetime<TLimit, TActivatorData, TRegistrationStyle>(
            IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
            ContainerRegistration registration)
        {
            if (registration.IsSingleton)
            {
                builder.SingleInstance();
            }
            else
            {
                builder.InstancePerDependency();
            }
        }
    }
}
