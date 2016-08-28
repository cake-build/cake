// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Frosting
{
    /// <summary>
    /// A <see cref="IFrostingTask"/> represents a unit of work.
    /// </summary>
    public interface IFrostingTask
    {
        /// <summary>
        /// Runs the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        void Run(ICakeContext context);

        /// <summary>
        /// Gets whether or not the task should be run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the task should run; otherwise <c>false</c>.
        /// </returns>
        bool ShouldRun(ICakeContext context);
    }
}
