// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core
{
    /// <summary>
    /// The execution status of a <see cref="CakeTask"/>.
    /// </summary>
    public enum CakeTaskExecutionStatus
    {
        /// <summary>
        /// The task was executed.
        /// </summary>
        Executed,

        /// <summary>
        /// The task delegated execution.
        /// </summary>
        Delegated,

        /// <summary>
        /// The task was skipped.
        /// </summary>
        Skipped
    }
}
