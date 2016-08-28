// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Autofac;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting.Internal.Composition
{
    internal static class ContainerExtensions
    {
        public static void Update(this IContainer container, ContainerRegistrar registrar)
        {
            registrar.Builder.Update(container);
        }
    }
}