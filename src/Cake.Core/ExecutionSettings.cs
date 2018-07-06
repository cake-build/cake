// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core
{
    /// <summary>
    /// Contains settings related to execution of the script.
    /// </summary>
    public sealed class ExecutionSettings
    {
        /// <summary>
        /// Gets the target to be executed.
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// Gets a value indicating whether or not to use the target exclusively.
        /// </summary>
        public bool Exclusive { get; private set; }

        /// <summary>
        /// Sets the target to be executed.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>The same <see cref="ExecutionSettings"/> instance so that multiple calls can be chained.</returns>
        public ExecutionSettings SetTarget(string target)
        {
            Target = target;
            return this;
        }

        /// <summary>
        /// Whether or not to use the target exclusively.
        /// </summary>
        /// <returns>The same <see cref="ExecutionSettings"/> instance so that multiple calls can be chained.</returns>
        public ExecutionSettings UseExclusiveTarget()
        {
            Exclusive = true;
            return this;
        }
    }
}