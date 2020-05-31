// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// An attribute used to hint Cake about additional namespaces that need
    /// to be imported for an alias to work. This attribute can mark an
    /// extension method, the extension method class, or the assembly to provide a global set of imports.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class CakeNamespaceImportAttribute : Attribute
    {
        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeNamespaceImportAttribute"/> class.
        /// </summary>
        /// <param name="namespace">The namespace.</param>
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