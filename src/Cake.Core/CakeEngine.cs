// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<ICakeTaskInfo> Tasks => _tasks;

        /// <summary>
        /// Raised during setup before any tasks are run.
        /// </summary>
        public event EventHandler<SetupEventArgs> Setup;

        /// <summary>
        /// Raised during teardown after all other tasks have been run.
        /// </summary>
        public event EventHandler<TeardownEventArgs> Teardown;

        /// <summary>
        /// Raised before each task is run.
        /// </summary>
        public event EventHandler<TaskSetupEventArgs> TaskSetup;

        /// <summary>
        /// Raised after each task has been run.
        /// </summary>
        public event EventHandler<TaskTeardownEventArgs> TaskTeardown;

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

        /// <summary>
        /// Creates and registers a new Cake task.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>
        /// A <see cref="CakeTaskBuilder"/> used to configure the task.
        /// </returns>
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

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterSetupAction(Action<ISetupContext> action)
        {
            _actions.RegisterSetup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        public void RegisterSetupAction<TData>(Func<ISetupContext, TData> action)
            where TData : class
        {
            _actions.RegisterSetup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTeardownAction(Action<ITeardownContext> action)
        {
            _actions.RegisterTeardown(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTeardownAction<TData>(Action<ITeardownContext, TData> action)
            where TData : class
        {
            _actions.RegisterTeardown(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTaskSetupAction(Action<ITaskSetupContext> action)
        {
            _actions.RegisterTaskSetup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTaskSetupAction<TData>(Action<ITaskSetupContext, TData> action)
            where TData : class
        {
            _actions.RegisterTaskSetup(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTaskTeardownAction(Action<ITaskTeardownContext> action)
        {
            _actions.RegisterTaskTeardown(action);
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <typeparam name="TData">The data type.</typeparam>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTaskTeardownAction<TData>(Action<ITaskTeardownContext, TData> action)
            where TData : class
        {
            _actions.RegisterTaskTeardown(action);
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The execution strategy.</param>
        /// <param name="settings">The execution settings.</param>
        /// <returns>The resulting report.</returns>
        public async Task<CakeReport> RunTargetAsync(ICakeContext context, IExecutionStrategy strategy, ExecutionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (string.IsNullOrWhiteSpace(settings.Target))
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

            // Make sure target exist.
            var target = settings.Target;
            if (!graph.Exist(target))
            {
                const string format = "The target '{0}' was not found.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, target));
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
                // Get all nodes to traverse in the correct order.
                var orderedTasks = graph.Traverse(target)
                    .Select(y => _tasks.FirstOrDefault(x =>
                        x.Name.Equals(y, StringComparison.OrdinalIgnoreCase))).ToArray();

                // Get target node
                var targetNode = orderedTasks
                    .FirstOrDefault(node => node.Name.Equals(target, StringComparison.OrdinalIgnoreCase));

                PerformSetup(strategy, context, targetNode, orderedTasks, stopWatch, report);

                if (settings.Exclusive)
                {
                    // Execute only the target task.
                    var task = _tasks.FirstOrDefault(x => x.Name.Equals(settings.Target, StringComparison.OrdinalIgnoreCase));
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

                return report;
            }
            catch (Exception ex)
            {
                exceptionWasThrown = true;
                thrownException = ex;
                throw;
            }
            finally
            {
                PerformTeardown(strategy, context, stopWatch, report, exceptionWasThrown, thrownException);
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

        private void PerformSetup(IExecutionStrategy strategy, ICakeContext context, CakeTask targetTask,
            CakeTask[] tasks, Stopwatch stopWatch, CakeReport report)
        {
            stopWatch.Restart();

            PublishEvent(Setup, new SetupEventArgs(context));

            if (_actions.Setups.Count > 0)
            {
                foreach (var setup in _actions.Setups)
                {
                    strategy.PerformSetup(setup, new SetupContext(context, targetTask, tasks));
                }

                report.Add("Setup", CakeReportEntryCategory.Setup, stopWatch.Elapsed);
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
                    const string format = "Could not reach target '{0}' since it was skipped due to a criteria.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, task.Name);
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

            var exceptionWasThrown = false;
            try
            {
                // Execute the task.
                await strategy.ExecuteAsync(task, context).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _log.Error("An error occurred when executing task '{0}'.", task.Name);

                exceptionWasThrown = true;

                // Got an error reporter?
                if (task.ErrorReporter != null)
                {
                    ReportErrors(strategy, task.ErrorReporter, exception);
                }

                // Got an error handler?
                if (task.ErrorHandler != null)
                {
                    HandleErrors(strategy, task.ErrorHandler, exception);
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
                    strategy.InvokeFinally(task.FinallyHandler);
                }

                PerformTaskTeardown(context, strategy, task, stopWatch.Elapsed, false, exceptionWasThrown);
            }

            // Add the task results to the report
            if (IsDelegatedTask(task))
            {
                report.AddDelegated(task.Name, stopWatch.Elapsed);
            }
            else
            {
                report.Add(task.Name, CakeReportEntryCategory.Task, stopWatch.Elapsed);
            }
        }

        private void PerformTaskSetup(ICakeContext context, IExecutionStrategy strategy, ICakeTaskInfo task,
            bool skipped)
        {
            var taskSetupContext = new TaskSetupContext(context, task);
            PublishEvent(TaskSetup, new TaskSetupEventArgs(taskSetupContext));
            // Trying to stay consistent with the behavior of script-level Setup & Teardown (if setup fails, don't run the task, but still run the teardown)
            if (_actions.TaskSetup != null)
            {
                try
                {
                    strategy.PerformTaskSetup(_actions.TaskSetup, taskSetupContext);
                }
                catch
                {
                    PerformTaskTeardown(context, strategy, task, TimeSpan.Zero, skipped, true);
                    throw;
                }
            }
        }

        private void PerformTaskTeardown(ICakeContext context, IExecutionStrategy strategy, ICakeTaskInfo task,
            TimeSpan duration, bool skipped, bool exceptionWasThrown)
        {
            var taskTeardownContext = new TaskTeardownContext(context, task, duration, skipped);
            PublishEvent(TaskTeardown, new TaskTeardownEventArgs(taskTeardownContext));
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

        private void SkipTask(ICakeContext context, IExecutionStrategy strategy, CakeTask task, CakeReport report,
            CakeTaskCriteria criteria)
        {
            PerformTaskSetup(context, strategy, task, true);
            strategy.Skip(task, criteria);
            PerformTaskTeardown(context, strategy, task, TimeSpan.Zero, true, false);

            // Add the skipped task to the report.
            report.AddSkipped(task.Name);
        }

        private static bool IsDelegatedTask(CakeTask task)
        {
            return task != null && !task.Actions.Any();
        }

        private static void ReportErrors(IExecutionStrategy strategy, Action<Exception> errorReporter,
            Exception taskException)
        {
            try
            {
                strategy.ReportErrors(errorReporter, taskException);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Ignore errors from the error reporter.
            }
        }

        private void HandleErrors(IExecutionStrategy strategy, Action<Exception> errorHandler, Exception exception)
        {
            try
            {
                strategy.HandleErrors(errorHandler, exception);
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
            PublishEvent(Teardown, new TeardownEventArgs(teardownContext));

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

                // If, any exceptions occured, process them now.
                if (exceptions.Count > 0)
                {
                    ProcessTeardownExceptions(exceptions, exceptionWasThrown);
                }
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
                        // Only a single exception occured, so lets throw it.
                        // Sadly this won't keep our original stack trace.
                        var ex = exceptions.First();
                        _log.Error("Teardown error: {0}", ex.ToString());
                        throw ex;
                    }

                    // Multiple exceptions occured, so let's wrap them in an aggregate and throw that one.
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