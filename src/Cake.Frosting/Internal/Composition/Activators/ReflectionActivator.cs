// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cake.Frosting.Internal.Composition.Activators
{
    internal sealed class ReflectionActivator : Activator
    {
        private readonly Type _type;
        private readonly ConstructorInfo _constructor;
        private readonly List<ParameterInfo> _parameters;

        public ReflectionActivator(Type type)
        {
            _type = type;
            _constructor = GetGreediestConstructor(type);
            _parameters = new List<ParameterInfo>();

            var parameters = _constructor.GetParameters();
            foreach (var parameter in parameters)
            {
                _parameters.Add(parameter);
            }
        }

        public override object Activate(Container container)
        {
            var parameters = new object[_parameters.Count];
            for (var i = 0; i < _parameters.Count; i++)
            {
                var parameter = _parameters[i];
                if (parameter.ParameterType == typeof(Container))
                {
                    parameters[i] = container;
                }
                else
                {
                    var resolved = container.Resolve(parameter.ParameterType);
                    if (resolved == null)
                    {
                        if (!parameter.IsOptional)
                        {
                            throw new InvalidOperationException($"Could not find registration for '{parameter.ParameterType.FullName}'.");
                        }
                        parameters[i] = null;
                    }
                    else
                    {
                        parameters[i] = resolved;
                    }
                }
            }
            return _constructor.Invoke(parameters);
        }

        public override Activator CreateCopy()
        {
            return new ReflectionActivator(_type);
        }

        private static ConstructorInfo GetGreediestConstructor(Type type)
        {
            ConstructorInfo current = null;
            var count = -1;
            foreach (var constructor in type.GetTypeInfo().GetConstructors())
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length > count)
                {
                    count = parameters.Length;
                    current = constructor;
                }
            }
            return current;
        }
    }
}