// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;

namespace Cake.Composition
{
    internal sealed class ContainerRegistration
    {
        private readonly Type _implementationType;
        private readonly List<Type> _registrationTypes;
        private readonly object _instance;
        private bool _isSingleton;

        public Type ImplementationType
        {
            get { return _implementationType; }
        }

        public ICollection<Type> RegistrationTypes
        {
            get { return _registrationTypes; }
        }

        public object Instance
        {
            get { return _instance; }
        }

        public bool IsSingleton
        {
            get { return _isSingleton; }
            internal set { _isSingleton = value; }
        }

        public ContainerRegistration(Type implementationType, object instance = null)
        {
            _implementationType = implementationType;
            _registrationTypes = new List<Type>();
            _instance = instance;
            _isSingleton = true;
        }
    }
}
