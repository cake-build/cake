// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a framework reference assembly resolver.
    /// </summary>
    public interface IReferenceAssemblyResolver
    {
        /// <summary>
        /// Finds framwork reference assemblies.
        /// </summary>
        /// <returns>The resolved reference assemblies.</returns>
        Assembly[] GetReferenceAssemblies();
    }
}