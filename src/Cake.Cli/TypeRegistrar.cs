// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Cake.Cli;

/// <summary>
/// A type registrar that uses <see cref="IServiceCollection"/>.
/// </summary>
public sealed class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _collection;

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegistrar"/> class.
    /// </summary>
    public TypeRegistrar()
        : this(new ServiceCollection())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeRegistrar"/> class.
    /// </summary>
    /// <param name="collection">The service collection.</param>
    public TypeRegistrar(IServiceCollection collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    /// <inheritdoc/>
    public ITypeResolver Build()
    {
        return new TypeResolver(_collection.BuildServiceProvider());
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public void Register(Type service, Type implementation)
    {
        _collection.AddSingleton(service, implementation);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public void RegisterInstance(Type service, object implementation)
    {
        _collection.AddSingleton(service, implementation);
    }

    /// <inheritdoc/>
    public void RegisterLazy(Type service, Func<object> factory)
    {
        _collection.AddSingleton(service, _ => factory());
    }
}
