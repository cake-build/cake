// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents a dependency.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependencyAttribute : Attribute
    {
        /// <summary>
        /// Gets the depdendency task type.
        /// </summary>
        /// <value>The dependency task type.</value>
        public Type Task { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyAttribute"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public DependencyAttribute(Type type)
        {
            Task = type;
        }
    }
}
