// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace Cake.Cli;

/// <summary>
/// An <see cref="ICakeContainerRegistrar"/> that registers services into an <see cref="IServiceCollection"/>.
/// </summary>
public sealed class ContainerRegistrar : ICakeContainerRegistrar
{
    private readonly IServiceCollection _services;
    private readonly List<ContainerRegistration> _registrations;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContainerRegistrar"/> class.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public ContainerRegistrar(IServiceCollection services)
    {
        _services = services ?? throw new ArgumentNullException(nameof(services));
        _registrations = new List<ContainerRegistration>();
    }

    /// <inheritdoc/>
    public ICakeRegistrationBuilder RegisterInstance<TImplementation>(TImplementation instance)
        where TImplementation : class
    {
        ArgumentNullException.ThrowIfNull(instance);

        var registration = new ContainerRegistration(typeof(TImplementation))
        {
            Instance = instance,
            Lifetime = ServiceLifetime.Singleton
        };

        _registrations.Add(registration);
        return registration;
    }

    /// <inheritdoc/>
    public ICakeRegistrationBuilder RegisterType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var registration = new ContainerRegistration(type)
        {
            Lifetime = ServiceLifetime.Transient
        };

        _registrations.Add(registration);
        return registration;
    }

    /// <summary>
    /// Transfers pending registrations to the service collection.
    /// </summary>
    public void Transfer()
    {
        foreach (var registration in _registrations)
        {
            var serviceTypes = registration.ServiceTypes.Count > 0
                ? (IReadOnlyList<Type>)registration.ServiceTypes
                : new[] { registration.ImplementationType };

            if (registration.Instance != null)
            {
                var instance = registration.Instance;
                foreach (var serviceType in serviceTypes)
                {
                    _services.Add(ServiceDescriptor.Describe(serviceType, _ => instance, registration.Lifetime));
                }

                continue;
            }

            if (registration.Lifetime == ServiceLifetime.Singleton && serviceTypes.Count > 1)
            {
                _services.Add(ServiceDescriptor.Describe(
                    registration.ImplementationType,
                    registration.ImplementationType,
                    ServiceLifetime.Singleton));

                foreach (var serviceType in serviceTypes)
                {
                    if (serviceType == registration.ImplementationType)
                    {
                        continue;
                    }

                    _services.Add(ServiceDescriptor.Describe(
                        serviceType,
                        provider => provider.GetRequiredService(registration.ImplementationType),
                        ServiceLifetime.Singleton));
                }

                continue;
            }

            foreach (var serviceType in serviceTypes)
            {
                _services.Add(ServiceDescriptor.Describe(
                    serviceType,
                    registration.ImplementationType,
                    registration.Lifetime));
            }
        }

        _registrations.Clear();
    }

    /// <summary>
    /// Transfers pending registrations and builds the service provider.
    /// </summary>
    /// <returns>The built service provider.</returns>
    public ServiceProvider BuildServiceProvider()
    {
        Transfer();
        return _services.BuildServiceProvider();
    }

    private sealed class ContainerRegistration : ICakeRegistrationBuilder
    {
        public Type ImplementationType { get; }
        public object Instance { get; set; }
        public List<Type> ServiceTypes { get; } = new List<Type>();
        public ServiceLifetime Lifetime { get; set; }

        public ContainerRegistration(Type implementationType)
        {
            ImplementationType = implementationType;
        }

        public ICakeRegistrationBuilder As(Type type)
        {
            ArgumentNullException.ThrowIfNull(type);
            if (!ServiceTypes.Contains(type))
            {
                ServiceTypes.Add(type);
            }

            return this;
        }

        public ICakeRegistrationBuilder AsSelf()
        {
            return As(ImplementationType);
        }

        public ICakeRegistrationBuilder Singleton()
        {
            Lifetime = ServiceLifetime.Singleton;
            return this;
        }

        public ICakeRegistrationBuilder Transient()
        {
            Lifetime = ServiceLifetime.Transient;
            return this;
        }
    }
}
