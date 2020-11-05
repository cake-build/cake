// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Internal.Composition
{
    internal static class ContainerExtensions
    {
        public static T Resolve<T>(this Container container)
        {
            return (T)container.Resolve(typeof(T));
        }

        public static object Resolve(this Container container, Type type)
        {
            return ContainerResolver.Resolve(container, type);
        }

        public static void Update(this Container container, CakeServices services)
        {
            services.Builder.Update(container);
        }
    }
}
