// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace Cake.Frosting.Internal
{
    internal sealed class ServiceCollectionAdapter : ICakeContainerRegistrar
    {
        private readonly List<ServiceRegistration> _registrations;

        public ServiceCollectionAdapter()
        {
            _registrations = new List<ServiceRegistration>();
        }

        public ICakeRegistrationBuilder RegisterInstance<TImplementation>(TImplementation instance)
            where TImplementation : class
        {
            var registration = new ServiceRegistration(typeof(TImplementation))
            {
                Instance = instance,
                Lifetime = ServiceLifetime.Singleton,
                ServiceType = typeof(TImplementation),
            };

            _registrations.Add(registration);
            return registration;
        }

        public ICakeRegistrationBuilder RegisterType(Type type)
        {
            var registration = new ServiceRegistration(type)
            {
                Lifetime = ServiceLifetime.Transient,
                ServiceType = type,
            };

            _registrations.Add(registration);
            return registration;
        }

        public void Transfer(IServiceCollection services)
        {
            foreach (var registration in _registrations)
            {
                if (registration.Instance != null)
                {
                    var descriptor = ServiceDescriptor.Describe(registration.ServiceType, f => registration.Instance, registration.Lifetime);
                    services.Add(descriptor);
                }
                else
                {
                    var descriptor = ServiceDescriptor.Describe(registration.ServiceType, registration.ImplementationType, registration.Lifetime);
                    services.Add(descriptor);
                }
            }
        }
    }
}
