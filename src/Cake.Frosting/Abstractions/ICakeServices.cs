// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Composition;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents a collection of service registrations.
    /// </summary>
    /// <seealso cref="ICakeContainerRegistrar" />
    public interface ICakeServices : ICakeContainerRegistrar
    {
    }
}