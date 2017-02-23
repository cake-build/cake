// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// A <see cref="CakeTaskDependency"/> represents a dependency between two tasks.
    /// </summary>
    public class CakeTaskDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskDependency"/> class.
        /// </summary>
        /// <param name="targetTaskName">Name of the target task.</param>
        public CakeTaskDependency(string targetTaskName)
            : this(targetTaskName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskDependency"/> class.
        /// </summary>
        /// <param name="targetTaskName">Name of the target task.</param>
        /// <param name="ignoreIfNotExists">Value indicating whether the dependency should be
        /// ignored if not task with <paramref name="targetTaskName"/> exists, or if an error
        /// should be thrown.</param>
        public CakeTaskDependency(string targetTaskName, bool ignoreIfNotExists)
        {
            if (string.IsNullOrWhiteSpace(targetTaskName))
            {
                throw new ArgumentNullException(nameof(targetTaskName));
            }

            TargetTaskName = targetTaskName;
            IgnoreIfNotExists = ignoreIfNotExists;
        }

        /// <summary>
        /// Gets the name of the target task.
        /// </summary>
        public string TargetTaskName { get; }

        /// <summary>
        /// Gets a value indicating whether the dependency should be ignored if not task
        /// with <see cref="TargetTaskName"/> exists, or if an error should be thrown.
        /// </summary>
        public bool IgnoreIfNotExists { get; }
    }
}
