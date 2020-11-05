// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Internal
{
    internal static class MethodInfoExtensions
    {
        public static bool IsOverriden(this MethodInfo method)
        {
            return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }
    }
}
