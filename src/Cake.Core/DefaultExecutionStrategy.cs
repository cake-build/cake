// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.Diagnostics;

namespace Cake.Core
{
    /// <summary>
    /// The default execution strategy.
    /// </summary>
    public sealed class DefaultExecutionStrategy : IExecutionStrategy
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExecutionStrategy"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public DefaultExecutionStrategy(ICakeLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Performs the setup.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        public void PerformSetup(Action<ICakeContext> action, ICakeContext context)
        {
            if (action != null)
            {
                _log.Information(string.Empty);
                _log.Information("----------------------------------------");
                _log.Information("Setup");
                _log.Information("----------------------------------------");
                _log.Verbose("Executing custom setup action...");

                action(context);
            }
        }

        /// <summary>
        /// Performs the teardown.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        public void PerformTeardown(Action<ICakeContext> action, ICakeContext context)
        {
            if (action != null)
            {
                _log.Information(string.Empty);
                _log.Information("----------------------------------------");
                _log.Information("Teardown");
                _log.Information("----------------------------------------");
                _log.Verbose("Executing custom teardown action...");

                action(context);
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
                _log.Information(string.Empty);
                _log.Information("========================================");
                _log.Information(task.Name);
                _log.Information("========================================");
                _log.Verbose("Executing task: {0}", task.Name);

                task.Execute(context);

                _log.Verbose("Finished executing task: {0}", task.Name);
            }
        }

        /// <summary>
        /// Skips the specified task.
        /// </summary>
        /// <param name="task">The task to skip.</param>
        public void Skip(CakeTask task)
        {
            if (task != null)
            {
                _log.Verbose(string.Empty);
                _log.Verbose("========================================");
                _log.Verbose(task.Name);
                _log.Verbose("========================================");
                _log.Verbose("Skipping task: {0}", task.Name);
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

        /// <summary>
        /// Performs the specified setup action before each task is invoked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        /// <param name="setupContext">The setup context.</param>
        public void PerformTaskSetup(Action<ICakeContext, ITaskSetupContext> action, ICakeContext context, ITaskSetupContext setupContext)
        {
            if (setupContext == null)
            {
                throw new ArgumentNullException("setupContext");
            }
            if (action != null)
            {
                _log.Debug("Executing custom task setup action ({0})...", setupContext.Task.Name);
                action(context, setupContext);
            }
        }

        /// <summary>
        /// Performs the specified teardown action after each task is invoked.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="context">The context.</param>
        /// <param name="teardownContext">The teardown context.</param>
        public void PerformTaskTeardown(Action<ICakeContext, ITaskTeardownContext> action, ICakeContext context, ITaskTeardownContext teardownContext)
        {
            if (teardownContext == null)
            {
                throw new ArgumentNullException("teardownContext");
            }
            if (action != null)
            {
                _log.Debug("Executing custom task teardown action ({0})...", teardownContext.Task.Name);
                action(context, teardownContext);
            }
        }
    }
}
