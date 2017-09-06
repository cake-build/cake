// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// Causes a namespace to be imported by scripts.
    /// Can be applied to addin and module assemblies, as well as alias methods and classes in addin assemblies.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class CakeNamespaceImportAttribute : Attribute
    {
        /// <summary>
        /// Gets the namespace.
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeNamespaceImportAttribute"/> class.
        /// </summary>
        /// <param name="namespace">The namespace to import into scripts.</param>
        public CakeNamespaceImportAttribute(string @namespace)
        {
            if (@namespace == null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }
            Namespace = @namespace;
        }
    }
}