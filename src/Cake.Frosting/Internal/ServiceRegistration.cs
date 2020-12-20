// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Composition;
using Microsoft.Extensions.DependencyInjection;

namespace Cake.Frosting.Internal
{
    internal sealed class ServiceRegistration : ICakeRegistrationBuilder
    {
        public Type ImplementationType { get; }
        public object Instance { get; set; }
        public Type ServiceType { get; set; }
        public ServiceLifetime Lifetime { get; set; }

        public ServiceRegistration(Type implementationType)
        {
            ImplementationType = implementationType;
        }

        public ICakeRegistrationBuilder As(Type type)
        {
            ServiceType = type;
            return this;
        }

        public ICakeRegistrationBuilder AsSelf()
        {
            ServiceType = ImplementationType;
            return this;
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
