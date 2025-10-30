// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Spectre.Console.Cli;

namespace Cake.Infrastructure.Composition
{
    /// <summary>
    /// Represents an Autofac-based type resolver.
    /// </summary>
    public sealed class AutofacTypeResolver : ITypeResolver
    {
        private readonly ILifetimeScope _scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacTypeResolver"/> class.
        /// </summary>
        /// <param name="scope">The lifetime scope.</param>
        public AutofacTypeResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// Resolves a type from the container.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>The resolved instance or null if not found.</returns>
        public object Resolve(Type type)
        {
            if (type == null)
            {
                return null;
            }

            return _scope.ResolveOptional(type);
        }
    }
}
