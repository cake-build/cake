// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Internal
{
    internal static class CakeTaskExtensions
    {
        public static bool HasCompatibleContext(this IFrostingTask task, IFrostingContext context)
        {
            return context.GetType().IsConvertableTo(task.GetContextType());
        }

        public static Type GetContextType(this IFrostingTask task)
        {
            var baseType = task.GetType().GetTypeInfo().BaseType;
            if (baseType.IsConstructedGenericType)
            {
                if (baseType.GetGenericTypeDefinition() == typeof(FrostingTask<>))
                {
                    return baseType.GenericTypeArguments[0];
                }
            }
            return typeof(ICakeContext);
        }

        private static bool IsConvertableTo(this Type type, Type other)
        {
            return other == type || other.IsAssignableFrom(type);
        }
    }
}
