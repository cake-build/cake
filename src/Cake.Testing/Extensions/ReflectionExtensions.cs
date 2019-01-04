// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;

namespace Cake.Testing.Extensions
{
    /// <summary>
    /// Contains extension methods for types in System.Reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Indicates whether the specified type implements the specified interface definition.
        /// If <paramref name="interfaceTypeDefinition"/> is generic, it must not be a generic type instantiation.
        /// </summary>
        /// <param name="type">The type to examine.</param>
        /// <param name="interfaceTypeDefinition">
        /// The interface definition to find.
        /// If <paramref name="interfaceTypeDefinition"/> is generic, it must not be a generic type instantiation.
        /// </param>
        /// <exception cref="ArgumentNullException">A required argument was <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="interfaceTypeDefinition"/> was not an interface type or was a generic type instantiation.
        /// </exception>
        /// <returns>
        /// <see langword="true"/> if  the specified type implements the specified interface definition; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool SatisfiesInterfaceDefinition(this Type type, Type interfaceTypeDefinition)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (interfaceTypeDefinition == null)
            {
                throw new ArgumentNullException(nameof(interfaceTypeDefinition));
            }

            if (!interfaceTypeDefinition.IsInterface)
            {
                throw new ArgumentException("An interface type definition must be specified.", nameof(interfaceTypeDefinition));
            }

            if (!interfaceTypeDefinition.IsGenericType)
            {
                if (type == interfaceTypeDefinition)
                {
                    return true;
                }

                return type.GetInterfaces().Contains(interfaceTypeDefinition);
            }

            if (interfaceTypeDefinition.IsConstructedGenericType)
            {
                throw new ArgumentException("An interface type definition must be specified.", nameof(interfaceTypeDefinition));
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == interfaceTypeDefinition)
            {
                return true;
            }

            return type.GetInterfaces().Any(@interface =>
                @interface.IsGenericType
                && @interface.GetGenericTypeDefinition() == interfaceTypeDefinition);
        }
    }
}
