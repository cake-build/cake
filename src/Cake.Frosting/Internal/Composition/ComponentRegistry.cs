// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Frosting.Internal.Composition.Activators;

namespace Cake.Frosting.Internal.Composition
{
    internal sealed class ComponentRegistry : IDisposable
    {
        private readonly Dictionary<Type, HashSet<ComponentRegistration>> _registrations;

        public ComponentRegistry()
        {
            _registrations = new Dictionary<Type, HashSet<ComponentRegistration>>();
        }

        public ComponentRegistry CreateCopy()
        {
            var registry = new ComponentRegistry();
            foreach (var registration in _registrations.SelectMany(p => p.Value))
            {
                registry.Register(registration.CreateCopy());
            }
            return registry;
        }

        public void Dispose()
        {
            foreach (var registration in _registrations)
            {
                registration.Value.Clear();
            }
            _registrations.Clear();
        }

        public void Register(ComponentRegistration registration)
        {
            var types = new HashSet<Type>(registration.RegistrationTypes);
            if (types.Count == 0)
            {
                // Make sure that each registration have at least one registration type.
                registration.RegistrationTypes.Add(registration.ImplementationType);
                types.Add(registration.ImplementationType);
            }

            if (registration.Singleton)
            {
                // Cache singletons after first resolve.
                registration.Activator = new CachingActivator(registration.Activator);
            }

            foreach (var type in types)
            {
                if (!_registrations.ContainsKey(type))
                {
                    _registrations.Add(type, new HashSet<ComponentRegistration>());
                }
                _registrations[type].Add(registration);
            }
        }

        public ICollection<ComponentRegistration> GetRegistrations(Type type)
        {
            if (_registrations.ContainsKey(type))
            {
                return _registrations[type];
            }
            return new List<ComponentRegistration>();
        }
    }
}