// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Frosting.Internal.Composition
{
    internal sealed class ComponentRegistration
    {
        public Type ImplementationType { get; }

        public List<Type> RegistrationTypes { get; }

        public bool Singleton { get; set; }

        public Activator Activator { get; set; }

        public ComponentRegistration(Type type)
        {
            ImplementationType = type;
            RegistrationTypes = new List<Type>();
            Singleton = true;
            Activator = null;
        }

        public ComponentRegistration CreateCopy()
        {
            var registration = new ComponentRegistration(ImplementationType);
            registration.RegistrationTypes.AddRange(RegistrationTypes);
            registration.Singleton = Singleton;
            registration.Activator = Activator?.CreateCopy();
            return registration;
        }
    }
}