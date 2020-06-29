// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

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
        void PerformSetup(Action<ISetupContext> action, ISetupContext context);

        /// <summary>
        /// Performs the teardown.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="teardownContext">The context.</param>
        void PerformTeardown(Action<ITeardownContext> action, ITeardownContext teardownContext);

        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        /// <param name="context">The context.</param>
        /// <returns>Returned Task.</returns>
        Task ExecuteAsync(CakeTask task, ICakeContext context);

        /// <summary>
        /// Skips the specified task.
        /// </summary>
        /// <param name="task">The task to skip.</param>
        /// <param name="criteria">The criteria that caused the task to be skipped.</param>
        void Skip(CakeTask task, CakeTaskCriteria criteria);

        /// <summary>
        /// Executes the error reporter.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exception">The exception.</param>
        /// <returns>The awaitable task.</returns>
        Task ReportErrorsAsync(Func<Exception, Task> action, Exception exception);

        /// <summary>
        /// Executes the error handler.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context.</param>
        /// <returns>The awaitable task.</returns>
        Task HandleErrorsAsync(Func<Exception, ICakeContext, Task> action, Exception exception, ICakeContext context);

        /// <summary>
        /// Invokes the finally handler.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The awaitable task.</returns>
        Task InvokeFinallyAsync(Func<Task> action);

        /// <summary>
        /// Performs the specified setup action before each task is invoked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="taskSetupContext">The context.</param>
        void PerformTaskSetup(Action<ITaskSetupContext> action, ITaskSetupContext taskSetupContext);

        /// <summary>
        /// Performs the specified teardown action after each task is invoked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="taskTeardownContext">The context.</param>
        void PerformTaskTeardown(Action<ITaskTeardownContext> action, ITaskTeardownContext taskTeardownContext);
    }
}