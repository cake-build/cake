// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// An attribute used to identify a module implementation in an assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public sealed class CakeModuleAttribute : Attribute
    {
        /// <summary>
        /// Gets the module type.
        /// </summary>
        /// <value>The module type.</value>
        public Type ModuleType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeModuleAttribute"/> class.
        /// </summary>
        /// <param name="moduleType">The module type.</param>
        public CakeModuleAttribute(Type moduleType)
        {
            if (moduleType == null)
            {
                throw new ArgumentNullException(nameof(moduleType));
            }

            ModuleType = moduleType;
        }
    }
}