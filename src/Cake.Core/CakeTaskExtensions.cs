// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="CakeTask"/>.
    /// </summary>
    public static class CakeTaskExtensions
    {
        /// <summary>
        /// Adds a criteria to the task that is invoked when the task is invoked.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="criteria">The criteria.</param>
        public static void AddCriteria(this CakeTask task, Func<bool> criteria)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            if (criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            task.AddCriteria(context => criteria());
        }
    }
}
