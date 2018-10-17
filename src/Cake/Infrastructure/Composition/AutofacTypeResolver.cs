// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Autofac;
using Spectre.Cli;

namespace Cake.Infrastructure.Composition
{
    public sealed class AutofacTypeResolver : ITypeResolver
    {
        private readonly ILifetimeScope _scope;

        public AutofacTypeResolver(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public object Resolve(Type type)
        {
            return _scope.Resolve(type);
        }
    }
}
