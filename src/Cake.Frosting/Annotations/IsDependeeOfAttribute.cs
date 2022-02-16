// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting
{
    /// <summary>
    /// Represents a reverse dependency.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class IsDependeeOfAttribute : Attribute, IReverseTaskDependency
    {
        /// <summary>
        /// Gets the reverse dependency task type.
        /// </summary>
        /// <value>The reverse dependency task type.</value>
        public Type Task { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsDependeeOfAttribute"/> class.
        /// </summary>
        /// <param name="type">The reverse dependency type.</param>
        public IsDependeeOfAttribute(Type type)
        {
            Task = type;
        }
    }
}
