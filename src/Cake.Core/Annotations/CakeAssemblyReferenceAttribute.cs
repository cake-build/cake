// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// Causes an assembly to be referenced by scripts.
    /// Can be applied to module assemblies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class CakeAssemblyReferenceAttribute : Attribute
    {
        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeAssemblyReferenceAttribute"/> class.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly to reference in scripts.</param>
        public CakeAssemblyReferenceAttribute(string assemblyName)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
        }
    }
}
