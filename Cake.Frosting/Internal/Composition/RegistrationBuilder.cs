// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.Composition;

namespace Cake.Frosting.Internal.Composition
{
    internal class RegistrationBuilder : ICakeRegistrationBuilder
    {
        private readonly ComponentRegistration _registration;

        public RegistrationBuilder(ComponentRegistration registration)
        {
            _registration = registration;
        }

        public ICakeRegistrationBuilder As(Type type)
        {
            if (!type.GetTypeInfo().IsAssignableFrom(_registration.ImplementationType))
            {
                throw new InvalidOperationException("Invalid registration.");
            }
            _registration.RegistrationTypes.Add(type);
            return this;
        }

        public ICakeRegistrationBuilder AsSelf()
        {
            _registration.RegistrationTypes.Add(_registration.ImplementationType);
            return this;
        }

        public ICakeRegistrationBuilder Singleton()
        {
            _registration.Singleton = true;
            return this;
        }

        public ICakeRegistrationBuilder Transient()
        {
            _registration.Singleton = false;
            return this;
        }
    }
}