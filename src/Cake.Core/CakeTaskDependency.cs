// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Represents a task dependency.
    /// </summary>
    public sealed class CakeTaskDependency
    {
        /// <summary>
        /// Gets the name of the dependency.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a value indicating whether or not the dependency is required.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskDependency"/> class.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <param name="required">Whether or not the dependency is required.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public CakeTaskDependency(string name, bool required)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Required = required;
        }
    }
}