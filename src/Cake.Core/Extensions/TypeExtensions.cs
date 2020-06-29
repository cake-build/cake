// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="Type"/>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the specified <see cref="Type"/> is static.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Whether or not the specified type is static.</returns>
        public static bool IsStatic(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsAbstract && typeInfo.IsSealed;
        }

        /// <summary>
        /// Gets the full name of a <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="includeNamespace">if set to <c>true</c> then namespace is included.</param>
        /// <returns>The full name of a type.</returns>
        public static string GetFullName(this Type type, bool includeNamespace = true)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            if (type.IsArray)
            {
                return type.GetElementType().GetFullName(includeNamespace) + "[]";
            }

            // fix any nested types by a simple replacement + will become .
            Type genericType;
            return (type.IsGenericType(out genericType)
                ? GetGenericTypeName(genericType, includeNamespace)
                : includeNamespace ? type.FullName : type.Name).Replace('+', '.');
        }

        /// <summary>
        /// Gets whether or not a given <see cref="System.Type"/> is a subclass of an raw/open generic.
        /// </summary>
        /// <param name="toCheck">The type to check.</param>
        /// <param name="generic">The open generic to test.</param>
        /// <code>typeof(Nullable&lt;int&gt;).IsSubclassOfRawGeneric(typeof(Nullable&lt;&gt;));.</code>
        /// <returns>Returns <c>true</c> if the type is a subtype of a raw generic, else <c>false</c>.</returns>
        public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.GetTypeInfo().IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.GetTypeInfo().BaseType;
            }
            return false;
        }

        private static string GetGenericTypeName(this Type type, bool includeNamespace)
        {
            var builder = new StringBuilder();
            if (includeNamespace)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }
            builder.Append(type.Name.Substring(0, type.Name.IndexOf('`')));
            builder.Append("<");
            builder.Append(GetGenericTypeArguments(type, includeNamespace));
            builder.Append(">");
            return builder.ToString();
        }

        private static bool IsGenericType(this Type type, out Type genericType)
        {
            genericType = type.IsByRef
                ? (type.GetElementType() ?? type)
                : type;
            return genericType.GetTypeInfo().IsGenericType;
        }

        private static string GetGenericTypeArguments(this Type type, bool includeNamespace)
        {
            var genericArguments = new List<string>();
            foreach (var argument in type.GenericTypeArguments)
            {
                genericArguments.Add(GetFullName(argument, includeNamespace));
            }
            return string.Join(", ", genericArguments);
        }
    }
}