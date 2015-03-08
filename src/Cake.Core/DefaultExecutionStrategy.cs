using System;

namespace Cake.Core
{
    /// <summary>
    /// The default execution strategy.
    /// </summary>
    internal sealed class DefaultExecutionStrategy : IExecutionStrategy
    {
        /// <summary>
        /// Performs the setup.
        /// </summary>
        /// <param name="action">The action.</param>
        public void PerformSetup(Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// Performs the teardown.
        /// </summary>
        /// <param name="action">The action.</param>
        public void PerformTeardown(Action action)
        {
            if (action != null)
            {
                action();
            }
        }

        /// <summary>
        /// Executes the specified task.
        /// </summary>
        /// <param name="task">The task to execute.</param>
        /// <param name="context">The context.</param>
        public void Execute(CakeTask task, ICakeContext context)
        {
            if (task != null)
            {
                task.Execute(context);
            }
        }

        /// <summary>
        /// Executes the error reporter.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exception">The exception.</param>
        public void ReportErrors(Action<Exception> action, Exception exception)
        {
            if (action != null)
            {
                action(exception);
            }
        }

        /// <summary>
        /// Executes the error handler.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="exception">The exception.</param>
        public void HandleErrors(Action<Exception> action, Exception exception)
        {
            if (action != null)
            {
                action(exception);
            }
        }

        /// <summary>
        /// Invokes the finally handler.
        /// </summary>
        /// <param name="action">The action.</param>
        public void InvokeFinally(Action action)
        {
            if (action != null)
            {
                action();
            }
        }
    }
}
