// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

// ReSharper disable once CheckNamespace
namespace Cake.Frosting
{
    /// <summary>
    /// Represents the lifetime for all tasks.
    /// </summary>
    public interface IFrostingTaskLifetime
    {
        /// <summary>
        /// This method is executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The setup information.</param>
        void Setup(ICakeContext context, ITaskSetupContext info);

        /// <summary>
        /// This method is executed after each task have been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The teardown information.</param>
        void Teardown(ICakeContext context, ITaskTeardownContext info);
    }
}