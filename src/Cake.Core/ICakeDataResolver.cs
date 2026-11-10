// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core;

/// <summary>
/// Represents a data context resolver.
/// </summary>
public interface ICakeDataResolver
{
    /// <summary>
    /// Gets the data of the specified type.
    /// </summary>
    /// <typeparam name="TData">The data type.</typeparam>
    /// <returns>The value of the data.</returns>
    /// <example>
    /// <code>
    /// Setup&lt;Foo&gt;(context => new Foo { Place = "World" });
    ///
    /// Task("Hello")
    ///     .Does(context =>
    /// {
    ///     var data = context.Data.Get&lt;Foo&gt;();
    ///     Information("Hello {0}", data.Place);
    /// });
    /// </code>
    /// </example>
    TData Get<TData>() where TData : class;
}
