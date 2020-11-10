// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting
{
    /// <summary>
    /// Represents a task description.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class TaskDescriptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the task description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The task description.</param>
        public TaskDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}
