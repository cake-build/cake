// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Cake.Core.Diagnostics;
using Cake.Core.Graph;

namespace Cake.Core
{
    /// <summary>
    /// The Cake execution engine.
    /// </summary>
    public sealed class CakeEngine : ICakeEngine
    {
        private readonly ICakeLog _log;
        private readonly List<CakeTask> _tasks;
        private readonly CakeEngineActions _actions;

        /// <inheritdoc/>
        public IReadOnlyList<ICakeTaskInfo> Tasks => _tasks;

        /// <inheritdoc/>
        public event EventHandler<BeforeSetupEventArgs> BeforeSetup;

        /// <inheritdoc/>
        public event EventHandler<AfterSetupEventArgs> AfterSetup;

        /// <inheritdoc/>
        public event EventHandler<BeforeTeardownEventArgs> BeforeTeardown;

        /// <inheritdoc/>
        public event EventHandler<AfterTeardownEventArgs> AfterTeardown;

        /// <inheritdoc/>
        public event EventHandler<BeforeTaskSetupEventArgs> BeforeTaskSetup;

        /// <inheritdoc/>
        public event EventHandler<AfterTaskSetupEventArgs> AfterTaskSetup;

        /// <inheritdoc/>
        public event EventHandler<BeforeTaskTeardownEventArgs> BeforeTaskTeardown;

        /// <inheritdoc/>
        public event EventHandler<AfterTaskTeardownEventArgs> AfterTaskTeardown;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEngine"/> class.
        /// </summary>
        /// <param name="data">The data service.</param>
        /// <param name="log">The log.</param>
        public CakeEngine(ICakeDataService data, ICakeLog log)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _log = log ?? throw new ArgumentNullException(nameof(log));
            _tasks = new List<CakeTask>();
            _actions = new CakeEngineActions(data);
        }

        /// <inheritdoc/>
        public CakeTaskBuilder RegisterTask(string name)
        {
            if (_tasks.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                const string format = "Another task with the name '{0}' has already been added.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, name));
            }

            var task = new CakeTask(name);
            _tasks.Add(task);
            return new CakeTaskBuilder(task);
        }

        /// <inheritdoc/>
        public void RegisterSetupAction(Action<ISetupContext> action)
        {
            _actions.RegisterSetup(action);
        }

        /// <inheritdoc/>
        public void RegisterSetupAction<TData>(Func<ISetupContext, TData> action)
            where TData : class
        {
            _actions.RegisterSetup(action);
        }

        /// <inheritdoc/>
        public void RegisterTeardownAction(Action<ITeardownContext> action)
        {
            _actions.RegisterTeardown(action);
        }

        /// <inheritdoc/>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTeardownAction<TData>(Action<ITeardownContext, TData> action)
            where TData : class
        {
            _actions.RegisterTeardown(action);
        }

        /// <inheritdoc/>
        public void RegisterTaskSetupAction(Action<ITaskSetupContext> action)
        {
            _actions.RegisterTaskSetup(action);
        }

        /// <inheritdoc/>
        public void RegisterTaskSetupAction<TData>(Action<ITaskSetupContext, TData> action)
            where TData : class
        {
            _actions.RegisterTaskSetup(action);
        }

        /// <inheritdoc/>
        public void RegisterTaskTeardownAction(Action<ITaskTeardownContext> action)
        {
            _actions.RegisterTaskTeardown(action);
        }

        /// <inheritdoc/>
        public void RegisterTaskTeardownAction<TData>(Action<ITaskTeardownContext, TData> action)
            where TData : class
        {
            _actions.RegisterTaskTeardown(action);
        }

        /// <inheritdoc/>
        public async Task<CakeReport> RunTargetAsync(ICakeContext context, IExecutionStrategy strategy, ExecutionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (settings.Targets.Count() == 0)
            {
                throw new ArgumentException("No target specified.", nameof(settings));
            }
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            // Ensure that registered actions are valid.
            _actions.Validate();

            // Create a graph out of the tasks.
            var graph = CakeGraphBuilder.Build(_tasks);

            // Make sure each target exists, prior to attempting to execute them.
            var missingTargets = settings.Targets.Where(target => !graph.Exist(target)).ToArray();
            if (missingTargets.Count() == 1)
            {
                const string format = "The target '{0}' was not found.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, missingTargets[0]));
            }
            else if (missingTargets.Count() > 1)
            {
                const string format = "The targets {0} were not found.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, string.Join(", ", missingTargets.Select(s => $"'{s}'"))));
            }

            // This isn't pretty, but we need to keep track of exceptions thrown
            // while running a setup action, or a task. We do this since we don't
            // want to throw teardown exceptions if an exception was thrown previously.
            var exceptionWasThrown = false;
            Exception thrownException = null;

            var stopWatch = new Stopwatch();
            var report = new CakeReport();

            try
            {
                for (int i = 0; i < settings.Targets.Count(); i++)
                {
                    var target = settings.Targets.ElementAt(i);

                    // Get all nodes to traverse in the correct order.
                    var orderedTasks = graph.Traverse(target)
                        .Select(y => _tasks.FirstOrDefault(x =>
                            x.Name.Equals(y, StringComparison.OrdinalIgnoreCase))).ToArray();

                    // Execute setup once, even if multiple run targets have been specified.
                    if (i == 0)
                    {
                        PerformSetup(context, strategy, orderedTasks, target, stopWatch, report);
                    }

                    await RunTarget(context, strategy, orderedTasks, target, settings.Exclusive, stopWatch, report);
                }

                return report;
            }
            catch (Exception ex)
            {
                exceptionWasThrown = true;
                thrownException = ex;

                throw new CakeReportException(report, ex.Message, ex);
            }
            finally
            {
                PerformTeardown(strategy, context, stopWatch, report, exceptionWasThrown, thrownException);
            }
        }

        private async Task RunTarget(ICakeContext context, IExecutionStrategy strategy, CakeTask[] orderedTasks, string target, bool exclusive, Stopwatch stopWatch, CakeReport report)
        {
            if (exclusive)
            {
                // Execute only the target task.
                var task = _tasks.FirstOrDefault(x => x.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
                await RunTask(context, strategy, task, target, stopWatch, report);
            }
            else
            {
                // Execute all scheduled tasks.
                foreach (var task in orderedTasks)
                {
                    await RunTask(context, strategy, task, target, stopWatch, report);
                }
            }
        }

        private async Task RunTask(ICakeContext context, IExecutionStrategy strategy, CakeTask task, string target, Stopwatch stopWatch, CakeReport report)
        {
            // Is this the current target?
            var isTarget = task.Name.Equals(target, StringComparison.OrdinalIgnoreCase);

            // Should we execute the task?
            var skipped = false;
            foreach (var criteria in task.Criterias)
            {
                if (!ShouldTaskExecute(context, task, criteria, isTarget))
                {
                    criteria.CausedSkippingOfTask = true;
                    SkipTask(context, strategy, task, report, criteria);
                    skipped = true;
                    break;
                }
            }

            if (!skipped)
            {
                await ExecuteTaskAsync(context, strategy, stopWatch, task, report).ConfigureAwait(false);
            }
        }

        private void PerformSetup(ICakeContext context, IExecutionStrategy strategy, CakeTask[] orderedTasks, string target, Stopwatch stopWatch, CakeReport report)
        {
            // Get target node
            var targetTask = orderedTasks
                .FirstOrDefault(node => node.Name.Equals(target, StringComparison.OrdinalIgnoreCase));

            stopWatch.Restart();

            PublishEvent(BeforeSetup, new BeforeSetupEventArgs(context));

            try
            {
                if (_actions.Setups.Count > 0)
                {
                    foreach (var setup in _actions.Setups)
                    {
                        strategy.PerformSetup(setup, new SetupContext(context, targetTask, orderedTasks));
                    }

                    report.Add("Setup", CakeReportEntryCategory.Setup, stopWatch.Elapsed);
                }
            }
            finally
            {
                PublishEvent(AfterSetup, new AfterSetupEventArgs(context));
            }
        }

        private static bool ShouldTaskExecute(ICakeContext context, CakeTask task, CakeTaskCriteria criteria, bool isTarget)
        {
            if (!criteria.Predicate(context))
            {
                if (isTarget)
                {
                    // It's not OK to skip the target task.
                    // See issue #106 (https://github.com/cake-build/cake/issues/106)
                    var message = string.IsNullOrWhiteSpace(criteria.Message) ?
                        $"Could not reach target '{task.Name}' since it was skipped due to a criteria." :
                        $"Could not reach target '{task.Name}' since it was skipped due to a criteria: {criteria.Message}.";

                    throw new CakeException(message);
                }

                return false;
            }

            return true;
        }

        private async Task ExecuteTaskAsync(ICakeContext context, IExecutionStrategy strategy, Stopwatch stopWatch,
            CakeTask task, CakeReport report)
        {
            stopWatch.Restart();

            PerformTaskSetup(context, strategy, task, false);

            Exception taskException = null;
            try
            {
                // Execute the task.
                await strategy.ExecuteAsync(task, context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _log.Error("An error occurred when executing task '{0}'.", task.Name);

                taskException = exception;

                // Got an error reporter?
                if (task.ErrorReporter != null)
                {
                    await ReportErrorsAsync(strategy, task.ErrorReporter, exception);
                }

                // Got an error handler?
                if (task.ErrorHandler != null)
                {
                    await HandleErrorsAsync(strategy, task.ErrorHandler, exception, context);
                }
                else
                {
                    // No error handler defined for this task.
                    // Rethrow the exception and let it propagate.
                    throw;
                }
            }
            finally
            {
                if (task.FinallyHandler != null)
                {
                    await strategy.InvokeFinallyAsync(task.FinallyHandler, context);
                }

                PerformTaskTeardown(context, strategy, task, stopWatch.Elapsed, false, taskException);

                _log.Verbose($"Completed in {stopWatch.Elapsed}");

                // Add the task results to the report
                if (IsDelegatedTask(task))
                {
                    report.AddDelegated(task.Name, stopWatch.Elapsed);
                }
                else if (taskException is null)
                {
                    report.Add(task.Name, CakeReportEntryCategory.Task, stopWatch.Elapsed);
                }
                else
                {
                    report.AddFailed(task.Name, stopWatch.Elapsed);
                }
            }
        }

        private void PerformTaskSetup(ICakeContext context, IExecutionStrategy strategy, ICakeTaskInfo task,
            bool skipped)
        {
            var taskSetupContext = new TaskSetupContext(context, task);
            PublishEvent(BeforeTaskSetup, new BeforeTaskSetupEventArgs(taskSetupContext));

            // Trying to stay consistent with the behavior of script-level Setup & Teardown (if setup fails, don't run the task, but still run the teardown)
            try
            {
                if (_actions.TaskSetup != null)
                {
                    try
                    {
                        strategy.PerformTaskSetup(_actions.TaskSetup, taskSetupContext);
                    }
                    catch (Exception exception)
                    {
                        PerformTaskTeardown(context, strategy, task, TimeSpan.Zero, skipped, exception);
                        throw;
                    }
                }
            }
            finally
            {
                PublishEvent(AfterTaskSetup, new AfterTaskSetupEventArgs(taskSetupContext));
            }
        }

        private void PerformTaskTeardown(ICakeContext context, IExecutionStrategy strategy, ICakeTaskInfo task, TimeSpan duration, bool skipped, Exception taskException)
        {
            var exceptionWasThrown = taskException != null;

            var taskTeardownContext = new TaskTeardownContext(context, task, duration, skipped, taskException);
            PublishEvent(BeforeTaskTeardown, new BeforeTaskTeardownEventArgs(taskTeardownContext));

            try
            {
                if (_actions.TaskTeardown != null)
                {
                    try
                    {
                        strategy.PerformTaskTeardown(_actions.TaskTeardown, taskTeardownContext);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("An error occurred in the custom task teardown action ({0}).", task.Name);
                        if (!exceptionWasThrown)
                        {
                            // If no other exception was thrown, we throw this one.
                            throw;
                        }

                        _log.Error("Task Teardown error ({0}): {1}", task.Name, ex.ToString());
                    }
                }
            }
            finally
            {
                PublishEvent(AfterTaskTeardown, new AfterTaskTeardownEventArgs(taskTeardownContext));
            }
        }

        private void SkipTask(ICakeContext context, IExecutionStrategy strategy, CakeTask task, CakeReport report,
            CakeTaskCriteria criteria)
        {
            PerformTaskSetup(context, strategy, task, true);
            strategy.Skip(task, criteria);
            PerformTaskTeardown(context, strategy, task, TimeSpan.Zero, true, null);

            // Add the skipped task to the report.
            var skippedTaskCriteria = task.Criterias.FirstOrDefault(c => c.CausedSkippingOfTask == true);
            var skippedMessage = string.Empty;
            if (skippedTaskCriteria != null)
            {
                if (!string.IsNullOrEmpty(skippedTaskCriteria.Message))
                {
                    skippedMessage = skippedTaskCriteria.Message;
                }
            }

            report.AddSkipped(task.Name, skippedMessage);
        }

        private static bool IsDelegatedTask(CakeTask task)
        {
            return task != null && !task.Actions.Any();
        }

        private static async Task ReportErrorsAsync(IExecutionStrategy strategy, Func<Exception, Task> errorReporter,
            Exception taskException)
        {
            try
            {
                await strategy.ReportErrorsAsync(errorReporter, taskException);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Ignore errors from the error reporter.
            }
        }

        private async Task HandleErrorsAsync(IExecutionStrategy strategy, Func<Exception, ICakeContext, Task> errorHandler, Exception exception, ICakeContext context)
        {
            try
            {
                await strategy.HandleErrorsAsync(errorHandler, exception, context);
            }
            catch (Exception errorHandlerException)
            {
                if (errorHandlerException != exception)
                {
                    _log.Error("Error: {0}", exception.Message);
                }

                throw;
            }
        }

        private void PerformTeardown(IExecutionStrategy strategy, ICakeContext context, Stopwatch stopWatch,
            CakeReport report, bool exceptionWasThrown, Exception thrownException)
        {
            stopWatch.Restart();

            var teardownContext = new TeardownContext(context, thrownException);
            PublishEvent(BeforeTeardown, new BeforeTeardownEventArgs(teardownContext));

            try
            {
                if (_actions.Teardowns.Count > 0)
                {
                    var exceptions = new List<Exception>();

                    try
                    {
                        foreach (var teardown in _actions.Teardowns)
                        {
                            try
                            {
                                strategy.PerformTeardown(teardown, teardownContext);
                            }
                            catch (Exception ex)
                            {
                                // No other exceptions were thrown and this is the only teardown?
                                if (!exceptionWasThrown && _actions.Teardowns.Count == 1)
                                {
                                    // If no other exception was thrown, we throw this one.
                                    // By doing this we preserve the original stack trace which is always nice.
                                    _log.Error("An error occurred in a custom teardown action.");
                                    throw;
                                }

                                // Add this exception to the list.
                                exceptions.Add(ex);
                            }
                        }
                    }
                    finally
                    {
                        report.Add("Teardown", CakeReportEntryCategory.Teardown, stopWatch.Elapsed);
                    }

                    // If, any exceptions occurred, process them now.
                    if (exceptions.Count > 0)
                    {
                        ProcessTeardownExceptions(exceptions, exceptionWasThrown);
                    }
                }
            }
            finally
            {
                PublishEvent(AfterTeardown, new AfterTeardownEventArgs(teardownContext));
            }
        }

        private void ProcessTeardownExceptions(List<Exception> exceptions, bool exceptionWasThrown)
        {
            if (exceptions.Count > 0)
            {
                _log.Error("An error occurred in a custom teardown action.");

                if (exceptionWasThrown)
                {
                    // Since an earlier exception was thrown, from either a setup method
                    // or a task, we just log all exceptions.
                    foreach (var ex in exceptions)
                    {
                        if (exceptions.Count > 0)
                        {
                            // Output whole stack trace if there's only one exception,
                            // but output the message when there's several.
                            _log.Error("Teardown error: {0}", exceptions.Count == 1 ? ex.ToString() : ex.Message);
                        }
                    }
                }
                else
                {
                    if (exceptions.Count == 1)
                    {
                        // Only a single exception occurred, so lets throw it.
                        // Sadly this won't keep our original stack trace.
                        var ex = exceptions.First();
                        _log.Error("Teardown error: {0}", ex.ToString());
                        throw ex;
                    }

                    // Multiple exceptions occurred, so let's wrap them in an aggregate and throw that one.
                    throw new AggregateException("Multiple teardown methods threw exceptions.", exceptions);
                }
            }
        }

        private void PublishEvent<T>(EventHandler<T> eventHandler, T eventArgs) where T : EventArgs
        {
            if (eventHandler != null)
            {
                foreach (var @delegate in eventHandler.GetInvocationList())
                {
                    var handler = (EventHandler<T>)@delegate;
                    try
                    {
                        handler(this, eventArgs);
                    }
                    catch (Exception e)
                    {
                        _log.Error("An error occurred in the event handler {0}: {1}", handler.GetMethodInfo().Name,
                            e.Message);
                    }
                }
            }
        }
    }
}
