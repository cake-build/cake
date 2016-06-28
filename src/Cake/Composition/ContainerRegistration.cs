// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Composition
{
    internal sealed class ContainerRegistration
    {
        private readonly List<Type> _registrationTypes;

        public Type ImplementationType { get; }

        public ICollection<Type> RegistrationTypes => _registrationTypes;

        public object Instance { get; }

        public bool IsSingleton { get; internal set; }

        public ContainerRegistration(Type implementationType, object instance = null)
        {
            ImplementationType = implementationType;
            _registrationTypes = new List<Type>();
            Instance = instance;
            IsSingleton = true;
        }
    }
}