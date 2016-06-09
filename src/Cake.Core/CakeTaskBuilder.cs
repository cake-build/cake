// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <summary>
    /// Allows configuration to be performed for a registered <see cref="CakeTask"/>.
    /// </summary>
    /// <typeparam name="T">The task type.</typeparam>
    public sealed class CakeTaskBuilder<T>
        where T : CakeTask
    {
        private readonly T _task;

        /// <summary>
        /// Gets the task being configured.
        /// </summary>
        /// <value>The task being configured.</value>
        public T Task
        {
            get { return _task; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeTaskBuilder{T}"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public CakeTaskBuilder(T task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            _task = task;
        }
    }
}
