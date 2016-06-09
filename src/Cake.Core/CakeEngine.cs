// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
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
        private Action<ICakeContext> _setupAction;
        private Action<ICakeContext> _teardownAction;
        private Action<ICakeContext, ITaskSetupContext> _taskSetupAction;
        private Action<ICakeContext, ITaskTeardownContext> _taskTeardownAction;

        /// <summary>
        /// Gets all registered tasks.
        /// </summary>
        /// <value>The registered tasks.</value>
        public IReadOnlyList<CakeTask> Tasks
        {
            get { return _tasks; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeEngine"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public CakeEngine(ICakeLog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
            _tasks = new List<CakeTask>();
        }

        /// <summary>
        /// Creates and registers a new <see cref="ActionTask"/>.
        /// </summary>
        /// <param name="name">The name of the task.</param>
        /// <returns>
        /// A <see cref="CakeTaskBuilder{T}"/> used to configure the task.
        /// </returns>
        public CakeTaskBuilder<ActionTask> RegisterTask(string name)
        {
            if (_tasks.Any(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                const string format = "Another task with the name '{0}' has already been added.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, name));
            }
            var task = new ActionTask(name);
            _tasks.Add(task);
            return new CakeTaskBuilder<ActionTask>(task);
        }

        /// <summary>
        /// Allows registration of an action that's executed before any tasks are run.
        /// If setup fails, no tasks will be executed but teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterSetupAction(Action<ICakeContext> action)
        {
            _setupAction = action;
        }

        /// <summary>
        /// Allows registration of an action that's executed after all other tasks have been run.
        /// If a setup action or a task fails with or without recovery, the specified teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTeardownAction(Action<ICakeContext> action)
        {
            _teardownAction = action;
        }

        /// <summary>
        /// Runs the specified target.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategy">The execution strategy.</param>
        /// <param name="target">The target to run.</param>
        /// <returns>The resulting report.</returns>
        public CakeReport RunTarget(ICakeContext context, IExecutionStrategy strategy, string target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            if (strategy == null)
            {
                throw new ArgumentNullException("strategy");
            }

            var graph = CakeGraphBuilder.Build(_tasks);

            // Make sure target exist.
            if (!graph.Exist(target))
            {
                const string format = "The target '{0}' was not found.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, target));
            }

            // This isn't pretty, but we need to keep track of exceptions thrown
            // while running a setup action, or a task. We do this since we don't
            // want to throw teardown exceptions if an exception was thrown previously.
            var exceptionWasThrown = false;

            try
            {
                PerformSetup(strategy, context);

                var stopWatch = new Stopwatch();
                var report = new CakeReport();

                foreach (var taskNode in graph.Traverse(target))
                {
                    // Get the task.
                    var task = _tasks.FirstOrDefault(x => x.Name.Equals(taskNode, StringComparison.OrdinalIgnoreCase));
                    Debug.Assert(task != null, "Node should not be null.");

                    // Is this the current target?
                    var isTarget = task.Name.Equals(target, StringComparison.OrdinalIgnoreCase);

                    // Should we execute the task?
                    if (ShouldTaskExecute(context, task, isTarget))
                    {
                        ExecuteTask(context, strategy, stopWatch, task, report);
                    }
                    else
                    {
                        SkipTask(context, strategy, task, report);
                    }
                }

                return report;
            }
            catch
            {
                exceptionWasThrown = true;
                throw;
            }
            finally
            {
                PerformTeardown(strategy, context, exceptionWasThrown);
            }
        }

        /// <summary>
        /// Allows registration of an action that's executed before each task is run.
        /// If the task setup fails, the task will not be executed but the task's teardown will be performed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTaskSetupAction(Action<ICakeContext, ITaskSetupContext> action)
        {
            _taskSetupAction = action;
        }

        /// <summary>
        /// Allows registration of an action that's executed after each task has been run.
        /// If a task setup action or a task fails with or without recovery, the specified task teardown action will still be executed.
        /// </summary>
        /// <param name="action">The action to be executed.</param>
        public void RegisterTaskTeardownAction(Action<ICakeContext, ITaskTeardownContext> action)
        {
            _taskTeardownAction = action;
        }

        private void PerformSetup(IExecutionStrategy strategy, ICakeContext context)
        {
            if (_setupAction != null)
            {
                strategy.PerformSetup(_setupAction, context);
            }
        }

        private static bool ShouldTaskExecute(ICakeContext context, CakeTask task, bool isTarget)
        {
            foreach (var criteria in task.Criterias)
            {
                if (!criteria(context))
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
            }
            return true;
        }

        private void ExecuteTask(ICakeContext context, IExecutionStrategy strategy, Stopwatch stopWatch, CakeTask task, CakeReport report)
        {
            // Reset the stop watch.
            stopWatch.Reset();
            stopWatch.Start();

            PerformTaskSetup(context, strategy, task, false);

            bool exceptionWasThrown = false;
            try
            {
                // Execute the task.
                strategy.Execute(task, context);
            }
            catch (Exception exception)
            {
                _log.Error("An error occured when executing task '{0}'.", task.Name);

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
                report.Add(task.Name, stopWatch.Elapsed);
            }
        }

        private void PerformTaskSetup(ICakeContext context, IExecutionStrategy strategy, ICakeTaskInfo task, bool skipped)
        {
            // Trying to stay consistent with the behavior of script-level Setup & Teardown (if setup fails, don't run the task, but still run the teardown)
            if (_taskSetupAction != null)
            {
                try
                {
                    var taskSetupContext = new TaskSetupContext(task);
                    strategy.PerformTaskSetup(_taskSetupAction, context, taskSetupContext);
                }
                catch
                {
                    PerformTaskTeardown(context, strategy, task, TimeSpan.Zero, skipped, true);
                    throw;
                }
            }
        }

        private void PerformTaskTeardown(ICakeContext context, IExecutionStrategy strategy, ICakeTaskInfo task, TimeSpan duration, bool skipped, bool exceptionWasThrown)
        {
            if (_taskTeardownAction != null)
            {
                var taskTeardownContext = new TaskTeardownContext(task, duration, skipped);
                try
                {
                    strategy.PerformTaskTeardown(_taskTeardownAction, context, taskTeardownContext);
                }
                catch (Exception ex)
                {
                    _log.Error("An error occured in the custom task teardown action ({0}).", task.Name);
                    if (!exceptionWasThrown)
                    {
                        // If no other exception was thrown, we throw this one.
                        throw;
                    }
                    _log.Error("Task Teardown error ({0}): {1}", task.Name, ex.ToString());
                }
            }
        }

        private void SkipTask(ICakeContext context, IExecutionStrategy strategy, CakeTask task, CakeReport report)
        {
            PerformTaskSetup(context, strategy, task, true);
            strategy.Skip(task);
            PerformTaskTeardown(context, strategy, task, TimeSpan.Zero, true, false);

            // Add the skipped task to the report.
            report.AddSkipped(task.Name);
        }

        private static bool IsDelegatedTask(CakeTask task)
        {
            var actionTask = task as ActionTask;

            return actionTask != null && !actionTask.Actions.Any();
        }

        private static void ReportErrors(IExecutionStrategy strategy, Action<Exception> errorReporter, Exception taskException)
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

        private void PerformTeardown(IExecutionStrategy strategy, ICakeContext context, bool exceptionWasThrown)
        {
            if (_teardownAction != null)
            {
                try
                {
                    strategy.PerformTeardown(_teardownAction, context);
                }
                catch (Exception ex)
                {
                    _log.Error("An error occured in the custom teardown action.");
                    if (!exceptionWasThrown)
                    {
                        // If no other exception was thrown, we throw this one.
                        throw;
                    }
                    _log.Error("Teardown error: {0}", ex.ToString());
                }
            }
        }
    }
}
