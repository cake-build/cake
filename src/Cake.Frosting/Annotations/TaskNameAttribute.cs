// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting
{
    /// <summary>
    /// Represents a custom task name.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TaskNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the task name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNameAttribute"/> class.
        /// </summary>
        /// <param name="name">The task name.</param>
        public TaskNameAttribute(string name)
        {
            Name = name;
        }
    }
}
