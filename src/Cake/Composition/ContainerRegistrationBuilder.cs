// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Composition;

namespace Cake.Composition
{
    internal sealed class ContainerRegistrationBuilder<T> : ICakeRegistrationBuilder<T>
    {
        private readonly ContainerRegistration _registration;

        public ContainerRegistration Registration
        {
            get { return _registration; }
        }

        public ContainerRegistrationBuilder(ContainerRegistration registration)
        {
            _registration = registration;
        }

        public ICakeRegistrationBuilder<T> As<TRegistrationType>()
        {
            _registration.RegistrationTypes.Add(typeof(TRegistrationType));
            return this;
        }

        public ICakeRegistrationBuilder<T> AsSelf()
        {
            _registration.RegistrationTypes.Add(typeof(T));
            return this;
        }

        public ICakeRegistrationBuilder<T> Singleton()
        {
            _registration.IsSingleton = true;
            return this;
        }

        public ICakeRegistrationBuilder<T> Transient()
        {
            _registration.IsSingleton = false;
            return this;
        }
    }
}
