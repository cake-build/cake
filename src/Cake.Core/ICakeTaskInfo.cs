// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Provides descriptive properties about a cake task
    /// </summary>
    public interface ICakeTaskInfo
    {
        /// <summary>
        /// Gets the name of the task.
        /// </summary>
        /// <value>The name of the task.</value>
        string Name { get; }

        /// <summary>
        /// Gets the description of the task.
        /// </summary>
        /// <value>The description of the task.</value>
        string Description { get; }

        /// <summary>
        /// Gets the task's dependencies.
        /// </summary>
        /// <value>The task's dependencies.</value>
        IReadOnlyList<string> Dependencies { get; }
    }
}
