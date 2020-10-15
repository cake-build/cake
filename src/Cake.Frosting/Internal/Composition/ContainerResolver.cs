// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cake.Frosting.Internal.Composition
{
    internal static class ContainerResolver
    {
        public static object Resolve(Container container, Type type)
        {
            var isEnumerable = false;
            if (type.GetTypeInfo().IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    isEnumerable = true;
                    type = type.GenericTypeArguments[0];
                }
            }

            var registrations = container.Registry.GetRegistrations(type);
            if (registrations != null)
            {
                if (isEnumerable)
                {
                    var result = Array.CreateInstance(type, registrations.Count);
                    for (var index = 0; index < registrations.Count; index++)
                    {
                        var registration = registrations.ElementAt(index);
                        result.SetValue(container.Resolve(registration), index);
                    }
                    return result;
                }
            }

            return container.Resolve(registrations?.LastOrDefault());
        }
    }
}
