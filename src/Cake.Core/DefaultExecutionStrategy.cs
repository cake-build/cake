// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;

namespace Cake.Core
{
    /// <summary>
    /// The default execution strategy.
    /// </summary>
    public sealed class DefaultExecutionStrategy : IExecutionStrategy
    {
        private readonly ICakeLog _log;
        private readonly ICakeReportPrinter _reportPrinter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultExecutionStrategy"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="reportPrinter">The report printer.</param>
        public DefaultExecutionStrategy(ICakeLog log, ICakeReportPrinter reportPrinter)
        {
            _log = log;
            _reportPrinter = reportPrinter;
        }

        /// <inheritdoc/>
        public void PerformSetup(Action<ISetupContext> action, ISetupContext context)
        {
            if (action != null)
            {
                _reportPrinter.WriteLifeCycleStep("Setup", _log.Verbosity);

                _log.Verbose("Executing custom setup action...");

                action(context);
            }
        }

        /// <inheritdoc/>
        public void PerformTeardown(Action<ITeardownContext> action, ITeardownContext teardownContext)
        {
            if (teardownContext == null)
            {
                throw new ArgumentNullException(nameof(teardownContext));
            }
            if (action != null)
            {
                _reportPrinter.WriteLifeCycleStep("TearDown", _log.Verbosity);

                _log.Verbose("Executing custom teardown action...");

                action(teardownContext);
            }
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(CakeTask task, ICakeContext context)
        {
            if (task != null)
            {
                _reportPrinter.WriteStep(task.Name, _log.Verbosity);

                _log.Verbose("Executing task: {0}", task.Name);

                await task.Execute(context).ConfigureAwait(false);

                _log.Verbose("Finished executing task: {0}", task.Name);
            }
        }

        /// <inheritdoc/>
        public void Skip(CakeTask task, CakeTaskCriteria criteria)
        {
            if (task != null)
            {
                _reportPrinter.WriteSkippedStep(task.Name, _log.Verbosity);

                var message = string.IsNullOrWhiteSpace(criteria.Message)
                    ? task.Name : criteria.Message;

                _log.Verbose("Skipping task: {0}", message);
            }
        }

        /// <inheritdoc/>
        public async Task ReportErrorsAsync(Func<Exception, Task> action, Exception exception)
        {
            if (action is null)
            {
                return;
            }

            await action(exception);
        }

        /// <inheritdoc/>
        public async Task HandleErrorsAsync(Func<Exception, ICakeContext, Task> action, Exception exception, ICakeContext context)
        {
            if (action is null)
            {
                return;
            }

            await action(exception, context);
        }

        /// <inheritdoc/>
        public async Task InvokeFinallyAsync(Func<ICakeContext, Task> action, ICakeContext context)
        {
            if (action is null)
            {
                return;
            }

            await action(context);
        }

        /// <inheritdoc/>
        public void PerformTaskSetup(Action<ITaskSetupContext> action, ITaskSetupContext taskSetupContext)
        {
            if (taskSetupContext == null)
            {
                throw new ArgumentNullException(nameof(taskSetupContext));
            }
            if (action != null)
            {
                _log.Debug("Executing custom task setup action ({0})...", taskSetupContext.Task.Name);
                action(taskSetupContext);
            }
        }

        /// <inheritdoc/>
        public void PerformTaskTeardown(Action<ITaskTeardownContext> action, ITaskTeardownContext taskTeardownContext)
        {
            if (taskTeardownContext == null)
            {
                throw new ArgumentNullException(nameof(taskTeardownContext));
            }
            if (action != null)
            {
                _log.Debug("Executing custom task teardown action ({0})...", taskTeardownContext.Task.Name);
                action(taskTeardownContext);
            }
        }
    }
}