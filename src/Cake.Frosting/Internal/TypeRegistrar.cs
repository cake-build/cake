// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cake.Frosting.Internal
{
    internal sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _collection;

        public TypeRegistrar()
        {
            _collection = new ServiceCollection();
        }

        public ITypeResolver Build()
        {
            return new TypeResolver(_collection.BuildServiceProvider());
        }

        [DebuggerStepThrough]
        public void Register(Type service, Type implementation)
        {
            _collection.AddSingleton(service, implementation);
        }

        [DebuggerStepThrough]
        public void RegisterInstance(Type service, object implementation)
        {
            _collection.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> factory)
        {
            _collection.AddSingleton(service, _ => factory());
        }
    }
}
