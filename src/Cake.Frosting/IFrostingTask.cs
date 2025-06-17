﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake.Frosting
{
    /// <summary>
    /// A <see cref="IFrostingTask"/> represents a unit of work.
    /// </summary>
    public interface IFrostingTask
    {
        /// <summary>
        /// Gets ShouldRunCriteria for this task.
        /// </summary>
        List<CakeTaskCriteria> ShouldRunCriteria { get; }

        /// <summary>
        /// Runs the task using the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RunAsync(ICakeContext context);

        /// <summary>
        /// Gets whether or not the task should be run.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the task should run; otherwise <c>false</c>.
        /// </returns>
        [Obsolete("ShouldRun method will be removed in a future version")]
        bool ShouldRun(ICakeContext context);

        /// <summary>
        /// Gets message used for criteria if <see cref="ShouldRun(ICakeContext)" /> returns <c>false</c>.
        /// </summary>
        string SkippedMessage => string.Empty;

        /// <summary>
        /// The error handler to be executed if an exception occurs in the task.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context.</param>
        void OnError(Exception exception, ICakeContext context);

        /// <summary>
        /// The finally handler to be executed after the task have finished executing.
        /// </summary>
        /// <param name="context">The context.</param>
        void Finally(ICakeContext context);
    }
}
