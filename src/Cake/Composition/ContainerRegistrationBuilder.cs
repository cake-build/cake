// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;

namespace Cake.Composition
{
    internal sealed class ContainerRegistrationBuilder<T> : ICakeRegistrationBuilder<T>
    {
        public ContainerRegistration Registration { get; }

        public ContainerRegistrationBuilder(ContainerRegistration registration)
        {
            Registration = registration;
        }

        public ICakeRegistrationBuilder<T> As<TRegistrationType>()
        {
            Registration.RegistrationTypes.Add(typeof(TRegistrationType));
            return this;
        }

        public ICakeRegistrationBuilder<T> AsSelf()
        {
            Registration.RegistrationTypes.Add(typeof(T));
            return this;
        }

        public ICakeRegistrationBuilder<T> Singleton()
        {
            Registration.IsSingleton = true;
            return this;
        }

        public ICakeRegistrationBuilder<T> Transient()
        {
            Registration.IsSingleton = false;
            return this;
        }
    }
}