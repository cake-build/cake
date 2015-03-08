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
        void PerformSetup(Action action);

        /// <summary>
        /// Performs the teardown.
        /// </summary>
        /// <param name="action">The action.</param>
        void PerformTeardown(Action action);

        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        /// <param name="context">The context.</param>
        void Execute(CakeTask task, ICakeContext context);

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
    }
}
