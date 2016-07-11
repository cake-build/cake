// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <summary>
    /// Represents a task execution strategy.
    /// </summary>
    public interface IExecutionStrategy
    {
        /// <summary>
        /// Performs the setup.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        void PerformSetup(Action<ICakeContext> action, ICakeContext context);

        /// <summary>
        /// Performs the teardown.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        void PerformTeardown(Action<ICakeContext> action, ICakeContext context);

        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        /// <param name="context">The context.</param>
        void Execute(CakeTask task, ICakeContext context);

        /// <summary>
        /// Skips the specified task.
        /// </summary>
        /// <param name="task">The task to skip.</param>
        void Skip(CakeTask task);

        /// <summary>
        /// Executes the error reporter.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exception">The exception.</param>
        void ReportErrors(Action<Exception> action, Exception exception);

        /// <summary>
        /// Executes the error handler.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exception">The exception.</param>
        void HandleErrors(Action<Exception> action, Exception exception);

        /// <summary>
        /// Invokes the finally handler.
        /// </summary>
        /// <param name="action">The action.</param>
        void InvokeFinally(Action action);

        /// <summary>
        /// Performs the specified setup action before each task is invoked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        /// <param name="setupContext">The setup context.</param>
        void PerformTaskSetup(Action<ICakeContext, ITaskSetupContext> action, ICakeContext context, ITaskSetupContext setupContext);

        /// <summary>
        /// Performs the specified teardown action after each task is invoked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        /// <param name="teardownContext">The teardown context.</param>
        void PerformTaskTeardown(Action<ICakeContext, ITaskTeardownContext> action, ICakeContext context, ITaskTeardownContext teardownContext);
    }
}
