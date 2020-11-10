// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting
{
    /// <summary>
    /// Represents a dependency.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class IsDependentOnAttribute : Attribute, ITaskDependency
    {
        /// <summary>
        /// Gets the dependency task type.
        /// </summary>
        /// <value>The dependency task type.</value>
        public Type Task { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IsDependentOnAttribute"/> class.
        /// </summary>
        /// <param name="type">The dependency type.</param>
        public IsDependentOnAttribute(Type type)
        {
            Task = type;
        }
    }
}
